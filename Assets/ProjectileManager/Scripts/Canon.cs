using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileManager
{
    public class Canon : MonoBehaviour
    {
        public TouchManager touchManager;

        private void Update()
        {
            Vector3 markPos = touchManager.markPos;
            RotateCanon(markPos);
        }

        public void RotateCanon(Vector3 hitPos)
        {
            hitPos.y = transform.position.y;
            
            transform.LookAt(hitPos);
        }
    }
}