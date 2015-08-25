using System;
using System.Linq;

namespace Gherkin.Ast
{
    public class Scenario : ScenarioDefinition
    {
        public Scenario(Tag[] tags, Location location, string keyword, string name, string description, Step[] steps,ScenarioContribution[] scenarioContributions)
            : base(tags, location, keyword, name, description, steps, scenarioContributions)
        {
        }
    }
}