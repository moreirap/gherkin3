using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Gherkin.Ast;

namespace Gherkin.GRLCatalogueGenerator
{
    public class GRLCatalogueGenerator
    {
        static string[] DEFAULT_QUALITY_SCENARIOS_TAGS      = new[] { "@NFR"   , "@QUALITY"};
        static string[] GLOBAL_FEATURE_TAGS                 = new[] { "@GLOBAL", "@CROSS-CUTTING" };
        static string DEFAULT_SCENARIO_TO_GOAL_CONTRIBUTION = "Help";

        GRLElementsContainer container;

        public GRLCatalogueGenerator()
        {
            // Initialise a new container to keep track of GRL Elements
            container = new GRLElementsContainer();
        }

        public void UpdateGRLCatalogue(grlcatalog grlCatalogue, Ast.Feature parsingResult)
        {
            // Add new actor element or retrieve existing one from container
            var actorElement = AddActorToGRLCatalogue(parsingResult, grlCatalogue);

            // Add new goal (This is the "I want to..." goal of a BDD Feature)
            var goalElement = AddFeatureGoalToGRLCatalogue(parsingResult, grlCatalogue);

            // Attach goal to Actor
            BindElementToActorAndUpdateGRLCatalogue(grlCatalogue, actorElement, goalElement);

            // Are there any non-functionals?
            if (parsingResult.Description != null && parsingResult.Description.QualityAttributes != null && parsingResult.Description.QualityAttributes.Qualities.Length > 0)
            {
                // Add Qualities
                AddQualitiesToGRLCatalogue(parsingResult, grlCatalogue, actorElement);

                // Add Contribution Links from qualities to goal
                AddQualityGoalContributionsToGRLCatalogue(parsingResult, grlCatalogue, goalElement);
            }

            // Are there any scenarios?
            if (parsingResult.ScenarioDefinitions != null && parsingResult.ScenarioDefinitions.Count() > 0) 
            {
                // Add contributions from scenarios to goal
                AddScenarioGoalContributionsToGRLCatalogue(parsingResult, grlCatalogue, goalElement);
            }
        }

        /// <summary>
        /// Adds dependencies between any goals that have been defined as part of "Which may impact" feature descriptions
        /// Note this method should be invoked after all features have been processed to ensure all goals have already been added, so dependencies can be set
        /// </summary>
        /// <param name="grlCatalogue">The catalogue of GRL elements with all goals already defined</param>
        /// <param name="parsingResult">The returned AST from parsing a Gherkin feature</param>
        public void UpdateGRLCatalogueWithImpactedGoals(grlcatalog grlCatalogue, Ast.Feature parsingResult) 
        {
            var goalElement = container.GetElementByName<grlcatalogIntentionalelement>(parsingResult.Description.Goal.Description);
            foreach (var impactedGoal in parsingResult.Description.ImpactedGoals)
            {
                var impactedGoalElement = container.GetElementByName<grlcatalogIntentionalelement>(impactedGoal.Description);
                if (impactedGoalElement == null)
                    throw new ApplicationException(String.Format("Referring impacted goal that does not exist: {0}. Are you including all feature files?", impactedGoal.Description));
                var dependencyElement = BuildDependencyElement(goalElement, impactedGoalElement);
                grlCatalogue.linkdef.dependency = grlCatalogue.linkdef.dependency.Union(new[] { dependencyElement } ).ToArray();
            }
        }

        /// <summary>
        /// Adds dependencies between any goals that have been tagged as Global goals. This usually refers to cross-cuttting concerns or qualities that apply to all goals
        /// Note this method should be invoked after all features have been processed to ensure all goals have already been added, so dependencies can be set
        /// </summary>
        /// <param name="grlCatalogue">The catalogue of GRL elements with all goals already defined</param>
        /// <param name="parsingResult">The returned AST from parsing a Gherkin feature</param>
        public void UpdateGRLCatalogueWithGlobalGoals(grlcatalog grlCatalogue, Ast.Feature[] parsingResults)
        {
            foreach (var parsingResult in parsingResults)
            {
                var goalElement = container.GetElementByName<grlcatalogIntentionalelement>(parsingResult.Description.Goal.Description);
                if (parsingResult.Description != null && HasGlobalTag(parsingResult.Description))
                {
                    foreach (var intentionalElement in grlCatalogue.elementdef)
                    {
                        var otherGoalElement = container.GetElementByName<grlcatalogIntentionalelement>(intentionalElement.name);
                        var isGlobalGoal = parsingResults.Any(f => String.Compare(f.Description.Goal.Description,otherGoalElement.name,true)==0 && HasGlobalTag(f.Description));

                        if (goalElement.id != otherGoalElement.id && intentionalElement.type == "Goal" && !isGlobalGoal)
                        {
                            // Avoid adding a dependency if a reverse dependency already exists. For example 2 global goals A and B would create 
                            // two dependencies A->B and B->A and that would break the GRL Catalogue consistency
                            var dependencyElement = BuildDependencyElement(goalElement, otherGoalElement);
                            grlCatalogue.linkdef.dependency = grlCatalogue.linkdef.dependency.Union(new[] { dependencyElement }).ToArray();
                        }
                    }
                }
            }
        }

