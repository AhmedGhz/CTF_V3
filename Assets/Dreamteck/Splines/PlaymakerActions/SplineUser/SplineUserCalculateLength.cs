using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Calculates the length of the sampled spline of a SplineUser")]
    public class SplineUserCalculateLength : SplineUserAction
    {
        public FsmFloat from;
        public FsmFloat to;
        public FsmFloat length;

        public override void Reset()
        {
            base.Reset();
            length = null;
            from = new FsmFloat();
            from.Value = 0f;
            to = new FsmFloat();
            to.Value = 1f;
        }

        protected override void RunAction()
        {
            base.RunAction();
            length.Value = user.CalculateLength(from.Value, to.Value);
        }
    }
}