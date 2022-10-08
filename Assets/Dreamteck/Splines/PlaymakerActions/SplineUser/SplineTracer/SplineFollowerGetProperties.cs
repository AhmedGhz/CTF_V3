using UnityEngine;
using Dreamteck.Splines;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Spline User")]
    [Tooltip("Gets the properties of a Spline Follower")]
    public class SplineFollowerGetProperties : SplineTracerGetProperties
    {
        public FsmFloat speed;
        public FsmBool follow;
        public SplineFollower.Wrap wrapMode;
        public Spline.Direction direction;
        public FsmBool faceDirection;

        private SplineFollower _follower;

        public override void Reset()
        {
            base.Reset();
            speed = new FsmFloat();
            follow = new FsmBool();
            wrapMode = SplineFollower.Wrap.Default;
            direction = Spline.Direction.Forward;
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
            speed.Value = _follower.followSpeed;
            follow.Value = _follower.follow;
            wrapMode = _follower.wrapMode;
            faceDirection.Value = _follower.applyDirectionRotation;
            direction = _follower.direction;
        }
    }
}
