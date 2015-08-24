using System;
using System.Linq;

namespace Gherkin
{
    public class GherkinDialect
    {
        public string Language { get; private set; }

        public string[] FeatureKeywords { get; private set; }
        public string[] BackgroundKeywords { get; private set; }
        public string[] ScenarioKeywords { get; private set; }
        public string[] ScenarioOutlineKeywords { get; private set; }
        public string[] ExamplesKeywords { get; private set; }
        public string[] GivenStepKeywords { get; private set; }
        public string[] WhenStepKeywords { get; private set; }
        public string[] ThenStepKeywords { get; private set; }
        public string[] AndStepKeywords { get; private set; }
        public string[] ButStepKeywords { get; private set; }
        public string[] StepKeywords { get; private set; }

        public string[] AsAKeywords{ get; private set; }
        public string[] IWantKeywords{ get; private set; }
        public string[] SoThatKeywords{ get; private set; }
        public string[] QualityAttributeKeywords{ get; private set; }
        public string[] QualityReasonKeywords { get; private set; }

        public GherkinDialect(
            string language,
            string[] featureKeywords, 
            string[] backgroundKeywords, 
            string[] scenarioKeywords,
            string[] scenarioOutlineKeywords,
            string[] examplesKeywords,
            string[] givenStepKeywords,
            string[] whenStepKeywords,
            string[] thenStepKeywords,
            string[] andStepKeywords,
            string[] butStepKeywords,
            string[] asAKeywords,
            string[] iWantKeywords,
            string[] soThatKeywords,
            string[] qualityAttributeKeywords,
            string[] qualityReasonKeywords)
        {
            Language = language;
            FeatureKeywords = featureKeywords;
            BackgroundKeywords = backgroundKeywords;
            ScenarioKeywords = scenarioKeywords;
            ScenarioOutlineKeywords = scenarioOutlineKeywords;
            ExamplesKeywords = examplesKeywords;
            GivenStepKeywords = givenStepKeywords;
            WhenStepKeywords = whenStepKeywords;
            ThenStepKeywords = thenStepKeywords;
            AndStepKeywords = andStepKeywords;
            ButStepKeywords = butStepKeywords;

            StepKeywords = givenStepKeywords
                .Concat(whenStepKeywords)
                .Concat(thenStepKeywords)
                .Concat(andStepKeywords)
                .Concat(butStepKeywords)
                .Distinct()
                .ToArray();

            AsAKeywords   = asAKeywords
                .Concat(andStepKeywords)
                .Distinct()
                .ToArray();
            IWantKeywords = iWantKeywords
                .Concat(andStepKeywords)
                .Distinct()
                .ToArray();
            SoThatKeywords = soThatKeywords
                .Concat(andStepKeywords)
                .Distinct()
                .ToArray();
            QualityAttributeKeywords = qualityAttributeKeywords;
            QualityReasonKeywords = qualityReasonKeywords;
        }
    }
}
