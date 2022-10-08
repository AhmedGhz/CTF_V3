using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using CinematicPerspective;
using System.Linq;
using System;

namespace CinematicPerspective
{
    /// <summary>
    /// In this mode the camera will be placed in a fixed position and will face have the rotation of the selected rig
    /// </summary>
    [System.Serializable]
    public class FixedCameraMode
    {
        public bool FaceTargetOnEnter;
        [Range(10, 170)]
        public float zoomLevel = 60;
    }

    /// <summary>
    /// In this mode the camera will travel with a certain target, with an offset
    /// If chosen a simluated perlin noised base bobbing / shake can be added
    /// Shake is present if bobbing > 0;
    /// </summary>
    [System.Serializable]
    public class SteadyCameraMode
    {
        public Transform followTarget;                      // If null, camera will choose the general config target
        [Range(10, 170)]
        public float steadyZoomLevel = 30;                  // The zoomlevel can be defined here
        [HideInInspector]
        public Vector3 cameraRotation, cameraOffset;               
        [Range(0, 1)]
        public float bobbing = 0.06f;
        [Range(0.1f, 10)]
        public float shakiness = 1.5f;
        /// <summary>
        /// Direction the camera will face
        /// </summary>
        public enum facing
        {
            Target,
            Front,
            CameraDirection
        }
        public facing m_facing;
    }

    /// <summary>
    /// In this mode the camera will be placed in a fixed position, and will follow a target.
    /// </summary>
    [System.Serializable]
    public class LookAtCameraMode
    {
        [HideInInspector]
        public float fovCoef;
        public bool autoZoom = true;                        // The camera will fov according to the target position, zoomin in on more distance
        public bool inverseZoom = false;                    // Same as previous but inverted
        [Range(10, 170)]
        public float staticZoom = 50f;
        [Range(10, 170)]
        public float minZoom = 30f;
        [Range(10, 170)]
        public float maxZoom = 60f;
        [Range(0, 5)]
        public float zoomTransitionSpeed = 1f;

    }

    /// <summary>
    /// In this mode the camera will travel between the selected rig and the next rig 
    /// in the given travelTime according to the speedCurve. Once arrived the next rig 
    /// will be the selected rig.
    /// </summary>
    [System.Serializable]
    public class DollyCameraMode
    {
        
        public Vector3 destination = new Vector3(10f, 5f, 10f);                   // If null, camera will choose the general config target   
        public AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(5000, 0));                   // The curve in which the travel spped will be applied
        public float travelTime = 5000;                     // Time in ms to travel to the next rig 
        public bool reverseDirection;
        public bool lookAtTarget = true;              // If active the cam will always face the target
        [Range(10, 1000)]
        public float followSpeed = 100;               
        private float startTime;
        public bool autoZoom = true;                  // Taken from lookat, the camera will fov according to the target position
        public bool inverseAutoZoom = false;              // Same as previous but inverted   
           
        [Range(10, 170)]
        public float minAutoZoom = 30;
        [Range(10, 170)]
        public float maxAutoZoom = 60;
        [Range(10, 170)]
        public float manualZoomLevel = 60;                  // If !travelAutoZoom the zoomlevel can be defined here

        internal void ResetTravel()
        {
            startTime = Time.time;
        }

        internal void ValidateSpeedCurve()
        {
            //Fixing Speed Curve
            if (speedCurve.keys.Length < 2)
            {
                var length = Vector3.Distance(Vector3.zero, destination);
                speedCurve.keys = new Keyframe[]
                {
                   new Keyframe(0, length), new Keyframe(travelTime, 0)
                };
            }
            var algo = destination.magnitude;

            speedCurve.MoveKey(0, new Keyframe(0, 0));
            speedCurve.MoveKey(speedCurve.keys.Length - 1, new Keyframe(travelTime, algo));
        }


        /// <summary>
        /// Calculates the Travel position relative to time
        /// </summary>
        /// <param name="endPosition">Positon to travel, will be the start position if travel is set to inverse</param>
        /// <returns>Position for current Time</returns>
        internal Vector3 TravelPosition(Vector3 endPosition)
        {
            Vector3 position;
            currentTime = Time.time - startTime;            
            if (reverseDirection)
                currentTime = travelTime / 1000f - (Time.time - startTime);

            currentTime = Mathf.Clamp(currentTime, 0, travelTime / 1000f);

            position = endPosition + (destination).normalized * speedCurve.Evaluate(currentTime * 1000);

            return position;
        }