        private bool HasGlobalTag(IHasTags elementWithTags) 
        {
            return elementWithTags!= null && elementWithTags.Tags.Any(tag => GLOBAL_FEATURE_TAGS.Contains(tag.Name));
        }


        private void AddScenarioGoalContributionsToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue, grlcatalogIntentionalelement goalElement)
        {
            var scenarios = parsingResult.ScenarioDefinitions;
            foreach (var scenario in scenarios)
            {
                var scenarioExists = false;
                var scenarioElement = container.RegisterElement<grlcatalogIntentionalelement>(scenario.Name,out scenarioExists);
                scenarioElement.type = "Task";
                if (scenarioExists)
                    throw new ApplicationException("Scenario already exists: " + scenarioElement.name);

                // Add Scenario as a Task to GRL Catalogue
                grlCatalogue.elementdef = grlCatalogue.elementdef.Union(new[] { scenarioElement }).ToArray();

                // Only add contribution from scenario to goal if not an NFR-only scenario
                if (scenario.Tags == null || !scenario.Tags.Any(tag => DEFAULT_QUALITY_SCENARIOS_TAGS.Contains(tag.Name)))
                {
                    // Add default contribution from scenario to goal if there is no contribution specified in Scenario Contributions for Goal
                    if (!scenario.ScenarioContributions.Any(sc => String.Compare(sc.GoalOrQuality, goalElement.name, true) == 0))
                    {
                        var contributionElement = BuildContributionElement(DEFAULT_SCENARIO_TO_GOAL_CONTRIBUTION, scenarioElement, goalElement);
                        grlCatalogue.linkdef.contribution = grlCatalogue.linkdef.contribution.Union(new[] { contributionElement }).ToArray();
                    }
                }
                
                // Add scenario contributions
                foreach (var scenarioContribution in scenario.ScenarioContributions)
                {
                    var contributedGoal = container.GetElementByName<grlcatalogIntentionalelement>(scenarioContribution.GoalOrQuality);

                    // Add contribution from to goal
                    var contributionElement = BuildContributionElement(scenarioContribution.Contribution, scenarioElement, contributedGoal);
                    grlCatalogue.linkdef.contribution = grlCatalogue.linkdef.contribution.Union(new[] { contributionElement }).ToArray();
                }
            } 
        }

