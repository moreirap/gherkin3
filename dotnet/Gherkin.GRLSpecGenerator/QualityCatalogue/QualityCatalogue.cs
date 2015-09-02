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

        private static class DefaultQuality
        {
            private static int freeId = 0;
            public static string NextId { get { return Convert.ToString(++freeId); } }
            public static string[][] DEFAULT_QUALITY_CATALOGUE_SPEC = {
            // Product Quality Charactristics and sub-characteristics
            new [] {NextId,"Functional suitability","And", NextId,"Functional completeness", NextId,"Functional correctness", NextId,"Functional appropriateness"},
            new [] {NextId,"Performance efficiency","And", NextId,"Time behaviour", NextId,"Resource utilization", NextId,"Capacity"},
            new [] {NextId,"Compatibility","And", NextId,"Co-existence", NextId,"Interoperability"},
            new [] {NextId,"Usability","And", NextId,"Appropriateness recognizability", NextId,"Learnability", NextId,"Operability", NextId,"User error protection", NextId,"user interface aesthetics", NextId,"Accessibility"},
            new [] {NextId,"Reliability","And", NextId,"Maturity", NextId,"Availability", NextId,"Fault tolerance", NextId,"Recoverability"},
            new [] {NextId,"Security","And", NextId,"Confidentiality", NextId,"Integrity", NextId,"Non-repudiation", NextId,"Accountability", NextId,"Authenticity"},
            new [] {NextId,"Maintainability","And", NextId,"Modularity", NextId,"Reusability", NextId,"Analysability", NextId,"Modifiability", NextId,"Testability"},
            new [] {NextId,"Portability","And", NextId,"Adaptability", NextId,"Installability", NextId,"Replaceability"},
            // Quality in Use characteristics and sub-characteristics
            new [] {NextId,"Effectiveness"},
            new [] {NextId,"Efficiency"},
            new [] {NextId,"Satisfaction", "And", NextId,"Usefulness", NextId,"Trust", NextId,"Pleasure", NextId,"Comfort"},
            new [] {NextId,"Freedom from risk", "And", NextId,"Economic risk mitigation", NextId,"Health and safety risk mitigation", NextId,"Environmental risk mitigation"},
            new [] {NextId,"Context coverage", "And", NextId,"Context completeness", NextId,"Flexibility"}
        };
        }
    }
}

