using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Evaluates the sampled result of a SplineUser and returns the position, rotation, size and color from the evaluation")]
    public class SplineUserEvaluate : SplineUserAction
    {
        public FsmFloat percent;
        public FsmVector3 position;
        public FsmVector3 normal;
        public FsmVector3 right;
        public FsmVector3 eulerRotation;
        public FsmFloat size;
        public FsmColor color;

        public override void Reset()
        {
            base.Reset();
            position = null;
            normal = null;
            right = null;
            eulerRotation = null;
            size = null;
            color = null; 
        }

        protected override void RunAction()
        {
            base.RunAction();
            SplineSample result = user.Evaluate(percent.Value);
            position.Value = result.position;
            normal.Value = result.up;
            right.Value = result.right;
            eulerRotation.Value = result.rotation.eulerAngles;
            size.Value = result.size;
            color.Value = result.color;
        }
    }
}