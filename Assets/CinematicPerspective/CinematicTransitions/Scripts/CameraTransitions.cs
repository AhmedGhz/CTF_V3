

using UnityEngine;
using System.Collections;

namespace CinematicTransitions
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraTransitions : MonoBehaviour
    {

        #region InspectorFields

        [Header("Settings")]
        private Material m_material;
        public Camera targetTransitionCamera;

        [Header("Transition Options")]
        [Range(0, 1.0f)]
        public float transition;
        [Range(0.1f, 1.0f)]
        public float transitionSoftness;

        [Tooltip("Texture for the transition")]
        public Texture2D mask;

        [Tooltip("Invert the mask?")]
        public bool invertMask;

        [Tooltip("Set the Render Queue of the transition")]
        public int renderQueue = 4000;

        [Header("Editor Options")]
        public bool ExecuteInEditMode = false;



        #endregion

        #region NonInspectorFields

        RenderTexture transitionCameraTexture;
        Camera m_camera;

        #endregion

        public Material material
        {
            get
            {
                return m_material = m_material ?? new Material(Shader.Find("Hidden/Masker"));
            }
        }

        public Shader shader
        {
            get
            {
                return material.shader;
            }
        }

        private bool canRun
        {
            get
            {
                return (Application.isPlaying || ExecuteInEditMode)
                && targetTransitionCamera != null && material != null;
            }
        }

        public Camera attachedCamera
        {
            get
            {
                return m_camera = m_camera ?? GetComponent<Camera>(); ;
            }
        }

        void Start()
        {
            if (Application.isPlaying || ExecuteInEditMode)
            {
                m_camera = GetComponent<Camera>();
                // Disable if we don't support image effects
                if (!SystemInfo.supportsImageEffects)
                {
                    enabled = false;
                    return;
                }

                //shader = Shader.Find("Hidden/ScreenTransitionImageEffect");

                // Disable the image effect if the shader can't
                // run on the users graphics card
                if (shader == null || !shader.isSupported)
                    enabled = false;
            }
        }

        void OnDisable()
        {

        }

        void SetMaterialValues(RenderTexture source)
        {


            material.SetTexture("_MainTex2", targetTransitionCamera.targetTexture);
            material.SetTexture("_MainTex", source);
            material.SetTexture("_MaskTex", mask);

            if(mask != null)
                material.SetFloat("_MaskValue", invertMask ? 1 - transition : transition);
            else
                material.SetFloat("_MaskValue", 1 - transition);

            material.SetFloat("_MaskSpread", transitionSoftness);
            material.renderQueue = renderQueue;

            if (material.IsKeywordEnabled("NO_MASK") != (mask == null))
            {
                if (mask == null)
                    material.EnableKeyword("NO_MASK");
                else
                    material.DisableKeyword("NO_MASK");
            }
            if (material.IsKeywordEnabled("INVERT_MASK") != invertMask)
            {
                if (invertMask)
                    material.EnableKeyword("INVERT_MASK");
                else
                    material.DisableKeyword("INVERT_MASK");
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (canRun)
            {
                targetTransitionCamera.targetTexture = transitionCameraTexture = transitionCameraTexture ?? new RenderTexture(source.width, source.height, 24);
                SetMaterialValues(source);
                Graphics.Blit(source, destination, material);
            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }

        private bool isTransitioning = false;

        public static void CheckTransitionComponent(Camera camera)
        {
            var component = camera.GetComponent<CameraTransitions>();
            if (component == null) camera.gameObject.AddComponent<CameraTransitions>();
        }

        public IEnumerator ProgramTransition(float duration, float softness, Texture2D mask, Camera targetCamera, bool invertMask = false, bool invertedTransition = false, int renderQueue = 4000)
        {

            //Saving previous state
            var oldSoftness = this.transitionSoftness;
            var oldMask = this.mask;
            var oldInvertMask = this.invertMask;
            var oldTransitionCamera = this.targetTransitionCamera;
            var oldrenderQueue = this.renderQueue;


            //Setting transition Values
            this.mask = mask;
            this.invertMask = invertMask;
            this.targetTransitionCamera = targetCamera;
            this.transitionSoftness = softness;
            var keepTransitioning = !isTransitioning && duration > 0;
            this.renderQueue = renderQueue;
            isTransitioning = keepTransitioning;

            //Transitioning
            float transitionStopTime = Time.time + duration;
            float timeKeeper = 0;
            float startTime = Time.time;
            bool firstRender = true;
            while (keepTransitioning)
            {
                timeKeeper = Time.time - startTime;

                var value = Mathf.Clamp01(timeKeeper / duration);

                transition = !invertedTransition ? value : 1 - value;

                keepTransitioning = Time.time < transitionStopTime;

                if (firstRender)
                {
                    GetComponent<Camera>().Render();
                    targetCamera.Render();
                    firstRender = false;
                }

                yield return new WaitForEndOfFrame();

            }
            //End transition

            //Restoring previous state
            this.transitionSoftness = oldSoftness;
            this.mask = oldMask;
            this.invertMask = oldInvertMask;
            this.targetTransitionCamera = oldTransitionCamera;
            this.renderQueue = oldrenderQueue;
            isTransitioning = false;

        }
    }
}


