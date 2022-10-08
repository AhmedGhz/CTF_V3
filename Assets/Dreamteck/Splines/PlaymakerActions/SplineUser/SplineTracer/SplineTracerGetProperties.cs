using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Gets the properties of a SplineTracer")]
    public class SplineTracerGetProperties : SplineUserGetProperties
    {
        public FsmBool useTriggers;
        public FsmInt triggerGroup;
        public SplineTracer.PhysicsMode physicsMode;
        public FsmVector2 positionOffset;
        public FsmBool applyPositionX;
        public FsmBool applyPositionY;
        public FsmBool applyPositionZ;
        public FsmVector3 rotationOffset;
        public FsmBool applyRotationX;
        public FsmBool applyRotationY;
        public FsmBool applyRotationZ;
        public TransformModule.VelocityHandleMode velocityHandleMode;


        private SplineTracer _tracer = null;

        public override void Reset()
        {
            base.Reset();
            useTriggers = new FsmBool();
            triggerGroup = new FsmInt();
            physicsMode = SplineTracer.PhysicsMode.Transform;
            positionOffset = new FsmVector2();
            applyPositionX = new FsmBool();
            applyPositionY = new FsmBool();
            applyPositionZ = new FsmBool();
            rotationOffset = new FsmVector3();
            applyRotationX = new FsmBool();
            applyRotationY = new FsmBool();
            applyRotationZ = new FsmBool();
            velocityHandleMode = TransformModule.VelocityHandleMode.Zero;
        }

        protected override bool OnOwnerUpdate()
        {
            if (base.OnOwnerUpdate())
            {
                if (user is SplineTracer)
                {
                    _tracer = (SplineTracer)user;
                    return true;
                }
                else
                {
                    _tracer = null;
                }
            }
            return false;
        }

        protected override void RunAction()
        {
            base.RunAction();
            triggerGroup.Value = _tracer.triggerGroup;
            useTriggers.Value = _tracer.useTriggers;
            physicsMode = _tracer.physicsMode;
            positionOffset.Value = _tracer.motion.offset;
            applyPositionX.Value = _tracer.motion.applyPositionX;
            applyPositionY.Value = _tracer.motion.applyPositionY;
            applyPositionZ.Value = _tracer.motion.applyPositionZ;
            rotationOffset.Value = _tracer.motion.rotationOffset;
            applyRotationX.Value = _tracer.motion.applyRotationX;
            applyRotationY.Value = _tracer.motion.applyRotationY;
            applyRotationZ.Value = _tracer.motion.applyRotationZ;
            velocityHandleMode = _tracer.motion.velocityHandleMode;
        }
    }
}
