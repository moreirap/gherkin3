using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gherkin.GRLCatalogueGenerator
{
    public class Quality
    {
        public int Id { get; set; }
        public string Name  { get; set; }

        public Quality(int id, string name)
        {
            Name = name;
            Id = id;
        }

        public Decomposition Decomposition  { get; set; }

        public IEnumerable<Contributions> Contributions { get; set; }

    }
}
