﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gherkin.Ast;

namespace Gherkin
{
    public class AstBuilder<T> : IAstBuilder<T>
    {
        private readonly Stack<AstNode> stack = new Stack<AstNode>();
        private AstNode CurrentNode { get { return stack.Peek(); } }
        private List<Comment> comments = new List<Comment>();

        public AstBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            stack.Clear();
            stack.Push(new AstNode(RuleType.None));
            comments.Clear();
        }

        public void Build(Token token)
        {
            if (token.MatchedType == TokenType.Comment)
            {
                comments.Add(new Comment(GetLocation(token), token.MatchedText));
            }
            else
            {
                CurrentNode.Add((RuleType) token.MatchedType, token);
            }
        }

        public void StartRule(RuleType ruleType)
        {
            stack.Push(new AstNode(ruleType));
        }

        public void EndRule(RuleType ruleType)
        {
            var node = stack.Pop();
            object transformedNode = GetTransformedNode(node);
            CurrentNode.Add(node.RuleType, transformedNode);
        }

        public T GetResult()
        {
            return CurrentNode.GetSingle<T>(RuleType.Feature);
        }

        private object GetTransformedNode(AstNode node)
        {
            switch (node.RuleType)
            {
                case RuleType.Step:
                {
                    var stepLine = node.GetToken(TokenType.StepLine);
                    var stepArg = node.GetSingle<StepArgument>(RuleType.DataTable) ??
                                  node.GetSingle<StepArgument>(RuleType.DocString) ??
                                  null; // empty arg
                    return new Step(GetLocation(stepLine), stepLine.MatchedKeyword, stepLine.MatchedText, stepArg);
                }
                case RuleType.DocString:
                {
                    var separatorToken = node.GetTokens(TokenType.DocStringSeparator).First();
                    var contentType = separatorToken.MatchedText;
                    var lineTokens = node.GetTokens(TokenType.Other);
                    var content = string.Join(Environment.NewLine, lineTokens.Select(lt => lt.MatchedText));

                    return new DocString(GetLocation(separatorToken), contentType, content);
                }
                case RuleType.DataTable:
                {
                    var rows = GetTableRows(node);
                    return new DataTable(rows);
                }
                case RuleType.Background:
                {
                    var backgroundLine = node.GetToken(TokenType.BackgroundLine);
                    var description = GetDescription(node);
                    var steps = GetSteps(node);
                    return new Background(GetLocation(backgroundLine), backgroundLine.MatchedKeyword, backgroundLine.MatchedText, description, steps);
                }
                case RuleType.Scenario_Definition:
                {
                    var tags = GetTags(node);

                    var scenarioNode = node.GetSingle<AstNode>(RuleType.Scenario);
                    if (scenarioNode != null)
                    {
                        var scenarioLine = scenarioNode.GetToken(TokenType.ScenarioLine);

                        var description = GetDescription(scenarioNode);
                        var steps = GetSteps(scenarioNode);

                        return new Scenario(tags, GetLocation(scenarioLine), scenarioLine.MatchedKeyword, scenarioLine.MatchedText, description, steps);
                    }
                    else
                    {
                        var scenarioOutlineNode = node.GetSingle<AstNode>(RuleType.ScenarioOutline);
                        if (scenarioOutlineNode == null)
                            throw new InvalidOperationException("Internal gramar error");
                        var scenarioOutlineLine = scenarioOutlineNode.GetToken(TokenType.ScenarioOutlineLine);

                        var description = GetDescription(scenarioOutlineNode);
                        var steps = GetSteps(scenarioOutlineNode);
                        var examples = scenarioOutlineNode.GetItems<Examples>(RuleType.Examples_Definition).ToArray();

                        return new ScenarioOutline(tags, GetLocation(scenarioOutlineLine), scenarioOutlineLine.MatchedKeyword, scenarioOutlineLine.MatchedText, description, steps, examples);
                    }
                }
                case RuleType.Examples_Definition:
                {
                    var tags = GetTags(node);
                    var examplesNode = node.GetSingle<AstNode>(RuleType.Examples);
                    var examplesLine = examplesNode.GetToken(TokenType.ExamplesLine);
                    var description = GetDescription(examplesNode);

                    var allRows = GetTableRows(examplesNode);
                    var header = allRows.First();
                    var rows = allRows.Skip(1).ToArray();
                    return new Examples(tags, GetLocation(examplesLine), examplesLine.MatchedKeyword, examplesLine.MatchedText, description, header, rows);
                }
                case RuleType.Description:
                {
                    var lineTokens = node.GetTokens(TokenType.Other);

                    // Trim trailing empty lines
                    lineTokens = lineTokens.Reverse().SkipWhile(t => string.IsNullOrWhiteSpace(t.MatchedText)).Reverse();

                    return string.Join(Environment.NewLine, lineTokens.Select(lt => lt.MatchedText));
                }
                case RuleType.Quality_Step:
                {
                    var qualityLine = node.GetToken(TokenType.QualityAttributeLine);
                    var qualities = node.GetSingle<DataTable>(RuleType.DataTable);
                    return new QualityAttributes(GetLocation(qualityLine), qualityLine.MatchedKeyword, qualities);               
                }
                case RuleType.Feature_Description:
                {
                    var tags = GetTags(node);

                    var actorLine = node.GetToken(TokenType.AsA_Step);
                    var actor = new Actor(GetLocation(actorLine), actorLine.MatchedKeyword, actorLine.MatchedText);

                    var goalLine = node.GetToken(TokenType.IWant_Step);
                    var goal = new Goal(GetLocation(goalLine), goalLine.MatchedKeyword, goalLine.MatchedText);

                    var benefitLine = node.GetToken(TokenType.SoThat_Step);
                    var benefit = new Benefit(GetLocation(benefitLine), benefitLine.MatchedKeyword, benefitLine.MatchedText);

                    var qualities = node.GetSingle<QualityAttributes>(RuleType.Quality_Step);

                    return new FeatureDescription(tags,GetLocation(actorLine), actor, goal, benefit,qualities);
                }
                case RuleType.Feature:
                {
                    var header = node.GetSingle<AstNode>(RuleType.Feature_Header);
                    if(header == null) return null;
                    var tags = GetTags(header);
                    var featureLine = header.GetToken(TokenType.FeatureLine);
                    if(featureLine == null) return null;
                    var featureDescription = header.GetSingle<FeatureDescription>(RuleType.Feature_Description);                   
                    var background = node.GetSingle<Background>(RuleType.Background);
                    var scenariodefinitions = node.GetItems<ScenarioDefinition>(RuleType.Scenario_Definition).ToArray();
                    var description = GetDescription(header);
                    if(featureLine.MatchedGherkinDialect == null) return null;
                    var language = featureLine.MatchedGherkinDialect.Language;

                    return new Feature(tags, GetLocation(featureLine), language, featureLine.MatchedKeyword, featureLine.MatchedText, featureDescription, background, scenariodefinitions, comments.ToArray());
                }
            }

