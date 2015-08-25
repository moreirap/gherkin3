using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gherkin.Ast
{
    public class FeatureDescription: IHasLocation
    {
        public IEnumerable<Tag> Tags { get; private set; }
        public Location Location { get; private set; }
        public Actor Actor { get; private set; }
        public Goal Goal { get; private set; }
        public Benefit Benefit { get; private set; }
        public IEnumerable<Goal> ImpactedGoals { get; private set; }
        public QualityAttributes QualityAttributes {get; private set;}

        public FeatureDescription(Tag[] tags, Location location, Actor actor, Goal goal, Benefit benefit, Goal[] impactedGoals, QualityAttributes qualityAttributes)
        {
            Tags = tags;
            Location = location;
            Actor = actor;
            Goal = goal;
            Benefit = benefit;
            ImpactedGoals = impactedGoals;
            QualityAttributes = qualityAttributes;
        }
    }
}
