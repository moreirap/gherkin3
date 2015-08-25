using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gherkin.Ast
{
    public class ScenarioContribution : IHasLocation
    {
        private string[] keywords = new[] { "Breaking the ", "Which breaks ", "Contributing to break ",
                        "Helping the ",  "Which helps ",  "Contributing to help ",
                        "Hurting the ",  "Which hurts ",  "Contributing to hurt ",
                        "Making the ",   "Which makes ",  "Contributing to make ",
                        "With some positive contribution to ",
                        "With some negative contribution to "};

        public Location Location { get; private set; }
        public string Keyword { get; private set; }
        public string GoalOrQuality { get; set; }
        public string Contribution { get; set; }

        public ScenarioContribution(Location location , string keyword , string goalOrQuality)
        {
            Location = location;
            Keyword = keyword;
            Contribution = GetContributionFromKeyword(keyword);
            GoalOrQuality = goalOrQuality;
        }

        private string GetContributionFromKeyword(string keyword)
        {
            var firstWord  = keyword.Split()[0];
            var secondWord = keyword.Split()[1];
            switch (firstWord)
            {
                case "Breaking":
                    return "Break";
                case "Helping":
                    return "Help";
                case "Hurting":
                    return "Hurt";
                case "Making":
                    return "Make";
                case "Which":
                    switch (secondWord)
                    {
                        case "breaks":
                            return "Break";
                        case "helps":
                            return "Help";
                        case "hurts":
                            return "Hurt";
                        case "makes":
                            return "Make";
                        default:
                            break;
                    }
                    break;
                case "Contributing":
                    return Char.ToUpper(keyword.Split()[2][0]) + keyword.Split()[2].Substring(1);
                case "With":
                    return keyword.Contains("positive") ? "SomePositive" : "SomeNegative";
                default:
                    return "Unknown";
            }
            return "Unknown"; // Default to Unknown
        }
    }
}