        [HideInInspector]
        public float currentTime;
    }

    /// <summary>
    /// Main Class
    /// </summary>
    [ExecuteInEditMode, RequireComponent(typeof(CameraController0))]
    public class CinematicTakes : MonoBehaviour
    {

        /// <summary>
        /// Distance to current target if both target and active rig are defined
        /// </summary>
        public float distanceToTarget
        {
            get
            {
                if (selectedRig != null)
                {
                    return m_distanceToTarget;
                }
                else
                {                     
                    if (target == null)
                        Debug.LogWarning("Trying to get distance to target when target is not selected");
                    return -1;
                }
            }
        }

        /// <summary>
        /// If !active, the asset will not work
        /// </summary>
        [Header("Configuration")]
        public bool active = false;
        private bool m_active = false;
        [Range(.1f, 2f)]
        public float cameraGizmoSize = 0.3f;

        /// <summary>
        /// General target to follow and this is the only target that will activate all rigs
        /// </summary>
        public Transform target;

        private Transform m_lastTarget;

        /// <summary>
        /// The camera with which the system will work
        /// </summary>
        public Camera selectedCamera;

        /// <summary>
        /// If true the rig will be placed on a collider surface 
        /// If false, the ig will be placed in the same position and orientation of the current camera
        /// </summary>
        public bool rigToCollider = true;


        /// <summary>
        /// The default mode if not overriden in rigs
        /// </summary>
        [Header("Default Mode Options")]
        public DefaultCameraMode defaultCameraMode;

        /// <summary>
        /// Range for camera changes if not overriden in rigs
        /// </summary>         
        public float defaultRange = 25f;

        /// <summary>
        /// The camera controller component
        /// </summary>
        [SerializeField, HideInInspector]
        private CameraController0 m_cameraController;

        /// <summary>
        /// The camera controller component
        /// </summary>
        public CameraController0 cameraController
        {
            get
            {
                return m_cameraController;
            }
            private set
            {
                m_cameraController = value;
            }
        }


        /// <summary>
        /// True before Start
        /// </summary>   
        private bool notInitialized = true;

        /// <summary>
        /// Configurations for the Fixed Mode
        /// </summary>
        [Tooltip("Fixed Mode camera Options")]
        public FixedCameraMode fixedMode = new FixedCameraMode();

        /// <summary>
        /// Configurations for the Steadicam Mode
        /// </summary>
        [Tooltip("Steadicam Mode camera Options")]
        public SteadyCameraMode steadyMode = new SteadyCameraMode();

        /// <summary>
        /// Configurations for the LookAt Mode
        /// </summary>
        [Tooltip("LookAt Mode camera Options")]
        public LookAtCameraMode lookAtMode = new LookAtCameraMode();

        /// <summary>
        /// Configurations for the Dolly Mode
        /// </summary>
        [Tooltip("Dolly Mode camera Options")]
        public DollyCameraMode dollyMode = new DollyCameraMode();

        /// <summary>
        /// Posible camera modes for default rigs
        /// </summary>
        public enum DefaultCameraMode
        {
            LookAt,
            Steadicam,
            Fixed,
            Dolly
        }

        /// <summary>
        /// Shows more information about the rig and component behaviour
        /// </summary>
        [Tooltip("Shows more information about the rig and component behaviour")]
        public bool extraInfo;

        /// <summary>
        /// Transforms from all the rigs parented to this object
        /// </summary>
        [SerializeField, HideInInspector]
        public List<Transform> m_rigs;

        /// <summary>
        /// Rig that was selected on last frame
        /// </summary>
        [SerializeField, HideInInspector]
        private Transform m_lastRig;

        /// <summary>
        /// Currently selected Rig
        /// </summary>
        [SerializeField, Tooltip("Currently selected Rig")]
        private Transform m_selectedRig;

        /// <summary>
        /// Field of view of the selected camera on current frame
        /// </summary>
        [SerializeField, HideInInspector, Tooltip("Field of view of the selected camera on current frame")]
        private float m_selectedCameraFov;  

        /// <summary>
        /// Distance from camera to target on current frame
        /// </summary>
        [SerializeField, HideInInspector, Tooltip("Distance from camera to target on current frame")]
        private float m_distanceToTarget;

        /// <summary>
        /// Rigs in range on current frame
        /// </summary>
        [SerializeField, HideInInspector, Tooltip("Rigs in range on current frame")]
        private List<Transform> m_rigsInRange;

        /// <summary>
        /// Use this to avoid calling methods that wont need to be updated every frame
        /// </summary>
        private bool placeCamera = true;

        /// <summary>
        /// The mode that was active of the previous Frame
        /// </summary>
        private DefaultCameraMode lastMode;

        /// <summary>
        /// The cinematic Takes rig component of the selected RIg
        /// </summary>
        internal CinematicTakesRig selectedCinematicRig;


        /// <summary>
        /// Rigs that was in range on the previous frame
        /// </summary>
        private List<Transform> m_lastRigsInRange;

        [HideInInspector]
        public bool useDepthOfField;

        /// <summary>
        /// Transforms from all the rigs parented to this object
        /// </summary>
        public List<Transform> rigs
        {
            get
            {
                return m_rigs;
            }
        }

        /// <summary>
        /// Currently selected Rig
        /// </summary>
        public Transform selectedRig
        {
            get
            {
                return m_selectedRig;
            }
            internal set
            {
                m_selectedRig = value;
            }
        }

        

        /// <summary>
        /// This will run before the Start Method
        /// </summary>
        private void Awake()
        {
            notInitialized = true;
            
        }

        /// <summary>
        /// This will run on the changes in the inspector
        /// </summary>
        private void OnValidate()
        {
            Validate();
        }

        private void Validate()
        {
#if UNITY_EDITOR
            if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
            {
                return;

            }
#endif
            Initializer();
            lookAtMode.minZoom = Mathf.Clamp(lookAtMode.minZoom, 0, lookAtMode.maxZoom);
            lookAtMode.maxZoom = Mathf.Clamp(lookAtMode.maxZoom, lookAtMode.minZoom, 179);
            dollyMode.minAutoZoom = Mathf.Clamp(dollyMode.minAutoZoom, 0, dollyMode.maxAutoZoom);
            dollyMode.maxAutoZoom = Mathf.Clamp(dollyMode.maxAutoZoom, dollyMode.minAutoZoom, 179);
            defaultRange = defaultRange < .1f ? .1f : defaultRange;
            dollyMode.ValidateSpeedCurve();
        }

        private void Initializer()
        {
            // We check if we have a target
            // If no target is assigned the component will be marked inactive
            if(target == null)
            {
                Debug.Log("Cinematic Perspectives needs a Target to run");
                active = false; 
                return;
            }
            else
            {
                m_active = true;
            }

            if (active && m_active)
            {
                // We check if we have a camera controller
                if (cameraController == null)
                    cameraController = transform.parent.GetComponentInChildren<CameraController0>();
                // We assign the camera
                if (selectedCamera == null)
                {
                    Debug.LogWarning("Must assign a camera to the Cinematic Perspective Script, asigning main camera!");

                    if (Camera.main != null)
                    {
                        selectedCamera = Camera.main;
                    }
                    else
                    {
                        Debug.LogError("Failed to assign main camera, please assign manually and activate!");
                        active = false;
                    }
                }
                m_rigs.Clear();     //  They must be clearead every time to avoid persistence nulls
                GetRigs();          //  We process the rigs
                // We choose the rig if none has been selected
                if (selectedRig == null && m_rigs.Count > 0)
                {
                    RigSelector();
                }
            }

        }

        /// <summary>
        /// This will run on play and on stop
        /// </summary>
        void Start()
        {
            Validate();
            
#if UNITY_EDITOR
            if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.PrefabInstance)
            {
                UnityEditor.PrefabUtility.DisconnectPrefabInstance(gameObject);

            }
#endif
        }

        /// <summary>
        /// Here is where the camera will apply the selected settings
        /// </summary>
        void LateUpdate()
        {
            if (!Application.isPlaying) return;
            if (cameraController != null && active == true)
            {
                if (target != null && m_rigs.Count > 0)
                {

                    if (selectedCamera != null)
                        m_selectedCameraFov = selectedCamera.fieldOfView;

                    if (m_selectedCameraFov < 0) Debug.Log("Current field of view is negative");

                    RigSelector();
                    if (selectedRig == null) return;
                    RunMode(PlaceCam());
                    UpdateDistance(selectedRig);                    
                }
            }

            if(m_lastTarget != target)
            {
                if(m_lastTarget == null && m_lastRig == null)
                    if (rigs.Count > 0)
                    {
                        m_lastRig = rigs.OrderBy(r => (r.transform.position - target.transform.position).magnitude).FirstOrDefault();
                    }
                m_lastTarget = target;
            }
        }

        /// <summary>
        /// Carga los rigs
        /// </summary>
        public int GetRigs()
        {
            if (m_rigs.Count != transform.childCount)
            {
#if UNITY_EDITOR
                if (UnityEditor.EditorApplication.isPlaying)
                    UnityEditor.SceneView.RepaintAll();                
#endif
                m_rigs.Clear();
                Transform[] rs = this.GetComponentsInChildren<Transform>();
                foreach (Transform r in rs)
                {
                    if (r != transform)
                    {
                        m_rigs.Add(r);
#if UNITY_EDITOR
                        HelperEditor.DrawIcon(r.gameObject, 1);
#endif
                    }
                }

                
            }
            
            return m_rigs.Count;
        }

        private bool _rigIsForced = false;


        /// <summary>
        /// Is a rig forced?
        /// </summary>
        public bool rigIsForced { get { return _rigIsForced; } }


        /// <summary>
        /// Forces a rig to be the active one
        /// </summary>
        /// <param name="rig">Target rig to activate</param>
        public void ForceActiveRig(CinematicTakesRig rig)
        {
            if (rig == null)
            {
                _rigIsForced = false;
                return;
            }

            _rigIsForced = true;
            m_lastRig = selectedRig;
            selectedCinematicRig = rig;
            selectedRig = rig.transform;
            
        }

        /// <summary>
        /// Releases the Forced rig if any
        /// </summary>
        public void ReleaseForcedRig()
        {
            ForceActiveRig(null);
        }


        /// <summary>
        ///  Selects rigs based on distance, closest rigs to mindistance have priority
        /// </summary>
        internal void RigSelector()
        {

            if (_rigIsForced) return;

            GetRigs();
            
            m_rigsInRange = new List<Transform>();
            m_lastRigsInRange = m_lastRigsInRange ?? new List<Transform>();

            foreach (Transform r in m_rigs.Where(r => r != null))
            {
                float rigDistance = defaultRange;

                if (r.GetComponent<CinematicTakesRig>().overrideRange)
                    rigDistance = getRigRange(r.GetComponent<CinematicTakesRig>());

                float presentDistance = Mathf.Abs((target.position - r.position).magnitude);

                if (presentDistance <= rigDistance)
                    m_rigsInRange.Add(r);
         
            }
            
            if (m_rigsInRange.Count > 1)
            {
                var newRig = m_rigsInRange.FirstOrDefault(rig => !m_lastRigsInRange.Contains(rig));                
                selectedRig = newRig ?? selectedRig;
                
            }
            else if(m_rigsInRange.Count == 1)
            {
                selectedRig = m_rigsInRange[0];
            }

            m_lastRigsInRange = m_rigsInRange;

            if(m_selectedRig != null)
                if (m_lastRig != selectedRig)
                    selectedCinematicRig = selectedRig.GetComponent<CinematicTakesRig>();
        }

        /// <summary>
        /// Returns the Rig Range.
        /// </summary>
        /// <param name="rig">Cinematic Takes Rig</param>
        /// <returns></returns>
        public float getRigRange(CinematicTakesRig rig)
        {
            float rigDistance = defaultRange;

                if(rig.overrideRange)
                    rigDistance = rig.range;
                
            return rigDistance;
        }
