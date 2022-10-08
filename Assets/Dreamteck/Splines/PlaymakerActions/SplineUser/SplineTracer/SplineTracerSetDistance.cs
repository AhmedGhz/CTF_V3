using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Sets the percent of a SplineTracer")]
    public class SplineTracerSetDistance : SplineTracerAction
    {
        public FsmFloat distance;
        public FsmBool checkTriggers;

        public override void Reset()
        {
            base.Reset();
            distance = new FsmFloat();
            checkTriggers = new FsmBool();
        }

        protected override void RunAction()
        {
            base.RunAction();
            tracer.SetDistance(distance.Value, checkTriggers.Value);
        }
    }
}
