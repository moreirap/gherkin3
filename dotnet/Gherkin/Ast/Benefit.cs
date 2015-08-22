using System;
using System.Linq;

namespace Gherkin.Ast
{
    public class Benefit : IHasLocation
    {
        public Location Location { get; private set; }
        public string Description { get; private set; }
        public string Keyword { get; private set; }

        public Benefit(Location location, string keyword, string description)
        {
            Description = description;
            Location = location;
            Keyword = keyword;
        }
    }
}