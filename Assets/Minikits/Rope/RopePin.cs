using UnityEngine;
using Unity.Mathematics;

namespace RopeMinikit
{
    public class RopePin : MonoBehaviour
    {
        public Rope rope;
        [DisableInPlayMode, Range(0.0f, 1.0f)] public float ropeLocation = 0.0f;
        public bool automaticallyFindRopeLocation = false;
        public new Transform transform;
        public Vector3 localPoint;

        protected bool initialized;
        protected int particleIndex;

        public void OnRopeSplit(Rope.OnSplitParams p)
        {
            if (automaticallyFindRopeLocation)
            {
                // There is no way to determine which side of the split this component was located, just remove it...
                Destroy(this);
            }
            else
            {
                var idx = p.preSplitMeasurements.GetParticleIndexAt(ropeLocation * p.preSplitMeasurements.realCurveLength);
                if (idx < p.minParticleIndex || idx > p.maxParticleIndex)
                {
                    Destroy(this);
                }
            }
        }

        protected void Initialize()
        {
            if (automaticallyFindRopeLocation)
            {
                var point = transform.TransformPoint(localPoint);
                rope.GetClosestParticle(point, out particleIndex, out float distance);
                ropeLocation = rope.GetScalarDistanceAt(particleIndex);
            }
            else
            {
                var ropeDistance = ropeLocation * rope.measurements.realCurveLength;
                particleIndex = rope.GetParticleIndexAt(ropeDistance);
            }
            rope.SetMassMultiplierAt(particleIndex, 0.0f);

            initialized = true;
        }

        public void OnEnable()
        {
            if (initialized)
            {
                rope.SetMassMultiplierAt(particleIndex, 0.0f);
            }
        }

        public void FixedUpdate()
        {
            if (rope == null || transform == null)
            {
                return;
            }
            if (!initialized)
            {
                Initialize();
            }
            rope.SetPositionAt(particleIndex, transform.TransformPoint(localPoint));
        }

        public void OnDisable()
        {
            if (initialized)
            {
                rope.SetMassMultiplierAt(particleIndex, 1.0f);
            }
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                return;
            }
            if (rope == null || transform == null || rope.spawnPoints.Count < 2)
            {
                return;
            }

            var point = transform.TransformPoint(localPoint);
            Gizmos.color = new Color(0.69f, 0.0f, 1.0f);
            Gizmos.DrawWireCube(point, Vector3.one * 0.05f);

            if (!automaticallyFindRopeLocation)
            {
                var localToWorld = (float4x4)rope.transform.localToWorldMatrix;
                var ropeLength = rope.spawnPoints.GetLengthOfCurve(ref localToWorld);
                rope.spawnPoints.GetPointAlongCurve(ref localToWorld, ropeLength * ropeLocation, out float3 ropePoint);

                Gizmos.DrawWireCube(ropePoint, Vector3.one * 0.05f);
                Gizmos.DrawLine(ropePoint, point);
            }
        }
#endif
    }
}
