using System;
using System.Linq;

namespace Gherkin.Ast
{
    public class Actor : IHasLocation
    {
        public Location Location { get; private set; }
        public string Name { get; private set; }
        public string Keyword { get; private set; }

        public Actor(Location location, string keyword,string name)
        {
            Name = name;
            Location = location;
            Keyword = keyword;
        }
    }
}