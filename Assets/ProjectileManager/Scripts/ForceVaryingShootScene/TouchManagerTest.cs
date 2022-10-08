/*
    Projectile Manager
    ©︎   2020 Shuji Hori
 */

using UnityEngine;

namespace ProjectileManager
{
    public class TouchManagerTest : MonoBehaviour
    {
        public bool useDefaultSetting;
        
        public GameObject projectile;
        public GameObject shootMgr;
        private Vector3 markPos;
        private float force = 100f;

        ProjectileThrow projectileThrow;
        TouchState info;

        public Vector3 MarkPos
        {
            get { return markPos; }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (useDefaultSetting)
            {
                shootMgr = GameObject.Find("ShootPoint");
            }
            projectileThrow = shootMgr.GetComponent<ProjectileThrow>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 touchPos;
            info = TouchControl.GetTouchState();
            switch (info)
            {
                case TouchState.Start:
                    touchPos = TouchControl.GetTouchPosition();
                    projectileThrow.DisplayTrajectoryByDirection(shootMgr.transform.forward, force);
                    break;

                case TouchState.Moved:
                    touchPos = TouchControl.GetTouchPosition();
                    projectileThrow.DisplayTrajectoryByDirection(shootMgr.transform.forward, force);
                    break;

                case TouchState.Ended:
                    touchPos = TouchControl.GetTouchPosition();
                    projectileThrow.ShootObjectByDirection(projectile, shootMgr.transform.forward, force);
                    
                    break;

                default:
                    break;
            }
        }
    }
}
