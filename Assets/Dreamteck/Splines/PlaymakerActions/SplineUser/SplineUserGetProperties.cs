using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Gets the properties of a SplineUser component")]
    public class SplineUserGetProperties : SplineUserAction
    {
        public FsmFloat clipFrom;
        public FsmFloat clipTo;

        public override void Reset()
        {
            base.Reset();
            clipFrom = null;
            clipTo = null;
        }

        protected override void RunAction()
        {
            base.RunAction();
            clipFrom.Value = (float)user.clipFrom;
            clipTo.Value = (float)user.clipTo;
        }
    }
}