        private grlcatalogActor AddActorToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue)
        {
            var actor = parsingResult.Description.Actor;

            // Create or obtain actor element from container
            var actorExists = false;
            var actorElement = container.RegisterElement<grlcatalogActor>(actor.Name, out actorExists);

            // Only add to catalogue if not already present
            if (!actorExists)
                grlCatalogue.actordef = grlCatalogue.actordef.Union(new[] { actorElement }).ToArray();

            return actorElement;
        }
        
        private grlcatalogIntentionalelement AddFeatureGoalToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue)
        {
            var goal = parsingResult.Description.Goal;

            // Create intentional element corresponding to goal (This should not exist in catalogue already)
            var goalExists = false;
            var goalElement = container.RegisterElement<grlcatalogIntentionalelement>(goal.Description, out goalExists);
            if (goalExists)
                throw new ApplicationException("Goal already processed:" + goal.Description);

            goalElement.type = "Goal";
            goalElement.decompositiontype = "And";

            // Add goal element to catalogue
            grlCatalogue.elementdef = grlCatalogue.elementdef.Union(new[] { goalElement }).ToArray();
            return goalElement;
        }

        private void AddQualitiesToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue,  grlcatalogActor actorElement)
        {
            var qualities = parsingResult.Description.QualityAttributes.Qualities;
            foreach (var q in qualities)
            {
                AddQualityAttributeToGRLCatalogue(grlCatalogue, actorElement, q.Name);
                //AddQualityAttributeToGRLCatalogue(grlCatalogue, actorElement, q.Description);
            }
        }

        private void AddQualityAttributeToGRLCatalogue(grlcatalog grlCatalogue,  grlcatalogActor actorElement, string qualityName)
        {
            var qualityExists = false;
            var qualityElement = container.RegisterElement<grlcatalogIntentionalelement>(qualityName, out qualityExists);
            if (!qualityExists)
            {
                qualityElement.type = "Softgoal";
                qualityElement.decompositiontype = "And";
                // Add intentional element to GRL catalogue
                grlCatalogue.elementdef = grlCatalogue.elementdef.Union(new[] { qualityElement }).ToArray();

            }
        }

        private void BindElementToActorAndUpdateGRLCatalogue(grlcatalog grlCatalogue, grlcatalogActor actorElement, grlcatalogIntentionalelement boundedElement)
        {
            var bindindName = String.Format("Bind {0} to Actor {1}",boundedElement.id,actorElement.id);
            var bindingExists = false;
            var actorContIE = container.RegisterElement<grlcatalogActorContIE>(bindindName, out bindingExists);
            actorContIE.actor = actorElement.id;
            actorContIE.ie = boundedElement.id;
            // Bind element to actor by adding the actor->element link to GRL catalogue
            grlCatalogue.actorIElinkdef = grlCatalogue.actorIElinkdef.Union(new[] { actorContIE }).ToArray();
        }

        

        private void AddQualityGoalContributionsToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue,  grlcatalogIntentionalelement goalElement)
        {
            var qualities = parsingResult.Description.QualityAttributes.Qualities;
            foreach (var q in qualities)
            {
                var qualityNameElement = container.GetElementByName<grlcatalogIntentionalelement>(q.Name);
                    
                var contributionElement = BuildContributionElement(q.Contribution, qualityNameElement,goalElement);
                grlCatalogue.linkdef.contribution = grlCatalogue.linkdef.contribution.Union(new[] { contributionElement }).ToArray();

            }    
        }

        private grlcatalogLinkdefDecomposition BuildDecompositionElement(grlcatalogIntentionalelement sourceElement, grlcatalogIntentionalelement targetElement)
        {
            // Set decomposition between quality name and description
            var decomposition_name = String.Format("Decomposition_From_{0}_To_{1}", sourceElement.id, targetElement.id);
            var decompositionExists = false;
            var decompositionElement = container.RegisterElement<grlcatalogLinkdefDecomposition>(decomposition_name, out decompositionExists);
            if (!decompositionExists)
            {
                decompositionElement.srcid = sourceElement.id;
                decompositionElement.destid = targetElement.id;      
            }
            return decompositionElement;
        }

        private grlcatalogLinkdefContribution BuildContributionElement(string contribution, grlcatalogIntentionalelement sourceElement, grlcatalogIntentionalelement targetElement)
        {
            // Set contribution between quality and goal
            var contribution_name = String.Format("Contribution_From_{0}_To_{1}", sourceElement.id, targetElement.id);
            var contributionExists = false;
            var contributionElement = container.RegisterElement<grlcatalogLinkdefContribution>(contribution_name, out contributionExists);
            if (!contributionExists)
            {
                contributionElement.srcid = sourceElement.id;
                contributionElement.destid = targetElement.id;
                contributionElement.contributiontype = contribution;
                contributionElement.quantitativeContribution = grlcatalog.GetContributionAsQuantity(contribution);
                contributionElement.correlation = false;
            }
            return contributionElement;
        }

        private grlcatalogLinkdefDependency BuildDependencyElement(grlcatalogIntentionalelement sourceElement, grlcatalogIntentionalelement targetElement)
        {
            // Set contribution between quality and goal
            var contribution_name = String.Format("Dependency_From_{0}_To_{1}", sourceElement.id, targetElement.id);
            var contributionExists = false;
            var contributionElement = container.RegisterElement<grlcatalogLinkdefDependency>(contribution_name, out contributionExists);
            if (!contributionExists)
            {
                contributionElement.dependerid = targetElement.id;
                contributionElement.dependeeid = sourceElement.id;
            }
            return contributionElement;
        }


        internal void AppendQualityCatalogue(QualityCatalogue defaultQualityCatalogue, grlcatalog grlCatalogue)
        {
            var qualities = defaultQualityCatalogue.Qualities;

            foreach (var q in qualities)
            {
                var qualityExists = false;
                var qualityElement = container.RegisterElement<grlcatalogIntentionalelement>(q.Name, out qualityExists);
                if (qualityExists)
                    throw new ApplicationException("When building default catalogues, all elements should be unique: " + q.Name + " exists!");

                // Add intentional element to GRL catalogue
                qualityElement.type = "Softgoal";
                grlCatalogue.elementdef = grlCatalogue.elementdef.Union(new[] { qualityElement }).ToArray();
            }   
            
            foreach (var q in qualities)
            {
                var qualityExists = false;
                var qualityElement = container.RegisterElement<grlcatalogIntentionalelement>(q.Name, out qualityExists);
                if (q.Decomposition != null)
                {
                    qualityElement.decompositiontype = q.Decomposition.Type.ToString();
                    foreach (var subQuality in q.Decomposition.SubQualities)
                    {
                        var subQualityExists = false;
                        var subqualityElement = container.RegisterElement<grlcatalogIntentionalelement>(subQuality.Name, out subQualityExists);
                        
                        if (subQualityExists)
                            throw new ApplicationException("When building default catalogues, all elements should be unique: " + subQuality.Name + " exists!");
                        
                        // Add subquality to catalogue
                        subqualityElement.type = "Softgoal";
                        grlCatalogue.elementdef = grlCatalogue.elementdef.Union(new[] { subqualityElement }).ToArray();

                        // Add Decomposition to GRL Catalogue
                        var decompositionElement = BuildDecompositionElement(subqualityElement, qualityElement);
                        grlCatalogue.linkdef.decomposition = grlCatalogue.linkdef.decomposition.Union(new[] { decompositionElement }).ToArray();
                    }
                }        
            }   
        }
    }
}
