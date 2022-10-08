using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline Computer")]
    [Tooltip("Calculates the length of the spline")]
    public class SplineComputerCalculateLength : SplineComputerAction
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
            length.Value = spline.CalculateLength(from.Value, to.Value);
        }
    }
}