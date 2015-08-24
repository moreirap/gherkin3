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
                var quality = new Quality();
                SetQualityValues(quality, row.Cells);
                temp.Add(quality);               
            }
            Qualities = temp.ToArray();
        }

        private void SetQualityValues(Quality quality, IEnumerable<TableCell> cells) 
        {
            // Set Name
            TrySetQualityValue(quality, cells, 0);
            // Set Contribution
            TrySetQualityValue(quality, cells, 1);
        }
        private bool TrySetQualityValue(Quality quality, IEnumerable<TableCell> cells, int index)
        {
            var success = false;
            try
            {
                switch (index) 
                {
                    case 0:
                        quality.Name = cells.ElementAt(index).Value;
                        break;
                    case 1:
                        quality.Contribution = cells.ElementAt(index).Value;
                        break;
                    default:
                        throw new ApplicationException("Unexpected number of cells in quality");
                }
                success = true;
            }
            catch (Exception) {}
            return success;
        }
    }
    public class Quality 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Contribution { get; set; }

        public Quality()
        {
            
        }
    }
}
