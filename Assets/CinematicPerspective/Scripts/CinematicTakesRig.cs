

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CinematicPerspective;

namespace CinematicPerspective
{
    [ExecuteInEditMode]
    public class CinematicTakesRig : MonoBehaviour
    {
        public string rigName;
        public enum OverrideCameraMode
        {
            Default = -1,
            LookAt,
            Steadicam,
            Fixed,
            Dolly
        }

        public bool usesTransitions = false;

        private bool lastUsesTransition = false;

        public Camera overrideCamera = null;

        public Vector3 cameraPositionOffset = Vector3.zero;
        public Vector3 cameraRotation = Vector3.zero;

        public bool overrideRange;
        // Este Valor se superpone a el valor general en CameraRigPoints
        public float range = 0;
        private RigCameraTransition m_camTrans;
        public RigCameraTransition CameraTransitions
        {
            get
            {
                if (usesTransitions)
                {
                    return m_camTrans = m_camTrans ?? GetComponent<RigCameraTransition>();
                }
                else
                {
                    if (m_camTrans != null)
#if UNITY_EDITOR
                        CheckTransitionComponent();
#else
                        Destroy(m_camTrans);
#endif
                        return m_camTrans = null;
                }
                    
            }
        }

        public bool isActive
        {
            get
            {
                return cinematicTakes.selectedRig == this.transform;
            }
        }

        void OnValidate()
        {
            Validate();
        }



        public void CheckTransitionComponent()
        {
#if UNITY_EDITOR
            
                var component = GetComponent<RigCameraTransition>();
                if (usesTransitions)
                {
                    if (component == null)
                    {
                        gameObject.AddComponent<RigCameraTransition>();

                    }
                }
                else
                {
                    if (component != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(component);
                        };
                    }
                }

#endif
        }

        private void Validate()
        {
             if (usesTransitions != lastUsesTransition)              
                CheckTransitionComponent();
            lastUsesTransition = usesTransitions;

            lookAtMode.minZoom = Mathf.Clamp(lookAtMode.minZoom, 0, lookAtMode.maxZoom);
            lookAtMode.maxZoom = Mathf.Clamp(lookAtMode.maxZoom, lookAtMode.minZoom, 179);
            dollyMode.minAutoZoom = Mathf.Clamp(dollyMode.minAutoZoom, 0, dollyMode.maxAutoZoom);
            dollyMode.maxAutoZoom = Mathf.Clamp(dollyMode.maxAutoZoom, dollyMode.minAutoZoom, 179);
            dollyMode.ValidateSpeedCurve();
            range = range < .1f ? .1f : range;
            SetName();
        }

        private void Start()
        {
            Validate();
        }



        private void SetName()
        {
            if (rigName == null || rigName == "")
            {
                rigName = this.name.Split('[')[0];
            }
            name = (rigName + string.Format(" [{0}]", System.Enum.GetName(typeof(OverrideCameraMode), rigCameraMode))).Trim();
        }

        public CinematicTakes.DefaultCameraMode selectedRigMode
        {
            get
            {
                if (rigCameraMode == OverrideCameraMode.Default)
                    return cinematicTakes.defaultCameraMode;
                else
                    return (CinematicTakes.DefaultCameraMode)rigCameraMode;
            }
        }

        public bool overridesMode
        {
            get
            {
                return rigCameraMode != OverrideCameraMode.Default;
            }
        }

        public OverrideCameraMode rigCameraMode = OverrideCameraMode.Default;
        public FixedCameraMode fixedMode = new FixedCameraMode();
        public SteadyCameraMode steadyMode = new SteadyCameraMode();
        public LookAtCameraMode lookAtMode = new LookAtCameraMode();
        public DollyCameraMode dollyMode = new DollyCameraMode();
        [HideInInspector]
        public bool editCamera = true;

        private CinematicTakes _cinematicTakes;
        public CinematicTakes cinematicTakes
        {
            get
            {
                if (transform.parent == null) return null;
                return _cinematicTakes = _cinematicTakes ?? transform.parent.GetComponent<CinematicTakes>();
            }
        }

        public bool willTransition {
            get
            {
                return GetComponent<RigCameraTransition>().DoTransition;
            }
        }

        public void MoveAndAlignCamera()
        {
#if UNITY_EDITOR

            var view = UnityEditor.SceneView.lastActiveSceneView;
            if (view != null)
            {
                if (!view.orthographic)
                {
                    // In perspective mode the new rig will have the exact perspective in the scene
                    cameraPositionOffset = view.camera.transform.position - transform.position;
                    cameraRotation = view.camera.transform.rotation.eulerAngles;

                }
                else
                {
                    // In Orthographic mode the new rig will center to scene but will have the same y and rotation that the previous rig
                    cameraPositionOffset = view.camera.transform.position - transform.position;
                    transform.position = new Vector3(transform.position.x, cinematicTakes.rigs[cinematicTakes.rigs.Count - 1].position.y, transform.position.z);
                    cameraRotation = cinematicTakes.rigs[cinematicTakes.rigs.Count - 1].rotation.eulerAngles;
                }

            }
            UnityEditor.SceneView.RepaintAll();
#endif
        }



        public void MoveAndAlign()
        {
#if UNITY_EDITOR
            var view = UnityEditor.SceneView.lastActiveSceneView;
            if (view != null)
            {
                if(!view.orthographic)
                {
                    // In perspective mode the new rig will have the exact perspective in the scene
                    transform.position = view.camera.transform.position;
                    transform.rotation = view.camera.transform.rotation;
                }
                else
                {
                    // In Orthographic mode the new rig will center to scene but will have the same y and rotation that the previous rig
                    transform.position = view.camera.transform.position;
                    transform.position = new Vector3(transform.position.x,cinematicTakes.rigs[cinematicTakes.rigs.Count - 1].position.y,transform.position.z);
                    transform.rotation = Quaternion.identity;
                }
            }
            MoveAndAlignCamera();
#endif
        }

        /// <summary>
        /// Deletes the Rig GameObject
        /// </summary>
        public void DeleteRig()
        {
#if UNITY_EDITOR

            UnityEditor.Selection.activeObject = null;
            var gameObject = this.gameObject;
            UnityEditor.Selection.activeObject = transform.parent.parent;
            //Object.DestroyImmediate(this);
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(this.gameObject);
            };
#else
 
            Object.Destroy(this);
            Object.Destroy(gameObject);
            cinematicTakes.GetRigs();  
#endif
        }

        void OnDestroy()
        {
            try
            {
                cinematicTakes.GetRigs();
            }
            catch
            {
                throw;
            }
        }


    }
}