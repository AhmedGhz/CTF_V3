using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Calculates the percent from the spline at a given distance from the start point")]
    public class SplineUserTravel : SplineUserAction
    {
        public FsmFloat start;
        public FsmFloat distance;
        [ObjectType(typeof(Spline.Direction))]
        public FsmEnum direction;
        public FsmFloat result;

        public override void Reset()
        {
            base.Reset();
            start = new FsmFloat();
            distance = new FsmFloat();
            result = new FsmFloat();
            direction = new FsmEnum();
        }

        protected override void RunAction()
        {
            base.RunAction();
            result.Value = (float)user.Travel(start.Value, distance.Value, (Spline.Direction)direction.RawValue);
        }
    }
}