using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gherkin.GRLCatalogueGenerator
{
    public class GRLCatalogueGenerator
    {
        GRLElementsContainer container;

        public GRLCatalogueGenerator()
        {
            // Initialise a new container to keep track of GRL Elements
            container = new GRLElementsContainer();
        }

        public void UpdateGRLCatalogue(grlcatalog grlCatalogue, Ast.Feature parsingResult)
        {
            // Add new actor element or retrieve existing one from container
            var actorElement = AddActorToGRLCatalogue(parsingResult, grlCatalogue);

            // Add new goal (This is the "I want to..." goal of a BDD Feature)
            var goalElement = AddFeatureGoalToGRLCatalogue(parsingResult, grlCatalogue);

            // Attach goal to Actor
            BindElementToActorAndUpdateGRLCatalogue(grlCatalogue, actorElement, goalElement);

            // Are there any non-functionals?
            if (parsingResult.Description != null && parsingResult.Description.Qualities != null && parsingResult.Description.Qualities.Qualities.Length > 0)
            {
                // Add Qualities
                AddQualitiesToGRLCatalogue(parsingResult, grlCatalogue, actorElement);

                // Add Contribution Links from qualities to goal
                AddQualityGoalContributionsToGRLCatalogue(parsingResult, grlCatalogue, goalElement);

                // Add Decomposition links from quality descriptions to quality name
                //AddQualityDecompositionsToGRLCatalogue(parsingResult, grlCatalogue, goalElement);

            }
        }

        private grlcatalogActor AddActorToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue)
        {
            var actor = parsingResult.Description.Actor;

            // Create or obtain actor element from container
            var actorExists = false;
            var actorElement = container.RegisterElement<grlcatalogActor>(actor.Name, out actorExists);

            // Only add to catalogue if not already present
            if (!actorExists)
                grlCatalogue.actordef = grlCatalogue.actordef.Union(new[] { actorElement }).ToArray();

            return actorElement;
        }
        
        private grlcatalogIntentionalelement AddFeatureGoalToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue)
        {
            var goal = parsingResult.Description.Goal;

            // Create intentional element corresponding to goal (This should not exist in catalogue already)
            var goalExists = false;
            var goalElement = container.RegisterElement<grlcatalogIntentionalelement>(goal.Description, out goalExists);
            if (goalExists)
                throw new ApplicationException("Goal already processed:" + goal.Description);

            goalElement.type = "Goal";
            goalElement.decompositiontype = "And";

            // Add goal element to catalogue
            grlCatalogue.elementdef = grlCatalogue.elementdef.Union(new[] { goalElement }).ToArray();
            return goalElement;
        }

        private void AddQualitiesToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue,  grlcatalogActor actorElement)
        {
            var qualities = parsingResult.Description.Qualities.Qualities;
            foreach (var q in qualities)
            {
                AddQualityAttributeToGRLCatalogue(grlCatalogue, actorElement, q.Name);
                //AddQualityAttributeToGRLCatalogue(grlCatalogue, actorElement, q.Description);
            }
        }

        private void AddQualityAttributeToGRLCatalogue(grlcatalog grlCatalogue,  grlcatalogActor actorElement, string qualityName)
        {
            var qualityExists = false;
            var qualityElement = container.RegisterElement<grlcatalogIntentionalelement>(qualityName, out qualityExists);
            if (!qualityExists)
            {
                qualityElement.type = "Softgoal";
                qualityElement.decompositiontype = "And";
                // Add intentional element to GRL catalogue
                grlCatalogue.elementdef = grlCatalogue.elementdef.Union(new[] { qualityElement }).ToArray();

                // Attach quality to Actor
                //BindElementToActorAndUpdateGRLCatalogue(grlCatalogue, actorElement, qualityElement);
            }
        }

        private void BindElementToActorAndUpdateGRLCatalogue(grlcatalog grlCatalogue, grlcatalogActor actorElement, grlcatalogIntentionalelement boundedElement)
        {
            var bindindName = String.Format("Bind {0} to Actor {1}",boundedElement.id,actorElement.id);
            var bindingExists = false;
            var actorContIE = container.RegisterElement<grlcatalogActorContIE>(bindindName, out bindingExists);
            actorContIE.actor = actorElement.id;
            actorContIE.ie = boundedElement.id;
            

            grlCatalogue.actorIElinkdef = grlCatalogue.actorIElinkdef.Union(new[] { actorContIE }).ToArray();
        }

        

        private void AddQualityGoalContributionsToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue,  grlcatalogIntentionalelement goalElement)
        {
            var qualities = parsingResult.Description.Qualities.Qualities;
            foreach (var q in qualities)
            {
                var qualityNameElement = container.GetElementByName<grlcatalogIntentionalelement>(q.Name);
                var qualityDescriptionElement = container.GetElementByName<grlcatalogIntentionalelement>(q.Description);
                    
                var contributionElement = BuildContributionElement(q.Contribution, qualityNameElement,goalElement);
                grlCatalogue.linkdef.contribution = grlCatalogue.linkdef.contribution.Union(new[] { contributionElement }).ToArray();

            }    
        }

        private void AddQualityDecompositionsToGRLCatalogue(Ast.Feature parsingResult, grlcatalog grlCatalogue,  grlcatalogIntentionalelement goalElement)
        {
            var qualities = parsingResult.Description.Qualities.Qualities;
            foreach (var q in qualities)
            {
                var qualityNameElement = container.GetElementByName<grlcatalogIntentionalelement>(q.Name);
                var qualityDescriptionElement = container.GetElementByName<grlcatalogIntentionalelement>(q.Description);

                var decompositionElement = BuildDecompositionElement(qualityDescriptionElement, qualityNameElement);
                grlCatalogue.linkdef.decomposition = grlCatalogue.linkdef.decomposition.Union(new[] { decompositionElement }).ToArray();

            }    
            
        }

        private grlcatalogLinkdefDecomposition BuildDecompositionElement(grlcatalogIntentionalelement sourceElement, grlcatalogIntentionalelement targetElement)
        {
            // Set decomposition between quality name and description
            var decomposition_name = String.Format("QualityDecomposition_{0}_To_{1}", sourceElement.id, targetElement.id);
            var decompositionExists = false;
            var decompositionElement = container.RegisterElement<grlcatalogLinkdefDecomposition>(decomposition_name, out decompositionExists);
            if (!decompositionExists)
            {
                decompositionElement.srcid = sourceElement.id;
                decompositionElement.destid = targetElement.id;      
            }
            return decompositionElement;
        }

        private grlcatalogLinkdefContribution BuildContributionElement(string contribution, grlcatalogIntentionalelement sourceElement, grlcatalogIntentionalelement targetElement)
        {
            // Set contribution between quality and goal
            var contribution_name = String.Format("Contribution_From_Quality_{0}_To_Goal_{1}", sourceElement.id, targetElement.id);
            var contributionExists = false;
            var contributionElement = container.RegisterElement<grlcatalogLinkdefContribution>(contribution_name, out contributionExists);
            if (!contributionExists)
            {
                contributionElement.srcid = sourceElement.id;
                contributionElement.destid = targetElement.id;
                contributionElement.contributiontype = contribution;
                contributionElement.quantitativeContribution = grlcatalog.GetContributionAsQuantity(contribution);
                contributionElement.correlation = false;
            }
            return contributionElement;
        }


        internal void AppendQualityCatalogue(QualityCatalogue defaultQualityCatalogue, grlcatalog grlCatalogue)
        {
            var qualities = defaultQualityCatalogue.Qualities;
            foreach (var q in qualities)
            {
                var qualityExists = false;
                var qualityElement = container.RegisterElement<grlcatalogIntentionalelement>(q.Name, out qualityExists);
                if (qualityExists)
                     throw new ApplicationException("When building default catalogues, all elements should be unique: " + q.Name + " exists!");

                // Add intentional element to GRL catalogue
                qualityElement.type = "Softgoal";
                grlCatalogue.elementdef = grlCatalogue.elementdef.Union(new[] { qualityElement }).ToArray();

                if (q.Decomposition != null)
                {
                    qualityElement.decompositiontype = q.Decomposition.Type.ToString();
                    foreach (var subQuality in q.Decomposition.SubQualities)
                    {
                        var subQualityExists = false;
                        var subqualityElement = container.RegisterElement<grlcatalogIntentionalelement>(subQuality.Name, out subQualityExists);
                        
                        if (subQualityExists)
                            throw new ApplicationException("When building default catalogues, all elements should be unique: " + subQuality.Name + " exists!");
                        
                        // Add subquality to catalogue
                        subqualityElement.type = "Softgoal";
                        grlCatalogue.elementdef = grlCatalogue.elementdef.Union(new[] { subqualityElement }).ToArray();

                        // Add Decomposition to GRL Catalogue
                        var decompositionElement = BuildDecompositionElement(subqualityElement, qualityElement);
                        grlCatalogue.linkdef.decomposition = grlCatalogue.linkdef.decomposition.Union(new[] { decompositionElement }).ToArray();
                    }
                }        
            }   
        }
    }
}
