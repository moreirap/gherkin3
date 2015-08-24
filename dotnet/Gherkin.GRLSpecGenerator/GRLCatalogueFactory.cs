using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gherkin.GRLCatalogueGenerator
{
    public static class GRLCatalogueFactory      
    {
        public static grlcatalog NewGRLCatalog(string name, string description, string author)
        {
            var grlCatalogue = new grlcatalog();
            grlCatalogue.author = author;
            grlCatalogue.catalogname = name;
            grlCatalogue.description = description;
            grlCatalogue.elementdef = new grlcatalogIntentionalelement[] { };
            grlCatalogue.linkdef = new grlcatalogLinkdef();
            grlCatalogue.linkdef.contribution = new grlcatalogLinkdefContribution[] { };
            grlCatalogue.linkdef.decomposition = new grlcatalogLinkdefDecomposition[] { };
            grlCatalogue.actordef = new grlcatalogActor[] { };
            grlCatalogue.actorIElinkdef = new grlcatalogActorContIE[] { };
            return grlCatalogue;
        }

        public static grlcatalogActorContIE NewActorContribution(grlcatalogActor actorElement, grlcatalogIntentionalelement goalElement)
        {
            var actorContIE = new grlcatalogActorContIE();
            actorContIE.actor = actorElement.id;
            actorContIE.ie = goalElement.id;
            return actorContIE;
        }  
    }
}
