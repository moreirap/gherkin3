using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gherkin.GRLCatalogueGenerator
{
    public class Decomposition
    {
        public DecompositionType Type { get; set; }
        public Quality[] SubQualities { get; set; }

        public static DecompositionType GetDecompositionTypeFromString(string decompositionType)
        {
            return (DecompositionType)Enum.Parse(typeof(DecompositionType), decompositionType,true);
        }
    }

    public enum DecompositionType
    {
        AND,OR,XOR
    }
}