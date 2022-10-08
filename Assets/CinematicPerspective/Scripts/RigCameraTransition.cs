
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CinematicPerspective
{
    [RequireComponent(typeof(CinematicTakesRig))]
    public class RigCameraTransition : MonoBehaviour
    {

        public bool DoTransition = true;

        public Texture2D transitionMask;

        public bool invertMask;

        public float transitionTime = .6f;
        [Range(0.1f, 1)]
        public float transitionSoftness = 0.2f;

        public int renderQueue = 4000;

        private CinematicTakesRig m_rig;

        private Camera transitionCamera;
        private Camera activeCamera;       

        public CinematicTakesRig rig
        {
            get
            {
                return m_rig = m_rig ?? GetComponent<CinematicTakesRig>();
            }
        }

        static bool transitioning;

        private void OnValidate()
        {
            transitionTime = transitionTime < 0 ? 0.0f : transitionTime;
        }

        void Update()
        {

            if(transitioning)
                TransitionCameraBehaviour();          
        }

        public void SetOptions(bool DoTransition = true, float transitionTime = .6f, float transitionSoftness = 0.2f, bool invertMask = false)
        {
            this.DoTransition = DoTransition;
            this.transitionTime = transitionTime;
            this.transitionSoftness = transitionSoftness;
            this.invertMask = invertMask;
        }

        void TransitionCameraBehaviour()
        {
            //The transition camera must be instanciated for this to work
            if (transitionCamera == null)
                return;

            //Checking the current rig mode, including defaut mode

            var lookAt = false;

            // Will be used when overriding the target
            var target = rig.cinematicTakes.target;


            switch (rig.selectedRigMode)
            {
                case CinematicTakes.DefaultCameraMode.LookAt:
                    transitionCamera.transform.position = rig.transform.position + rig.cameraPositionOffset;
                    lookAt = true;
                    break;
                case CinematicTakes.DefaultCameraMode.Fixed:
                    transitionCamera.transform.position = rig.transform.position + rig.cameraPositionOffset;
                    transitionCamera.transform.rotation = Quaternion.Euler(rig.cameraRotation);
                    lookAt = false;
                    break;
                case CinematicTakes.DefaultCameraMode.Dolly:
                    var dollyMode = rig.overridesMode ? rig.dollyMode : rig.cinematicTakes.dollyMode;
                    lookAt = dollyMode.lookAtTarget;
                    transitionCamera.transform.position = dollyMode.TravelPosition(rig.transform.position + rig.cameraPositionOffset);

                    //Setting rotation of Camera
                    if (dollyMode.lookAtTarget)
                        lookAt = true;
                    else
                        transitionCamera.transform.rotation =
                            rig.cameraRotation != Vector3.zero
                            ? Quaternion.Euler(rig.cameraRotation)
                            : rig.transform.rotation;
                    break;
                case CinematicTakes.DefaultCameraMode.Steadicam:
                    lookAt = false;                    
                    TransitionSteadyMode();
                    break;
                default:
                    break;
            }

            if (lookAt)
            {                
                transitionCamera.transform.LookAt(target);
            }
        }

        void TransitionSteadyMode()
        {
            //Selecting the mode, checks if selected rig is overriding 
            SteadyCameraMode mode;
            if (rig.rigCameraMode != CinematicTakesRig.OverrideCameraMode.Default)
                mode = rig.steadyMode;
            else
                mode = rig.cinematicTakes.steadyMode;
            //Cheking if steadyMode has target, if not using the global target
            var target = mode.followTarget != null ? mode.followTarget : rig.cinematicTakes.target;

            //Setting the FOV
            transitionCamera.fieldOfView = mode.steadyZoomLevel;

            //Setting the position
            transitionCamera.transform.position = target.position + rig.cameraPositionOffset;
            

            switch (mode.m_facing)
            {
                case SteadyCameraMode.facing.Target:
                    if (rig.cameraPositionOffset != Vector3.zero)
                    {
                        transitionCamera.transform.LookAt(target);
                    }
                    else
                    {
                        Debug.Log("With no camera offset facing target will change to facing the camera Direction");
                        mode.m_facing = SteadyCameraMode.facing.CameraDirection;
                    }

                    break;
                case SteadyCameraMode.facing.Front:
                    transitionCamera.transform.rotation = Quaternion.Euler(target.transform.forward);
                    break;
                case SteadyCameraMode.facing.CameraDirection:
                    transitionCamera.transform.rotation = Quaternion.Euler(rig.cameraRotation);
                    break;
            }

            if (mode.bobbing != 0)
            {
                float y = mode.bobbing * Mathf.PerlinNoise(Time.time * mode.shakiness, 0.0F);
                Vector3 pos = transitionCamera.transform.position;
                pos.y += y;
                pos.x += y * mode.shakiness * 0.5f;
                pos.z += y * mode.shakiness * 0.5f;
                transitionCamera.transform.position = pos;
            }
        }


        internal void StartTransition()
        {
            if (!DoTransition) return;
            
            activeCamera = (rig.cinematicTakes.selectedCinematicRig.overrideCamera != null ? rig.cinematicTakes.selectedCinematicRig.overrideCamera : rig.cinematicTakes.selectedCamera);
            CinematicTransitions.CameraTransitions.CheckTransitionComponent(activeCamera);
            
            //Cloning the targetCamera
            var camObj = Instantiate(rig.overrideCamera != null ? rig.overrideCamera : rig.cinematicTakes.selectedCamera);
            transitionCamera = camObj.GetComponent<Camera>();

            TransitionCameraBehaviour();

            Destroy(transitionCamera.GetComponent<CinematicTransitions.CameraTransitions>());

            transitionCamera.GetComponent<AudioListener>().enabled = false;
            

            StartCoroutine(Transition());
        }

        
        IEnumerator Transition()
        {
            transitioning = true;

            
            yield return StartCoroutine(
                        activeCamera.GetComponent<CinematicTransitions.CameraTransitions>()
                        .ProgramTransition(
                            transitionTime,
                            transitionSoftness,
                            transitionMask,
                            transitionCamera,
                            invertMask,
                            false,
                            renderQueue
                    ));
            Destroy(transitionCamera.gameObject);

            //Avoiding calling a destroyed camera
            transitionCamera = null;
           

            transitioning = false;

            
        }

    }
}


