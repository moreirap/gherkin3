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
        private static int freeId = 0;
        public static string NextId { get { return Convert.ToString(++freeId); } }
        public static string[][] DEFAULT_QUALITY_CATALOGUE_SPEC = {
                                                        new [] {NextId,"Operability","And",NextId,"By ensuring the product is easy to use",NextId,"By guaranteeing that 90% of users will be able to reserve conference room within 5 minutes of product use"},
                                                        new [] {NextId,"Interoperability","And",NextId,"By ensuring the product can work with most DBMS",NextId,"By ensuring the product can interface with various email servers and send email"},
                                                        new [] {NextId,"Success rate","And",NextId,"By ensuring the product can communicate with DBMS on 100% of all transactions"},
                                                        new [] {NextId,"Consistency","And",NextId,"By ensuring the database data corresponds to the data entered by the user",NextId,"By making each conference room reservation in the system contain the same information as entered by the user"},
                                                        new [] {NextId,"Understandability","And",NextId,"By allowing intuitive searching of available conference rooms"},
                                                        new [] {NextId,"Maintainability","And",NextId,"By designing the product using Design Patterns and coding best practices"},
                                                        new [] {NextId,"Modifiability","And",NextId,"By ensuring 90% of maintenance software developers are able to integrate new functionality into the product with 2 working days"},
                                                        new [] {NextId,"Configurability","And",NextId,"By allowing for customization of start page and views preferences"}
                                                        //new [] {"","","And","",""}
                                                    };
        //public static string[][] DEFAULT_QUALITY_CATALOGUE_SPEC = {
        //                                                new [] {NextId,"Functional suitability","And", NextId,"Functional completeness", NextId,"Functional correctness", NextId,"Functional appropriateness"},
        //                                                new [] {NextId,"Performance efficiency","And", NextId,"Time behaviour", NextId,"Resource utilization", NextId,"Capacity"},
        //                                                new [] {NextId,"Compatibility","And", NextId,"Co-existence", NextId,"Interoperability"},
        //                                                new [] {NextId,"Usability","And", NextId,"Appropriateness recognizability", NextId,"Learnability", NextId,"Operability", NextId,"User error protection", NextId,"user interface aesthetics", NextId,"Accessibility"},
        //                                                new [] {NextId,"Reliability","And", NextId,"Maturity", NextId,"Availability", NextId,"Fault tolerance", NextId,"Recoverability"},
        //                                                new [] {NextId,"Security","And", NextId,"Confidentiality", NextId,"Integrity", NextId,"Non-repudiation", NextId,"Accountability", NextId,"Authenticity"},
        //                                                new [] {NextId,"Maintainability","And", NextId,"Modularity", NextId,"Reusability", NextId,"Analysability", NextId,"Modifiability", NextId,"Testability"},
        //                                                new [] {NextId,"Portability","And", NextId,"Adaptability", NextId,"Installability", NextId,"Replaceability"}
        //                                                //new [] {"","","And","",""}
        //                                            };
    }
}

