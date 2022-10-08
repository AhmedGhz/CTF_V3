using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Sets the properties of a Spline Follower")]
    public class SplineFollowerSetProperties : SplineTracerSetProprties
    {
        public FsmBool setSpeed;
        public FsmFloat speed;
        public FsmBool setFollowState;
        public FsmBool follow;
        public FsmBool setWrapMode;
        public SplineFollower.Wrap wrapMode;
        public FsmBool setDirection;
        public Spline.Direction direction;
        public FsmBool setFaceDirection;
        public FsmBool faceDirection;

        private SplineFollower _follower;

        public override void Reset()
        {
            base.Reset();
            speed = new FsmFloat();
            setSpeed = new FsmBool();
            setFollowState = new FsmBool();
            follow = new FsmBool();
            setWrapMode = new FsmBool();
            wrapMode = SplineFollower.Wrap.Default;
            setDirection = new FsmBool();
            direction = Spline.Direction.Forward;
            setFaceDirection = new FsmBool();
            faceDirection = new FsmBool();
        }

        protected override bool OnOwnerUpdate()
        {
            if (base.OnOwnerUpdate())
            {
                if (user is SplineFollower)
                {
                    _follower = (SplineFollower)user;
                    return true;
                }
                else
                {
                    _follower = null;
                }
            }
            return false;
        }

        protected override void RunAction()
        {
            base.RunAction();
            if (setSpeed.Value)  _follower.followSpeed = speed.Value;
            if(setFollowState.Value) _follower.follow = follow.Value;
            if (setWrapMode.Value) _follower.wrapMode = wrapMode;
            if(setFaceDirection.Value) _follower.applyDirectionRotation = faceDirection.Value;
            if(setDirection.Value) _follower.direction = direction;
        }
    }
}