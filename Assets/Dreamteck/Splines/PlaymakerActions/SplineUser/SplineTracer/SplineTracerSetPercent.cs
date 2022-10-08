using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Sets the percent of a SplineTracer")]
    public class SplineTracerSetPercent : SplineUserAction
    {
        public FsmFloat percent;
        public FsmBool checkTriggers;


        public override void Reset()
        {
            base.Reset();
            percent = new FsmFloat();
            checkTriggers = new FsmBool();
        }

        protected override void RunAction()
        {
            base.RunAction();
            if (!(user is SplineTracer)) return;
            SplineTracer tracer = (SplineTracer)user;
            tracer.SetPercent(percent.Value, checkTriggers.Value);
        }
    }
}
