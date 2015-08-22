using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gherkin.Ast
{
    public class FeatureDescription: IHasLocation
    {
        public Location Location { get; private set; }
        public Actor Actor { get; private set; }
        public Goal Goal { get; private set; }
        public Benefit Benefit { get; private set; }
        public QualityAttributes Qualities {get; private set;}


        public FeatureDescription(Location location, Actor actor, Goal goal, Benefit benefit,QualityAttributes qualities )
        {
            Location = location;
            Actor = actor;
            Goal = goal;
            Benefit = benefit;
            Qualities = qualities;
        }
    }
}
