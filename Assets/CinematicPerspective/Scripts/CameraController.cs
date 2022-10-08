using UnityEngine;
using UnityEngine.Serialization;
using System;

namespace CinematicPerspective
{
    /// <summary>
    /// Controls the behaviour of the camera
    /// </summary>
    public class CameraController0 : MonoBehaviour
    {

        /// <summary>
        /// Target camera to use
        /// </summary>
        public Camera selectedCamera
        {
            get
            {
                return m_selectedCamera;
            }
            internal set
            {
                SetCamera(value);
            }
        }

                

        /// <summary>
        /// How much should the accumulator grow every frame
        /// </summary>
        public static float s_accumulatorConstant = 0.01f;

        /// <summary>
        /// Minimum angle to start smooth Follow
        /// </summary>
        public static float s_minAngleSmoothFollow = 1f;


        [SerializeField, HideInInspector]
        private Camera m_selectedCamera;
        
        //d_ = Debug purposes only
        [SerializeField]
        private float d_fieldOfView;
        [SerializeField]
        private float d_targetDistance;
        [SerializeField]
        private float d_changeValue;

        /// <summary>
        /// Sets the target camera
        /// </summary>
        /// <param name="camera"></param>
        internal void SetCamera(Camera camera)
        {
            if (m_selectedCamera != null)
                m_selectedCamera.enabled = false;

            this.m_selectedCamera = camera;

            if (m_selectedCamera != null)
                m_selectedCamera.enabled = true;
        }

        /// <summary>
        /// Changes FOV of the camera relative to target position
        /// </summary>
        /// <param name="target">The transform of the object to zoom in/out</param>
        /// <param name="min">Clamps FOV to a minimum value</param>
        /// <param name="max">>Clamps FOV to a maximum value</param>
        /// <param name="maxAutoRange">Maximum distance to zoom</param>
        /// <param name="inverse">If true, zooms out when object is close, and zooms in when object is far</param>
        /// <param name="transitionSpeed">Sets how fast should the camera zoom</param>
        internal void AutoFov(Transform target, float min, float max, float maxAutoRange, bool inverse, float transitionSpeed)
        {
            d_targetDistance = (target.transform.position - selectedCamera.transform.position).magnitude;
            d_changeValue = d_targetDistance / maxAutoRange;


            d_changeValue = Mathf.Clamp01(d_changeValue);
            if (!inverse) d_changeValue = 1 - d_changeValue;
            d_fieldOfView = Mathf.Lerp(min, max, d_changeValue);
            selectedCamera.fieldOfView = Mathf.Lerp(selectedCamera.fieldOfView, d_fieldOfView, transitionSpeed * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Rotates the camera towards a target
        /// </summary>
        /// <param name="target">Transform of object to be followed</param>
        internal void RotateTowards(Transform target, float speed = 0)
        {
            var smooth = speed > 0;

            if (selectedCamera != null)
            {
                if (target != null)
                {
                    var cameraTransform = selectedCamera.transform;
                    if (smooth)
                    {
                        SmoothRotation(cameraTransform, target, speed);
                    }
                    else
                    {
                        SmoothRotation(cameraTransform, target, 100);
                    }
                        
                }
                else
                {
                    throw new ArgumentNullException("target", "Can't rotate towards a null target");
                }
            }
            else
            {
                Debug.LogWarning("Cinematic Perspective: A camera is need to use Cinematic Perspective");
            }
        }

        /// <summary>
        ///  Smoothly rotate towards the target transform.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="target"></param>
        /// <param name="speed"></param>
        private void SmoothRotation(Transform subject, Transform target, float speed)
        {
            var dif = target.position - subject.position;
            var targetRotation = Quaternion.LookRotation(dif);
            
            //if(speed == 100)
            subject.rotation = targetRotation; 
            //else
            //    //subject.rotation = Quaternion.RotateTowards(subject.rotation, targetRotation, 360 * Time.deltaTime);
            //    subject.rotation = Quaternion.Slerp(subject.rotation, targetRotation, speed *  Time.deltaTime);
                //subject.rotation = Quaternion.SlerpUnclamped(subject.rotation, targetRotation, speed * Time.deltaTime);
        }

        /// <summary>
        /// Moves the camera to a world space position
        /// </summary>
        /// <param name="position">Position for the camera to move</param>
        internal void MoveTo(Vector3 position)
        {
            if (selectedCamera != null)
                selectedCamera.transform.position = position;
        }

        /// <summary>
        /// Changes FOV of the selected Camera
        /// </summary>
        /// <param name="value">New value for the FOV</param>
        internal void SetFov(float value)
        {
            if (selectedCamera != null)
                selectedCamera.fieldOfView = value;
        }
    }
}
