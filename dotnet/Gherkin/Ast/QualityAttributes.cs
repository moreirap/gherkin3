using System;
using System.Linq;
using System.Collections.Generic;

namespace Gherkin.Ast
{
    public class QualityAttributes: IHasLocation
    {
        public Location Location { get; private set; }
        public string Keyword { get; private set; }
        public Quality[] Qualities { get; private set;}

        public QualityAttributes(Location location, string keyword, DataTable qualities)
        {
            Location = location;
            Keyword = keyword;
            var temp = new List<Quality>();
            foreach (var row in qualities.Rows)
            {
                var quality = new Quality(row.Cells.ElementAt(0).Value, row.Cells.ElementAt(1).Value);
                temp.Add(quality);               
            }
            Qualities = temp.ToArray();
        }
    }
    public class Quality 
    {
        public string Name { get; private set; }
        public string Description { get; private set; }


        public Quality(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