            return node;
        }

        private Location GetLocation(Token token, int column = 0)
        {
            return column == 0 ? token.Location : new Location(token.Location.Line, column);
        }

        private Tag[] GetTags(AstNode node)
        {
            var tagsNode = node.GetSingle<AstNode>(RuleType.Tags);
            if (tagsNode == null)
                return new Tag[0];

            return tagsNode.GetTokens(TokenType.TagLine)
                .SelectMany(t => t.MatchedItems, (t, tagItem) =>
                    new Tag(GetLocation(t, tagItem.Column), tagItem.Text))
                .ToArray();
        }

        private TableRow[] GetTableRows(AstNode node)
        {
            var rows = node.GetTokens(TokenType.TableRow).Select(token => new TableRow(GetLocation(token), GetCells(token))).ToArray();
            EnsureCellCount(rows);
            return rows;
        }

        private void EnsureCellCount(TableRow[] rows)
        {
            if (rows.Length == 0)
                return;

            int cellCount = rows[0].Cells.Count();
            foreach (var row in rows)
            {
                if (row.Cells.Count() != cellCount)
                {
                    throw new AstBuilderException("inconsistent cell count within the table", row.Location);
                }
            }
        }

        private TableCell[] GetCells(Token tableRowToken)
        {
            return tableRowToken.MatchedItems
                .Select(cellItem => new TableCell(GetLocation(tableRowToken, cellItem.Column), cellItem.Text))
                .ToArray();
        }

        private static Step[] GetSteps(AstNode scenarioDefinitionNode)
        {
            return scenarioDefinitionNode.GetItems<Step>(RuleType.Step).ToArray();
        }

        private static string GetDescription(AstNode scenarioDefinitionNode)
        {
            return scenarioDefinitionNode.GetSingle<string>(RuleType.Description);
        }
    }
}