#if UNITY_EDITOR
        public Action RepaintInspector;
#endif

        /// <summary>
        /// Sets the values for a next camera Change
        /// </summary>
        private void SetInitialValues(DefaultCameraMode mode)
        {
            if (cameraController != null)
            {

#if UNITY_EDITOR
                if(RepaintInspector != null)
                    RepaintInspector();
#endif
                if (m_lastRig != null && m_lastRig.GetComponent<CinematicTakesRig>().usesTransitions)
                    m_lastRig.GetComponent<RigCameraTransition>().StartTransition();

                cameraController.SetCamera(selectedCinematicRig.overrideCamera != null ? selectedCinematicRig.overrideCamera : selectedCamera);
                
                placeCamera = true;
                m_lastRig = selectedRig;

                var lookAtOnEnter = false;
                switch (mode)
                {
                    case DefaultCameraMode.LookAt:
                        lookAtOnEnter = true;
                        break;
                    case DefaultCameraMode.Steadicam:
                        break;
                    case DefaultCameraMode.Fixed:
                        if (selectedCinematicRig.overridesMode)
                        {
                            lookAtOnEnter = selectedCinematicRig.fixedMode.FaceTargetOnEnter;
                        }
                        else
                        {
                            lookAtOnEnter = fixedMode.FaceTargetOnEnter;
                        }
                        break;
                    case DefaultCameraMode.Dolly:
                        if (selectedCinematicRig.overridesMode)
                        {
                            lookAtOnEnter = selectedCinematicRig.dollyMode.lookAtTarget;
                        }
                        else
                        {
                            lookAtOnEnter = dollyMode.lookAtTarget;
                        }
                        break;
                    default:
                        break;
                }
                if (lookAtOnEnter)
                {
                    cameraController.RotateTowards(target);
                }
               

                
            }
            notInitialized = false;
        }

        /// <summary>
        /// Places the camera
        /// </summary>
        /// <returns>The camera mode to be used</returns>
        internal DefaultCameraMode PlaceCam()
        {

            //Current mode to use, checks if selected rig overrides the mode
            var camMode = (int)selectedCinematicRig.rigCameraMode == -1 ? this.defaultCameraMode : (DefaultCameraMode)selectedCinematicRig.rigCameraMode;

            //Resetting the values if something changed
            if (m_lastRig != selectedRig || lastMode != camMode || (notInitialized && active))
            {
                SetInitialValues(camMode);
            }

            //returning the mode to be used, and saving the mode
            return lastMode = camMode;
        }

        /// <summary>
        /// Selects the settings for the Camera Controller
        /// </summary>
        /// <param name="camMode">Camera Mode to use</param>
        private void RunMode(DefaultCameraMode camMode)
        {
            switch (camMode)
            {
                case DefaultCameraMode.LookAt:
                    LookAtMode();
                    break;
                case DefaultCameraMode.Steadicam:
                    SteadyMode();
                    break;
                case DefaultCameraMode.Fixed:
                    FixedMode();
                    break;
                case DefaultCameraMode.Dolly:
                    DollyMode();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the distance from the selected Rig for Debugging purposes
        /// </summary>
        /// <param name="selectedRig">Selected Rig</param>
        private void UpdateDistance(Transform selectedRig)
        {
            m_distanceToTarget = Mathf.Abs((target.position - selectedRig.position).magnitude);
        }

        /// <summary>
        /// The setting of Dolly Mode are passed to the Camera Controller here
        /// </summary>
        private void DollyMode()
        {
            
            //Selecting the mode, checks if selected rig is overriding 
            var dollyMode = (DefaultCameraMode)selectedCinematicRig.rigCameraMode == DefaultCameraMode.Dolly ? selectedCinematicRig.dollyMode : this.dollyMode;

            //Calculating position of travel
            var currentTravelPosition = dollyMode.TravelPosition(selectedRig.transform.position + selectedCinematicRig.cameraPositionOffset);

            //Setting the inital values of Travel
            if (placeCamera)
            {
                dollyMode.ResetTravel();
                placeCamera = false;
                cameraController.MoveTo(currentTravelPosition);
            }

            //Moving the camera to travel position
            cameraController.MoveTo(currentTravelPosition);

            //Setting FOV
            if (dollyMode.autoZoom)
                cameraController.AutoFov(target, dollyMode.minAutoZoom, dollyMode.maxAutoZoom, 25f, dollyMode.inverseAutoZoom, 5);
            else 
                cameraController.SetFov(dollyMode.manualZoomLevel);

            //Setting rotation of Camera
            if (dollyMode.lookAtTarget)
                cameraController.RotateTowards(target);
            else
                cameraController.selectedCamera.transform.rotation =
                    selectedCinematicRig.cameraRotation != Vector3.zero 
                    ? Quaternion.Euler(selectedCinematicRig.cameraRotation) 
                    : selectedRig.rotation;

            

        }

        /// <summary>
        /// The setting of Steady Mode are passed to the Camera Controller here
        /// </summary>
        private void SteadyMode()
        {
            //Selecting the mode, checks if selected rig is overriding 
            SteadyCameraMode mode;
            if (selectedCinematicRig.rigCameraMode != CinematicTakesRig.OverrideCameraMode.Default)
                mode = selectedCinematicRig.steadyMode;
            else
                mode = this.steadyMode;
            //Cheking if steadyMode has target, if not using the global target
            var target = mode.followTarget != null ? mode.followTarget : this.target;

            //Setting the FOV
            cameraController.SetFov(mode.steadyZoomLevel);
            cameraController.MoveTo(target.position + selectedCinematicRig.cameraPositionOffset);

            switch(mode.m_facing)
            {
                case SteadyCameraMode.facing.Target:
                    if (selectedCinematicRig.cameraPositionOffset != Vector3.zero)
                    {
                        cameraController.RotateTowards(target);
                    }
                    else
                    {
                        Debug.Log("With no camera offset facing target will change to facing the camera Direction");
                        mode.m_facing = SteadyCameraMode.facing.CameraDirection;
                    }
                        
                    break;
                case SteadyCameraMode.facing.Front:
                    cameraController.selectedCamera.transform.rotation = target.transform.rotation;
                    break;
                case SteadyCameraMode.facing.CameraDirection:
                    cameraController.selectedCamera.transform.rotation = Quaternion.Euler(selectedCinematicRig.cameraRotation);
                    break;
            }

            if (mode.bobbing != 0)
            {
                float y = mode.bobbing * Mathf.PerlinNoise(Time.time* mode.shakiness, 0.0F);
                Vector3 pos = selectedCamera.transform.position;
                pos.y += y;
                pos.x += y * mode.shakiness * 0.5f;
                pos.z += y * mode.shakiness * 0.5f;
                selectedCamera.transform.position = pos;
            }
        }

        /// <summary>
        /// The setting of Fixed Mode are passed to the Camera Controller here
        /// </summary>
        private void FixedMode()
        {
            //Selecting the mode, checks if selected rig is overriding 
            FixedCameraMode mode;
            if (selectedCinematicRig.rigCameraMode != CinematicTakesRig.OverrideCameraMode.Default)
                mode = selectedCinematicRig.fixedMode;
            else
                mode = this.fixedMode;

            //Setting FOV
            cameraController.SetFov(mode.zoomLevel);

            //Setting initial values
            if (placeCamera)
            {
                //Placing the camera
                cameraController.MoveTo(selectedRig.position + selectedCinematicRig.cameraPositionOffset);

                //Rotating the camera
                if (mode.FaceTargetOnEnter)
                    cameraController.RotateTowards(target);
                else
                    cameraController.selectedCamera.transform.rotation =
                        selectedCinematicRig.cameraRotation != Vector3.zero
                        ? Quaternion.Euler(selectedCinematicRig.cameraRotation)
                        : selectedRig.rotation;

                placeCamera = false;
            }          

        }

        /// <summary>
        /// The setting of LookAt Mode are passed to the Camera Controller here
        /// </summary>
        private void LookAtMode()
        {

            //Selecting the mode, checks if selected rig is overriding 
            LookAtCameraMode mode;
            if (selectedCinematicRig.rigCameraMode != CinematicTakesRig.OverrideCameraMode.Default)
                mode = selectedCinematicRig.lookAtMode;
            else
                mode = this.lookAtMode;


            //Setting initial values
            if (placeCamera)
            {
                //placing the camera
                cameraController.MoveTo(selectedRig.position + selectedCinematicRig.cameraPositionOffset);
                placeCamera = false;
            } 

            //rotating the camera
            cameraController.RotateTowards(target, 0);

            // Auto Fov / Zoom
            if (mode.autoZoom)
                cameraController.AutoFov(target,
                    mode.minZoom,
                    mode.maxZoom,
                    selectedCinematicRig.overrideRange ? selectedCinematicRig.range : defaultRange,
                    mode.inverseZoom,
                    mode.zoomTransitionSpeed);
            else
                cameraController.SetFov(mode.staticZoom);

        }

        /// <summary>
        /// Adds a new reigpoint in the scene
        /// at the current view position and rotation
        /// </summary>
        public void AddRigPoint(Vector3 position)
        {

            //Instantiating the gameObject and parenting
            GameObject rig = new GameObject();
            rig.transform.parent = transform;   // Pareting the controller
            CinematicTakesRig rc = rig.AddComponent<CinematicTakesRig>();   // Adding the component

            if (rigToCollider)
            {
                float precision = 10000f;   // Precision is required as the hit can be different to 0 to surface.
                rig.transform.position = new Vector3(Mathf.Round(position.x * precision) / precision, Mathf.Round(position.y * precision) / precision, Mathf.Round(position.z * precision) / precision);
                rig.transform.position = rig.transform.position + Vector3.up; // Move it up 1 unit to make it more interesting
            }
            else
            {
                //This adds the rigpoint at the specified position / rotation of the current view camera
                rig.GetComponent<CinematicTakesRig>().MoveAndAlign();
                rig.GetComponent<CinematicTakesRig>().MoveAndAlignCamera();
            }

            // Default name
            rig.transform.name = "Rig" + rig.transform.parent.childCount;
            rc.editCamera = true;
            // Updating rigs
            GetRigs();
#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = rig.gameObject;
#endif
        }
    }
}
