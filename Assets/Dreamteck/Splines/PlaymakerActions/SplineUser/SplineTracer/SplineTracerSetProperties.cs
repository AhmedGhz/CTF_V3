using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Sets the properties of a SplineTracer")]
    public class SplineTracerSetProprties : SplineUserSetProperties
    {
        public FsmBool setTriggers;
        public FsmBool useTriggers;
        public FsmInt triggerGroup;

        public FsmBool setPhysicsMode;
        public SplineTracer.PhysicsMode physicsMode;

        public FsmBool setPositionOffset;
        public FsmVector2 positionOffset;

        public FsmBool setApplyPosition;
        public FsmBool applyPositionX;
        public FsmBool applyPositionY;
        public FsmBool applyPositionZ;

        public FsmBool setRotationOffset;
        public FsmVector3 rotationOffset;

        public FsmBool setApplyRotation;
        public FsmBool applyRotationX;
        public FsmBool applyRotationY;
        public FsmBool applyRotationZ;

        public FsmBool setVelocityHandleMode;
        public TransformModule.VelocityHandleMode velocityHandleMode;


        private SplineTracer _tracer = null;

        public override void Reset()
        {
            base.Reset();
            setTriggers = new FsmBool();
            useTriggers = new FsmBool();
            triggerGroup = new FsmInt();

            setPhysicsMode = new FsmBool();
            physicsMode = SplineTracer.PhysicsMode.Transform;

            setPositionOffset = new FsmBool();
            positionOffset = new FsmVector2();

            setApplyPosition = new FsmBool();
            applyPositionX = new FsmBool();
            applyPositionY = new FsmBool();
            applyPositionZ = new FsmBool();

            setRotationOffset = new FsmBool();
            rotationOffset = new FsmVector3();

            setApplyRotation = new FsmBool();
            applyRotationX = new FsmBool();
            applyRotationY = new FsmBool();
            applyRotationZ = new FsmBool();

            setVelocityHandleMode = new FsmBool();
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
            if (setTriggers.Value)
            {
                _tracer.triggerGroup = triggerGroup.Value;
                _tracer.useTriggers = useTriggers.Value;
            }
            if (setPhysicsMode.Value) _tracer.physicsMode = physicsMode;

            if (setPositionOffset.Value) _tracer.motion.offset = positionOffset.Value;

            if (setApplyPosition.Value)
            {
                _tracer.motion.applyPositionX = applyPositionX.Value;
                _tracer.motion.applyPositionY = applyPositionY.Value;
                _tracer.motion.applyPositionZ = applyPositionZ.Value;
            }

            if (setRotationOffset.Value) _tracer.motion.rotationOffset = rotationOffset.Value;

            if (setApplyRotation.Value)
            {
                _tracer.motion.applyRotationX = applyRotationX.Value;
                _tracer.motion.applyRotationY = applyRotationY.Value;
                _tracer.motion.applyRotationZ = applyRotationZ.Value;
            }

            if (setVelocityHandleMode.Value) _tracer.motion.velocityHandleMode = velocityHandleMode;
        }
    }
}
