/*
    Projectile Manager
    ©︎   2020 Shuji Hori
 */
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileManager
{
    [RequireComponent(typeof(LineRenderer))]
    public class ProjectileThrow : MonoBehaviour
    {
        public LaunchType launchType;
        [Tooltip("Uncheck to customize your own setting")]
        public bool useDefaultSetting;
        [Tooltip("Select a ignoring layer for collsion calculation")]
        public LayerMask ignoreLayer;
        public bool showTrajectory;
        public bool showTrajectoryVertex;
        public bool showTrajectoryAlways;

        public int archLineCount;//Count of vertices which Line Renderer renders. Starting from hit position.
        public float archCalcInterval;//Resolution of trajectory arch
        public float archHeightLimit;
        [Range(0, 90)]
        public float throwAngle;
        public float shootRange;

        [Tooltip("Required when showTrajectory is true")]
        public GameObject markPref;//Used to visualize arch vertices
        [Tooltip("Required")]
        public GameObject ground;
        private GameObject shootPoint;

        public float startLineWidth;//Width of Line where it starts
        public float endLineWidth;//Width of Line where it ends
        public Color startColor;//Color of Line where it starts
        public Color endColor;//Color of Line where it ends
        [Range(0, 1)]
        public float startAlpha;
        [Range(0, 1)]
        public float endAlpha;


        private float spdVec;
        private List<GameObject> archObj;
        private Vector3 shootVec;
        LineRenderer line;

        public enum ArchLimit
        {
            Height = 0,
            Time = 1
        }

        void Start()
        {
            InitializeComponents();
            
            if (useDefaultSetting)
            {
                InitializeVariables();
            }
        }

        private void InitializeComponents()
        {
            shootPoint = gameObject;
            archObj = new List<GameObject>();
            line = GetComponent<LineRenderer>();

            line.startWidth = endLineWidth;
            line.endWidth = startLineWidth;
            line.material = new Material(Shader.Find("Sprites/Default"));
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(endColor, 0.0f), new GradientColorKey(startColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(endAlpha, 0.0f), new GradientAlphaKey(startAlpha, 1.0f) }
            );
            line.colorGradient = gradient;
        }

        private void InitializeVariables()
        {
            showTrajectory = true;
            showTrajectoryVertex = false;
            showTrajectoryAlways = false;

            archLineCount = 50;
            archCalcInterval = 0.2f;
            archHeightLimit = GameObject.Find("Ground").transform.position.y;
            shootRange = 200f;

            startLineWidth = 0.1f;
            endLineWidth = 0.1f;

            startColor = Color.blue;
            endColor = Color.blue;

            startAlpha = 0.7f;
            endAlpha = 0.1f;

            switch (launchType)
            {
                case LaunchType.SetForce:
                    throwAngle = 0f;
                    break;
                default:
                    throwAngle = 45f;
                    break;
            }
        }

        public void CheckVector(Vector3 hitPos)
        {
            switch (launchType)
            {
                default:
                    spdVec = CalculateVectorFromAngle(hitPos, throwAngle);
                    if (spdVec <= 0.0f)
                    {
                        Debug.Log("impossible hit point");
                        return;
                    }

                    shootVec = ConvertVectorToVector3(spdVec, throwAngle, hitPos);
                    if (showTrajectory)
                    {
                        DisplayTrajectory(hitPos);
                    }
                    else
                    {
                        ClearTrajectory();
                    }
                    break;
            }
        }

        private float CalculateVectorFromAngle(Vector3 pos, float angle)
        {
            Vector2 shootPos = new Vector2(shootPoint.transform.position.x,
                shootPoint.transform.position.z);
            Vector2 hitPos = new Vector2(pos.x, pos.z);
            float x = Vector2.Distance(shootPos, hitPos);
            float g = Physics.gravity.y;
            float y0 = shootPoint.transform.position.y;
            float y = pos.y;
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float tan = Mathf.Tan(rad);

            float v0Sq = g * x * x / (2 * cos * cos * (y - y0 - x * tan));
            if (v0Sq <= 0.0f)
            {
                return 0.0f;
            }
            return Mathf.Sqrt(v0Sq);
        }

        private Vector3 ConvertVectorToVector3(float spdVec, float angle, Vector3 pos)
        {
            Vector3 shootPos = shootPoint.transform.position;
            Vector3 hitPos = pos;
            shootPos.y = 0f;
            hitPos.y = 0f;

            Vector3 dir = (hitPos - shootPos).normalized;
            Quaternion Rot3D = Quaternion.FromToRotation(Vector3.right, dir);
            Vector3 vec = spdVec * Vector3.right;
            vec = Rot3D * Quaternion.AngleAxis(angle, Vector3.forward) * vec;

            return vec;
        }

        public void ShootObject(GameObject shootObj, Vector3 hitPos)
        {
            int layerMask = -1;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(hitPos);
            Vector3 markPos = new Vector3();
            if (Physics.Raycast(ray, out hit, shootRange, layerMask))
            {
                markPos = Camera.main.ScreenToWorldPoint(new Vector3(hitPos.x, hitPos.y, hit.distance));   
            }
            CheckVector(markPos);
            Debug.Log(markPos);

            if (shootObj == null)
            {
                throw new System.NullReferenceException("shootObj");
            }
            if (shootPoint == null)
            {
                throw new System.NullReferenceException("shootPoint");
            }

            GameObject obj = Instantiate(shootObj, shootPoint.transform.position, Quaternion.identity);
            Rigidbody rig = obj.GetComponent<Rigidbody>();
            Vector3 force = shootVec * rig.mass;
            rig.AddForce(force, ForceMode.Impulse);

            if (showTrajectoryAlways)
            {
                line.positionCount = 0;
                foreach (GameObject vert in archObj)
                {
                    Destroy(vert, 0f);
                }
            }
        }

        public void ShootObjectByDirection(GameObject shootObj, Vector3 dir, float force)
        {
            DisplayTrajectoryByDirection(dir, force);
            GameObject obj = Instantiate(shootObj, shootPoint.transform.position, Quaternion.identity);
            Rigidbody rig = obj.GetComponent<Rigidbody>();
            Vector3 forceDir = dir * rig.mass * force;
            rig.AddForce(forceDir, ForceMode.Impulse);

            if (showTrajectoryAlways)
            {
                line.positionCount = 0;
                foreach (GameObject vert in archObj)
                {
                    Destroy(vert, 0f);
                }
            }
        } 

        public void ShootAtObject(GameObject shootObj, Vector3 hitPos)
        {
            CheckVector(hitPos);
            Debug.Log(hitPos);

            if (shootObj == null)
            {
                throw new System.NullReferenceException("shootObj");
            }
            if (shootPoint == null)
            {
                throw new System.NullReferenceException("shootPoint");
            }

            GameObject obj = Instantiate(shootObj, shootPoint.transform.position, Quaternion.identity);
            Rigidbody rig = obj.GetComponent<Rigidbody>();
            Vector3 force = shootVec * rig.mass;
            rig.AddForce(force, ForceMode.Impulse);

            if (showTrajectoryAlways)
            {
                line.positionCount = 0;
                foreach (GameObject vert in archObj)
                {
                    Destroy(vert, 0f);
                }
            }
        }

        private void DisplayTrajectory(Vector3 hitPos)
        {
            foreach (GameObject obj in archObj)
            {
                Destroy(obj, 0f);
            }
            archObj.Clear();

            float x;
            float y = shootPoint.transform.position.y;
            float y0 = y;
            float g = Physics.gravity.y;
            float rad = throwAngle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            float time;

            List<Vector3> archVerts = new List<Vector3>();
            Vector3 shootPos3 = shootPoint.transform.position;
            hitPos.y = shootPos3.y;
            Vector3 dir = (hitPos - shootPos3).normalized;
            float spd = spdVec;
            Quaternion yawRot = Quaternion.FromToRotation(Vector3.right, dir);
            RaycastHit hit;

            for (int i = 0; y > archHeightLimit; i++)
            {
                time = archCalcInterval * i;
                x = spd * cos * time;
                y = spd * sin * time + y0 + g * time * time / 2;
                archVerts.Add(new Vector3(x, y, 0));
                archVerts[i] = yawRot * archVerts[i];
                archVerts[i] = new Vector3(archVerts[i].x + shootPos3.x, archVerts[i].y, archVerts[i].z + shootPos3.z);

                if (i > 0)
                {
                    if (Physics.Linecast(archVerts[i - 1], archVerts[i], out hit, ignoreLayer))
                    {
                        archVerts[i] = hit.point;
                        if (showTrajectoryVertex)
                        {
                            archObj.Add(Instantiate(markPref, hit.point, Quaternion.identity));
                            archObj[i].transform.parent = shootPoint.transform;
                        }
                        break;
                    }
                }
                if (showTrajectoryVertex)
                {
                    archObj.Add(Instantiate(markPref, archVerts[i], Quaternion.identity));
                    archObj[i].transform.parent = shootPoint.transform;
                }
            }
            int lineLength = archLineCount;
            archVerts.Reverse();
            if (archVerts.Count < lineLength)
            {
                lineLength = archVerts.Count;
            }

            line.startWidth = endLineWidth;
            line.endWidth = startLineWidth;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(endColor, 0.0f), new GradientColorKey(startColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(endAlpha, 0.0f), new GradientAlphaKey(startAlpha, 1.0f) }
            );
            line.colorGradient = gradient;
            line.positionCount = archVerts.Count - (archVerts.Count - lineLength);
            line.SetPositions(archVerts.ToArray());
            line.useWorldSpace = true;
        }

        public void DisplayTrajectoryByDirection(Vector3 dir, float force)
        {
            foreach (GameObject obj in archObj)
            {
                Destroy(obj, 0f);
            }
            archObj.Clear();

            Vector3 dirNor = dir.normalized;

            float x;
            float y = shootPoint.transform.position.y;
            float y0 = y;
            float g = Physics.gravity.y;
            float rad = throwAngle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            float time;

            List<Vector3> archVerts = new List<Vector3>();
            Vector3 shootPos3 = shootPoint.transform.position;
            //hitPos.y = shootPos3.y;
            //Vector3 dir = (hitPos - shootPos3).normalized;
            float spd = force;
            Quaternion yawRot = Quaternion.FromToRotation(Vector3.right, dir);
            RaycastHit hit;

            for (int i = 0; y > archHeightLimit; i++)
            {
                time = archCalcInterval * i;
                x = spd * cos * time;
                y = spd * sin * time + y0 + g * time * time / 2;
                archVerts.Add(new Vector3(x, y, 0));
                archVerts[i] = yawRot * archVerts[i];
                archVerts[i] = new Vector3(archVerts[i].x + shootPos3.x, archVerts[i].y, archVerts[i].z + shootPos3.z);

                if (i > 0)
                {
                    if (Physics.Linecast(archVerts[i - 1], archVerts[i], out hit, ignoreLayer))
                    {
                        archVerts[i] = hit.point;
                        if (showTrajectoryVertex)
                        {
                            archObj.Add(Instantiate(markPref, hit.point, Quaternion.identity));
                            archObj[i].transform.parent = shootPoint.transform;
                        }
                        break;
                    }
                }
                if (showTrajectoryVertex)
                {
                    archObj.Add(Instantiate(markPref, archVerts[i], Quaternion.identity));
                    archObj[i].transform.parent = shootPoint.transform;
                }
            }
            int lineLength = archLineCount;
            archVerts.Reverse();
            if (archVerts.Count < lineLength)
            {
                lineLength = archVerts.Count;
            }

            line.startWidth = endLineWidth;
            line.endWidth = startLineWidth;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(endColor, 0.0f), new GradientColorKey(startColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(endAlpha, 0.0f), new GradientAlphaKey(startAlpha, 1.0f) }
            );
            line.colorGradient = gradient;
            line.positionCount = archVerts.Count - (archVerts.Count - lineLength);
            line.SetPositions(archVerts.ToArray());
            line.useWorldSpace = true;
        }

        void ClearTrajectory()
        {
            line.positionCount = 0;
            foreach (GameObject obj in archObj)
            {
                Destroy(obj);
            }
        }
    }

    public enum LaunchType
    {
        SetPosition,
        SetTarget,
        SetForce
    }
}
