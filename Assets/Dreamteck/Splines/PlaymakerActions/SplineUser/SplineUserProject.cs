using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Projects a point in space on the sampled spline and returns the evaluation percent")]
    public class SplineUserProject : SplineUserAction
    {
        public FsmVector3 point;
        public FsmFloat from;
        public FsmFloat to;
        public FsmFloat percent;
        SplineSample evalResult = new SplineSample();

        public override void Reset()
        {
            base.Reset();
            point = null;
            percent = null;
            from = new FsmFloat();
            from.Value = 0f;
            to = new FsmFloat();
            to.Value = 1f;
        }

        protected override void RunAction()
        {
            base.RunAction();
            user.Project(point.Value, evalResult, from.Value, to.Value);
            percent.Value = (float)evalResult.percent;
        }
    }
}