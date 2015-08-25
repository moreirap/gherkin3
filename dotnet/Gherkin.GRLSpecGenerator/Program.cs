using System;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Gherkin.GRLCatalogueGenerator
{
    class Program
    {
        const string DEFAULT_CATALOGUE_NAME     = "GRL Catalogue";
        const string DEFAULT_CATALOGUE_DESC     = "Generated from one or more feature files using Gherkin.GRLCatalogueGenerator";
        const string DEFAULT_CATALOGUE_AUTHOR   = "Pedro Moreira";
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: Gherkin.GRLCatalogueGenerator.exe file1.feature [[+file2.feature][+file3.feature]...]");
                return 100;
            }

            var startTime = Environment.TickCount;
            try
            {
                // Use to generate JSon of default quality catalogue
                //LoadDefaultQualityCatalogue();
                //return 0;

                // Load Default Quality Catalogue
                var lastAssignedId = 0;
                var defaultQualityCatalogue = QualityCatalogue.BuildDefault(out lastAssignedId);

                // Initialise a grlCatalogue
                var grlCatalogue = GRLCatalogueFactory.NewGRLCatalog(DEFAULT_CATALOGUE_NAME, DEFAULT_CATALOGUE_DESC, DEFAULT_CATALOGUE_AUTHOR);

                var generator = new GRLCatalogueGenerator();
                
                // Avoid parsing features twice by collecting parsing results
                var parsingResults = new Ast.Feature[] { };

                // Prepend qualities from default catalogue
                generator.AppendQualityCatalogue(defaultQualityCatalogue,grlCatalogue);

                // Process all feature files passed as arguments
                foreach (var featureFiles in args)
                {
                    parsingResults = parsingResults.Concat(new [] { UpdateGRLCatalogueWithFeature(featureFiles, grlCatalogue, generator) } ).ToArray();
                }

                // Add any dependencies amongst goals. Can only run when all features have been processed
                foreach (var parsingResult in parsingResults)
                {
                    generator.UpdateGRLCatalogueImpactedGoals(grlCatalogue, parsingResult);
                }

                // Serialise resulting GRL Catalogue
                var grlCatalogueAsXmlString = XMLSerializerHelper.SerializeObject(grlCatalogue);
                Console.WriteLine(XMLSerializerHelper.RemoveBOM(grlCatalogueAsXmlString));
                //var outputStream = Console.OpenStandardOutput();
                //using (var textWriter = new StreamWriter(@"D:\Dev\thesis\gherkin3\testdata\extendedBDD\All_transformed.grl",false,new UTF8Encoding(false)))
                //    textWriter.WriteLine(grlCatalogueAsXmlString);
            }
            catch (Exception ex)
            {
                // Ideally we'd use Console.Error here, but we can't because
                // 2> doesn't seem to work properly - at least not on Mono on OS X.
                Console.WriteLine(ex.Message);
                return 1;
            }
            var endTime = Environment.TickCount;
            if (Environment.GetEnvironmentVariable("GHERKIN_PERF") != null)
            {
                Console.Error.WriteLine(endTime - startTime);
            }
            return 0;
        }

        private static void LoadDefaultQualityCatalogue()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Formatting = Formatting.Indented;
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            var lastAssignedId = 0;
            var qualityCatalogue = QualityCatalogue.BuildDefault(out lastAssignedId);

            var defaultQaulityCatalogueAsJSon = JsonConvert.SerializeObject(qualityCatalogue, jsonSerializerSettings);
            Console.WriteLine(LineEndingHelper.NormalizeJSonLineEndings(defaultQaulityCatalogueAsJSon));
            Console.WriteLine("Last Assigned ID " + lastAssignedId);
        }

        private static Ast.Feature UpdateGRLCatalogueWithFeature(string featureFilePath, grlcatalog grlCatalogue, GRLCatalogueGenerator generator)
        {
            // Parse feature file and build AST
            var parser = new Parser();
            var parsingResult = parser.Parse(featureFilePath);
            if (parsingResult == null)
                throw new InvalidOperationException("parser returned null");

            generator.UpdateGRLCatalogue(grlCatalogue, parsingResult);
            return parsingResult;
        }
    }
}
