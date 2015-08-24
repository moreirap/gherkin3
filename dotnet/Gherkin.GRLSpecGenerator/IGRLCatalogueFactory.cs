using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gherkin.GRLCatalogueGenerator
{
    interface IGRLCatalogueFactory
    {
        grlcatalog InitialiseGrlCatalogue(string name, string description, string author);
    }
}
