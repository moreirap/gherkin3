using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gherkin.GRLCatalogueGenerator
{
    public class Contributions
    {
        public IDictionary<string,Quality> QualityContributions { get; set; }

        public static string GetContributionAsQuantity(string contributionType)
        {
            switch (contributionType)
            {
                case "Break":
                    return "-100";
                case "Help":
                    return "25";
                case "Hurt":
                    return "-25";
                case "Make":
                    return "100";
                case "SomeNegative":
                    return "-75";
                case "SomePositive":
                    return "75";
                case "Unknown":
                    return "0";
                default:
                    return "0";
            }
        }
    }
}
