using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gherkin.GRLSpecGenerator
{
    public static class GRLSpecGenerator
    {
        public static URNspec GenerateGRLSpec(string featureFilePath)
        {
            var grlSpecText = new StringBuilder();
            var parser = new Parser();
            var parsingResult = parser.Parse(featureFilePath);

            if (parsingResult == null)
                throw new InvalidOperationException("parser returned null");

            var urnSpec = InitialiseUrnSpec(parsingResult.Name);
            var container = new GRLContainer();       

            TransformFeature(parsingResult, urnSpec.grlspec,container);
            
            // Add jUCMNav tool required elements
            CompleteGRLSpec(urnSpec, container);
            return urnSpec;

        }

        private static URNspec InitialiseUrnSpec(string name)
        {
            var urnSpec = new URNspec();
            urnSpec.name = name;
            urnSpec.grlspec = new GRLspec();
            if (urnSpec.grlspec.intElements == null)
                urnSpec.grlspec.intElements = new IntentionalElement[] { };
            if (urnSpec.grlspec.actors == null)
                urnSpec.grlspec.actors = new Actor[] { };
            return urnSpec;
        }

        private static void CompleteGRLSpec(URNspec urnSpec, GRLContainer container)
        {
            // Set empty ucmSpec
            urnSpec.ucmspec = new UCMspec();

            // Set empty graph placeholder
            var emptyGraph = new GRLGraph();
            emptyGraph.id = (container.LastAddedElementId()+1).ToString();
            emptyGraph.name = "GRL Graph";
            urnSpec.grlspec.grlGraphs = new GRLGraph[] { emptyGraph };
            
            // Set info node
            urnSpec.info = new ConcreteURNspec();
            urnSpec.info.description = "GRL Spec generated from BDD feature file(s)";
            urnSpec.info.author = "Pedro";
            urnSpec.info.created = "August 22, 2015 3:16:50 PM BST";
            urnSpec.info.modified = "August 22, 2015 3:16:50 PM BST";
            urnSpec.info.specVersion = "3";
            urnSpec.info.urnVersion = "1.27";

        }

        private static void TransformFeature(Ast.Feature parsingResult, GRLspec  grlSpec,GRLContainer container) 
        {
            //<intElements>
            //    <id>11</id>
            //    <name>Record meeting entries</name>
            //    <linksDest>34</linksDest>
            //    <linksDest>59</linksDest>
            //    <type>Goal</type>
            //    <decompositionType>AND</decompositionType>
            //    <importance>High</importance>
            //    <importanceQuantitative>100</importanceQuantitative>
            //    <refs>12</refs>
            //    <style>
            //        <filled>false</filled>
            //    </style>
            //    <actor>23</actor>
            //</intElements>
            
            var importance = parsingResult.Description.Tags == null && parsingResult.Description.Tags.ElementAt(0) != null && parsingResult.Description.Tags.ElementAt(0).Name.StartsWith("@") ? "None" : parsingResult.Description.Tags.ElementAt(0).Name.Substring(1);
            var actor      = parsingResult.Description.Actor;
            var goal       = parsingResult.Description.Goal;         

            // Create or obtain actor element first
            var actorExists = false;
            var actorElement = RegisterIntentionalElement<Actor>(container, actor.Name, out actorExists);

            // create intentional element corresponding to goal (This should not exist)
            var goalExists = false;
            var goalElement = RegisterIntentionalElement<IntentionalElement>(container, goal.Description, out goalExists);
            if (goalExists)
                throw new ApplicationException("Goal already processed:" + goal.Description);

            goalElement.type = IntentionalElementType.Goal;
            goalElement.decompositionType = DecompositionType.AND;
            goalElement.importance = (ImportanceType)Enum.Parse(typeof(ImportanceType), importance);
            goalElement.importanceQuantitative = GetImportanceAsQuantity(goalElement.importance);
            SetIntentionalElementStyle(goalElement);

            // Set to which actor does this goal belong to
            goalElement.actor = actorElement.id;

            // Add Qualities (if any)
            AddQualitiesToGRLSpec(parsingResult, grlSpec, container, actorElement);

            // Update actor element with goal and qualities
            actorElement.elems = actorElement.elems ?? new String[]{};
            actorElement.elems = actorElement.elems.Union(new[] { goalElement.id }).ToArray();
            if (parsingResult.Description != null && parsingResult.Description.Qualities != null)
            {
                var qualities = parsingResult.Description.Qualities.Qualities;
                foreach (var q in qualities)
                {
                    var qualityNameElement = container.GetElementByName<IntentionalElement>(q.Name);
                    var qualityDescElement = container.GetElementByName<IntentionalElement>(q.Description);
                    SetIntentionalElementStyle(qualityNameElement);
                    SetIntentionalElementStyle(qualityDescElement);
                    actorElement.elems = actorElement.elems.Union(new[] { qualityNameElement.id, qualityDescElement.id }).ToArray();
                }
            }
            // Set Actor importance
            actorElement.importance = ImportanceType.Medium;
            actorElement.importanceQuantitative = GetImportanceAsQuantity(actorElement.importance);

            // Add or update actor element
            if (actorExists)
                grlSpec.actors.Update(ac => ac = (ac.id == actorElement.id) ? actorElement : ac);
            else
            {
                SetActorStyle(actorElement);
                grlSpec.actors = grlSpec.actors.Union(new[] { actorElement }).ToArray();
            }

            // Finally add goal element to GRL spec
            grlSpec.intElements = grlSpec.intElements.Union(new[] { goalElement }).ToArray();
        }

        private static void SetActorStyle(Actor actorElement)
        {
            actorElement.style = new ConcreteStyle();
            actorElement.style.lineColor = "0,0,0";
            actorElement.style.fillColor = "255,255,255";
            actorElement.style.filled = false;
        }

        private static void SetIntentionalElementStyle(IntentionalElement goalElement)
        {
            goalElement.style = new ConcreteStyle();
            goalElement.style.filled = false;
        }

        

        private static void AddQualitiesToGRLSpec(Ast.Feature parsingResult, GRLspec grlSpec, GRLContainer container, GRLLinkableElement actorElement)
        {
            //<intElements>
            //    <id>17</id>
            //    <name>Interoperability</name>
            //    <desc>
            //        <description></description>
            //    </desc>
            //    <linksDest>69</linksDest>
            //    <type>Softgoal</type>
            //    <decompositionType>AND</decompositionType>
            //    <importance>High</importance>
            //    <importanceQuantitative>100</importanceQuantitative>
            //    <refs>18</refs>
            //    <style>
            //        <filled>false</filled>
            //    </style>
            //    <actor>23</actor>
            //</intElements>
            if (parsingResult.Description != null && parsingResult.Description.Qualities != null) 
            {
                var qualities = parsingResult.Description.Qualities.Qualities;
                foreach (var q in qualities)
                {
                    AddQualityAttributeToGRLSpec(grlSpec, container, actorElement, q.Name,q.Contribution);
                    AddQualityAttributeToGRLSpec(grlSpec, container, actorElement, q.Description,q.Contribution);
                }
            }
        }

        private static void AddQualityAttributeToGRLSpec(GRLspec grlSpec, GRLContainer container, GRLLinkableElement actorElement, string qualityName,string contribution)
        {
            var qualityExists = false;
            var qualityElement = RegisterIntentionalElement<IntentionalElement>(container, qualityName,out qualityExists);
            if (!qualityExists)
            {
                qualityElement.name = qualityName;
                qualityElement.type = IntentionalElementType.Softgoal;
                qualityElement.decompositionType = DecompositionType.AND;
                qualityElement.importance = ImportanceType.Medium;
                qualityElement.importanceQuantitative = GetImportanceAsQuantity(qualityElement.importance);
                // Set to which actor does this qualitygoal belong to
                qualityElement.actor = actorElement.id;
                grlSpec.intElements = grlSpec.intElements.Concat(new[] { qualityElement }).ToArray();
            }
        }

        private static string GetImportanceAsQuantity(ImportanceType importanceType)
        {
            switch (importanceType)
            {
                case ImportanceType.High:
                    return "100";
                case ImportanceType.Medium:
                    return "50";
                case ImportanceType.Low:
                    return "25";
                default:
                    return "0";
            }
        }

        private static string GetContributionAsQuantity(ContributionType contributionType)
        {
            switch (contributionType)
            {
                case ContributionType.Break:
                    return "";
                case ContributionType.Help:
                    return "";
                case ContributionType.Hurt:
                    return "";
                case ContributionType.Make:
                    return "";
                case ContributionType.SomeNegative:
                    return "";
                case ContributionType.SomePositive:
                    return "";
                case ContributionType.Unknown:
                    return "";
                default:
                    return "0";
            }
        }


        private static T RegisterIntentionalElement<T>(GRLContainer container, string name, out bool exists) where T : GRLLinkableElement,new()
        {
            T intElement = new T();
            exists = false;
            var existingElement = container.GetElementByName<T>(name);
            if (existingElement != null) {
                exists = true;   
                return existingElement;
            }

            var id = container.Add<GRLLinkableElement>(intElement);
            intElement.id = id.ToString();
            intElement.name = name;
            return intElement;
        }
    }

    public class GRLContainer 
    {
        private Dictionary<int, GRLLinkableElement> registry;
        private int id = 0;

        public GRLContainer()
        {
            registry = new Dictionary<int, GRLLinkableElement>();
        }

        public int Add<T>(T value) where T : GRLLinkableElement
        {         
            if (value == null)
                throw new ApplicationException("Cannot register null object in GRLContainer");
            if (registry.Any(keyval => keyval.Value.name == value.name))
                throw new ApplicationException("Cannot register value in GRLContainer as it already exists");
            registry.Add(++id, value);        
            return id;
        }

        public int AddOrUpdate<T>(T value) where T : GRLLinkableElement
        {
            if (value == null)
                throw new ApplicationException("Cannot register null object in GRLContainer");
            if (registry.Any(keyval => keyval.Value.name == value.name))
                registry.Remove(Convert.ToInt32(value.id));
            return Add<T>(value);
        }


        public T GetValue<T>(int key) where T : GRLLinkableElement
        {
            return registry[key] as T;
        }
        
        public T GetElementByName<T>(string name) where T : GRLLinkableElement
        {
            if (registry.Any(keyval => keyval.Value.name == name))
                return registry.First(keyval => keyval.Value.name == name).Value as T;
            return null;
        }

        public int LastAddedElementId() 
        {
            return id;
        }
    }
}
