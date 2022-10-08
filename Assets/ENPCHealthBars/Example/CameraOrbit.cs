using UnityEngine;

namespace SnazzlebotTools.ENPCHealthBars
{
    public class CameraOrbit : MonoBehaviour
    {
        public Transform Target;

        [Range(0, 5)]
        public int Speed = 2;

        void Update()
        {
            if (Target == null)
                return;

            transform.Translate(Vector3.right * Time.deltaTime * Speed);
            transform.LookAt(Target);
        }
    }
}