using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Gets the result of a spline tracer")]
    public class SplineTracerGetResult : SplineTracerAction
    {
        public FsmFloat percent;
        public FsmVector3 position;
        public FsmVector3 direction;
        public FsmVector3 normal;
        public FsmFloat size;
        public FsmColor color;

        public override void Reset()
        {
            base.Reset();
            percent = new FsmFloat();
            position = new FsmVector3();
            direction = new FsmVector3();
            normal = new FsmVector3();
            size = new FsmFloat();
            color = new FsmColor();
        }

        protected override void RunAction()
        {
            base.RunAction();
            percent.Value = (float)tracer.result.percent;
            position.Value = tracer.result.position;
            direction.Value = tracer.result.forward;
            normal.Value = tracer.result.up;
            size.Value = tracer.result.size;
            color.Value = tracer.result.color;
        }
    }
}
