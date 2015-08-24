using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gherkin.GRLCatalogueGenerator
{
    public class QualityCatalogue
    {     
        public IEnumerable<Quality> Qualities { get; set; }

        public static QualityCatalogue BuildDefault(out int lastAssignedId) 
        {
            var qualityCatalogue = new QualityCatalogue();
            qualityCatalogue.Qualities = new Quality[] { };
            lastAssignedId = 0;
            
            foreach (var qualityDef in DefaultQuality.DEFAULT_QUALITY_CATALOGUE_SPEC)
            {
                var id = qualityDef[0];
                var name = qualityDef[1];
                lastAssignedId = Convert.ToInt32(id);
                // Create Quality
                var quality = new Quality(Convert.ToInt32(id), name);
                qualityCatalogue.Qualities = qualityCatalogue.Qualities.Union(new Quality[] { quality });
                var index = 2;
                while (qualityDef.Length >= 5 && index<qualityDef.Length) 
                {
                    if (index == 2)
                    {
                        quality.Decomposition = new Decomposition(); // Create Decompositions
                        quality.Decomposition.Type = Decomposition.GetDecompositionTypeFromString(qualityDef[index++]);
                        quality.Decomposition.SubQualities = new Quality[] { };
                    }
                    var subQualityId = qualityDef[index++];
                    var subQualityName = qualityDef[index++];
                    // Create Quality
                    var subQuality = new Quality(Convert.ToInt32(subQualityId), subQualityName);
                    quality.Decomposition.SubQualities = quality.Decomposition.SubQualities.Union(new Quality[] { subQuality }).ToArray();
                    lastAssignedId = Convert.ToInt32(subQualityId);
                }
            }

            return qualityCatalogue;
        }
    }

    public static class DefaultQuality 
    {
        public static string[][] DEFAULT_QUALITY_CATALOGUE_SPEC = {
                                                        new [] {"1","Operability","And","2","By ensuring the product is easy to use","3","By guaranteeing that 90% of users will be able to reserve conference room within 5 minutes of product use"},
                                                        new [] {"4","Interoperability","And","5","By ensuring the product can work with most DBMS"},
                                                        new [] {"6","Success rate","And","7","By ensuring the product can communicate with DBMS on 100% of all transactions"},
                                                        new [] {"8","Consistency","And","9","By ensuring the database data corresponds to the data entered by the user","10","By making each conference room reservation in the system contain the same information as entered by the user"}
                                                    };              
    }
}
