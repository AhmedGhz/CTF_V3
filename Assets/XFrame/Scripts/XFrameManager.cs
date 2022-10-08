using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Experimental.Rendering;

namespace XFrameEffect {
    public enum XFRAME_DOWNSAMPLING_METHOD {
        Disabled = 0,
        AdaptativeDownsampling = 1,
        HorizontalDownsampling = 2,
        QuadDownsampling = 3
    }

    public enum XFRAME_COMPOSITING_METHOD {
        SecondCameraBillboardWorldSpace = 1,
        SecondCameraBlit = 3,
        LightweightRenderingPipeline = 4
    }

    public enum XFRAME_FILTERING_MODE {
        Bilinear = 0,
        NearestNeighbour = 1
    }

    public enum XFRAME_FPS_LOCATION {
        TopLeftCorner = 0,
        TopRightCorner = 1,
        BottomLeftCorner = 2,
        BottomRightCorner = 3
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Rendering/X-Frame FPS Accelerator", 90)]
    public class XFrameManager : MonoBehaviour {

        const string XFRAME_CAMERA_OLD = "XFrameCamera";
        const string XFRAME_BILLBOARD_OLD = "XFrameBillboard";
        const string XFRAME_CAMERA_INSTANCED = "XFrameCameraInstanced";
        const string XFRAME_BILLBOARD_INSTANCED = "XFrameBillboardInstanced";
        const int XFRAME_BILLBOARD_LAYER = 29;
        public Action repaintInspectorAction;
        [SerializeField]
        XFRAME_DOWNSAMPLING_METHOD
                        _method = XFRAME_DOWNSAMPLING_METHOD.AdaptativeDownsampling;

        public XFRAME_DOWNSAMPLING_METHOD method {
            get { return _method; }
            set {
                if (_method != value) {
                    _method = value;
                    ClearRTArray();
                    UpdateSettings();
                    switch (_method) {
                        case XFRAME_DOWNSAMPLING_METHOD.AdaptativeDownsampling:
                            _downsampling = 0.25f;
                            _staticDownsampling = 0.25f;
                            break;
                        case XFRAME_DOWNSAMPLING_METHOD.HorizontalDownsampling:
                            _downsampling = 0.5f;
                            _staticDownsampling = 0.5f;
                            break;
                        case XFRAME_DOWNSAMPLING_METHOD.QuadDownsampling:
                            _downsampling = 0.5f;
                            _staticDownsampling = 0.5f;
                            break;
                        case XFRAME_DOWNSAMPLING_METHOD.Disabled:
                            CheckCamera();
                            break;
                    }
                    rtDownsampling = 0f;
                    CheckRT();
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        XFRAME_FILTERING_MODE
                        _filtering = XFRAME_FILTERING_MODE.Bilinear;

        public XFRAME_FILTERING_MODE filtering {
            get { return _filtering; }
            set {
                if (_filtering != value) {
                    _filtering = value;
                    ClearRTArray();
                    UpdateSettings();
                    rtDownsampling = 0f;
                    CheckRT();
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        int
                        _targetFPS = 30;

        public int targetFPS {
            get { return _targetFPS; }
            set {
                if (_targetFPS != value) {
                    _targetFPS = Mathf.Clamp(value, 10, 120);
                    if (_targetFPS > _niceFPS)
                        _niceFPS = _targetFPS;
                    UpdateSettings();
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        CameraClearFlags _cameraClearFlags = CameraClearFlags.SolidColor;


        public CameraClearFlags cameraClearFlags {
            get { return _cameraClearFlags; }
            set {
                if (_cameraClearFlags != value) {
                    _cameraClearFlags = value;
                    if (xFrameCamera != null)
                        xFrameCamera.clearFlags = _cameraClearFlags;
                    isDirty = true;
                }
            }
        }


        [SerializeField]
        bool
                        _niceFPSEnabled;

        public bool niceFPSEnabled {
            get { return _niceFPSEnabled; }
            set {
                if (_niceFPSEnabled != value) {
                    _niceFPSEnabled = value;
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        bool
                        _prewarm;

        public bool prewarm {
            get { return _prewarm; }
            set {
                if (_prewarm != value) {
                    _prewarm = value;
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        int
                        _niceFPS = 55;

        public int niceFPS {
            get { return _niceFPS; }
            set {
                if (_niceFPS != value) {
                    if (_method == XFRAME_DOWNSAMPLING_METHOD.AdaptativeDownsampling) {
                        _niceFPS = Mathf.Clamp(value, _targetFPS, 120);
                    } else {
                        _niceFPS = Mathf.Clamp(value, 30, 120);
                    }
                    UpdateSettings();
                    isDirty = true;
                }
            }
        }

        public bool niceFPSisActive { get { return avgFPSNice >= _niceFPS && _niceFPSEnabled; } }

        [SerializeField]
        float
                        _downsampling = 0.25f;

        public float downsampling {
            get { return _downsampling; }
            set {
                if (_downsampling != value) {
                    _downsampling = rtClamp(value);
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        float
                        _staticDownsampling = 0.3f;

        public float staticCameraDownsampling {
            get { return _staticDownsampling; }
            set {
                if (_staticDownsampling != value) {
                    _staticDownsampling = rtClamp(value);
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        float
                        _fpsChangeSpeedUp = 0.02f;

        public float fpsChangeSpeedUp {
            get { return _fpsChangeSpeedUp; }
            set {
                if (_fpsChangeSpeedUp != value) {
                    _fpsChangeSpeedUp = Mathf.Clamp(value, 0.01f, 0.1f);
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        float
                        _fpsChangeSpeedDown = 0.02f;

        public float fpsChangeSpeedDown {
            get { return _fpsChangeSpeedDown; }
            set {
                if (_fpsChangeSpeedDown != value) {
                    _fpsChangeSpeedDown = Mathf.Clamp(value, 0.01f, 0.1f);
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        bool
                        _sharpen;

        public bool sharpen {
            get { return _sharpen; }
            set {
                if (_sharpen != value) {
                    _sharpen = value;
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        int
                        _antialias = 1;

        public int antialias {
            get { return _antialias; }
            set {
                if (_antialias != value) {
                    _antialias = value;
                    if (mainCamera != null && mainCamera.targetTexture != null && mainCamera.targetTexture.antiAliasing != getAntialiasLevel()) {
                        ClearRTArray();
                        CheckRT();
                    }
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        bool
                        _reducePixelLights = true;

        public bool reducePixelLights {
            get { return _reducePixelLights; }
            set {
                if (_reducePixelLights != value) {
                    _reducePixelLights = value;
                    if (!_reducePixelLights && oldPixelLightCount > 0) {
                        QualitySettings.pixelLightCount = oldPixelLightCount;
                    }
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        bool
                        _manageShadows = true;

        public bool manageShadows {
            get { return _manageShadows; }
            set {
                if (_manageShadows != value) {
                    _manageShadows = value;
                    if (!_manageShadows)
                        ResetShadows();
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        XFRAME_COMPOSITING_METHOD
                        _compositingMethod = XFRAME_COMPOSITING_METHOD.SecondCameraBillboardWorldSpace;

        public XFRAME_COMPOSITING_METHOD compositingMethod {
            get { return _compositingMethod; }
            set {
                if (_compositingMethod != value) {
                    _compositingMethod = value;
                    CheckPipeline();
                    DestroyXFrameCamera();
                    CheckCamera();
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        bool _showFPS;

        public bool showFPS {
            get {
                return _showFPS;
            }
            set {
                if (_showFPS != value) {
                    _showFPS = value;
                    CheckStatsLayer();
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        int _fpsFontSize = 24;

        public int fpsFontSize {
            get {
                return _fpsFontSize;
            }
            set {
                if (_fpsFontSize != value) {
                    _fpsFontSize = value;
                    isDirty = true;
                }
            }
        }

        [SerializeField]
        Color _fpsColor = Color.white;

        public Color fpsColor {
            get {
                return _fpsColor;
            }
            set {
                if (_fpsColor != value) {
                    _fpsColor = value;
                    isDirty = true;
                }
            }
        }



        [SerializeField]
        XFRAME_FPS_LOCATION _fpsLocation = XFRAME_FPS_LOCATION.TopLeftCorner;

        public XFRAME_FPS_LOCATION fpsLocation {
            get { return _fpsLocation; }
            set {
                if (_fpsLocation != value) {
                    _fpsLocation = value;
                    isDirty = true;
                }
            }
        }


        public bool enableClickEvents;


        [SerializeField]
        bool
                        _boostFrameRate = true;

        public bool boostFrameRate {
            get { return _boostFrameRate; }
            set {
                if (_boostFrameRate != value) {
                    _boostFrameRate = value;
                    isDirty = true;
                }
            }
        }


        [NonSerialized]
        public GameObject
                        xFrameCameraObj;
        [NonSerialized]
        public GameObject
                        xFrameBillboardObj;
        [NonSerialized]
        public bool
                        isDirty;
        static XFrameManager _xFrame;

        public static XFrameManager instance {
            get {
                if (_xFrame == null) {
                    foreach (Camera camera in Camera.allCameras) {
                        _xFrame = camera.GetComponent<XFrameManager>();
                        if (_xFrame != null)
                            break;
                    }
                }
                return _xFrame;
            }
        }

        public int currentFPS {
            get { return fps; }
        }

        public float appliedDownsampling {
            get { return rtDownsampling; }
        }

        public RenderTexture rt {
            get {
                if (mainCamera == null) {
                    return null;
                }
                return mainCamera.targetTexture;
            }
        }

        public float activeDownsampling {
            get {
                return cameraIsStatic ? _staticDownsampling : _downsampling;
            }
        }

        [NonSerialized]
        public Camera mainCamera;
        static int cameraNumber;

        /* Internal fields */

        RenderTexture[] rtArray;
        int frameCount;
        float nextPeriod;
        int fps;
        const float FPS_UPDATE_RATE = 0.5f;
        Camera xFrameCamera;
        PostFrameCompositor xFramePost;
        float avgDownsampling, rtDownsampling;
        int oldVSyncCount;
        bool cameraIsStatic;
        Vector3 oldCameraPos, oldCameraRot;
        int oldPixelLightCount;
        List<Light> lights;
        float lastLightCheckTime;
        Dictionary<Light, LightShadows> oldLightShadows;
        int _screenWidth, _screenHeight;
        Material xFrameBillboardMat;
        float lastNiceTimeCheck;
        float avgFPSNice;
        float lastInspectorRefresh;
        int camMovDetectThreshold;
        PropertyInfo renderScale;
        RenderPipelineAsset pipelineAsset;
        Canvas[] canvases;
        List<RaycastResult> hitResults;
        bool niceMode;

        int screenWidth {
            get {
#if UNITY_EDITOR
                return _screenWidth;
#else
				return (int)(Screen.width * mainCamera.rect.width);
#endif
            }
        }

        int screenHeight {
            get {
#if UNITY_EDITOR
                return _screenHeight;
#else
				return (int)(Screen.height * mainCamera.rect.height);
#endif
            }
        }



        #region Game loop events

        // Creates a private material used to the effect
        void OnEnable() {
            Init();
        }


        void Start() {
            nextPeriod = Time.realtimeSinceStartup + FPS_UPDATE_RATE;
            oldPixelLightCount = QualitySettings.pixelLightCount;
        }

        void Reset() {
            Init();
        }

        void Init() {
            mainCamera = GetComponent<Camera>();

#if UNITY_EDITOR
            _screenWidth = (int)(Screen.width * mainCamera.rect.width);
            _screenHeight = (int)(Screen.height * mainCamera.rect.height);
#endif
            oldLightShadows = new Dictionary<Light, LightShadows>();
            oldVSyncCount = QualitySettings.vSyncCount;
            fps = _targetFPS;
            avgDownsampling = 1f;
            ClearRTArray();
            ClearOldInstancedCamera();
            CheckCamera();
            CheckPipeline();
            UpdateSettings();
            if (_prewarm)
                PrewarmRTs();
            CheckRT();
            UpdateUICanvases();
            CheckStatsLayer();
        }

        void CheckStatsLayer() {
            if (mainCamera == null)
                return;
            XFrameFPSInfo stats = mainCamera.GetComponent<XFrameFPSInfo>();
            if (stats != null && !_showFPS) {
                DestroyImmediate(stats);
                return;
            }
            if (stats == null && _showFPS) {
                mainCamera.gameObject.AddComponent<XFrameFPSInfo>();
            }
        }


        void CheckPipeline() {
            pipelineAsset = GraphicsSettings.renderPipelineAsset;
            renderScale = null;
            if (pipelineAsset != null) {
                renderScale = pipelineAsset.GetType().GetProperty("RenderScale");
                if (renderScale == null) {
                    renderScale = pipelineAsset.GetType().GetProperty("renderScale");
                }
                if (renderScale != null) {
                    _compositingMethod = XFRAME_COMPOSITING_METHOD.LightweightRenderingPipeline;
                    if (mainCamera != null) {
                        mainCamera.targetTexture = null;
                        ResetCameraAspect();
                    }
                    return;
                }
            }
            if (_compositingMethod == XFRAME_COMPOSITING_METHOD.LightweightRenderingPipeline) {
                Debug.LogWarning("Lightweight Rendering Pipeline is not enabled in Graphics Settings. Reverting to Second Camera Billboard mode.");
                _compositingMethod = XFRAME_COMPOSITING_METHOD.SecondCameraBillboardWorldSpace;
            }
        }

        void ClearOldInstancedCamera() {
            for (int k = 0; k < 50; k++) {
                GameObject go = GameObject.Find(XFRAME_CAMERA_OLD);
                if (go != null)
                    DestroyImmediate(go);
            }
            for (int k = 0; k < 50; k++) {
                GameObject go = GameObject.Find(XFRAME_BILLBOARD_OLD);
                if (go != null)
                    DestroyImmediate(go);
            }
        }

        void PrewarmRTs() {
            if (_compositingMethod == XFRAME_COMPOSITING_METHOD.LightweightRenderingPipeline)
                return;
            if (_method == XFRAME_DOWNSAMPLING_METHOD.AdaptativeDownsampling) {
                for (int k = 1; k <= 10; k++) {
                    avgDownsampling = k / 10f;
                    CheckRT();
                }
            } else {
                cameraIsStatic = false;
                CheckRT();
                cameraIsStatic = true;
                CheckRT();
            }
        }

        void ClearRTArray() {
            if (rtArray != null) {
                if (mainCamera != null) {
                    mainCamera.targetTexture = null;
                    ResetCameraAspect();
                }
                for (int k = 0; k < rtArray.Length; k++) {
                    CheckAndReleaseRT(rtArray[k]);
                    rtArray[k] = null;
                }
            } else {
                rtArray = new RenderTexture[10];
            }
        }

        void OnDisable() {
            if (mainCamera != null) {
                mainCamera.targetTexture = null;
                ResetCameraAspect();
            }
            if (xFrameCamera != null) {
                xFrameCamera.enabled = false;
            }
            if (xFrameBillboardObj != null) {
                xFrameBillboardObj.SetActive(false);
            }
            ResetShadows();
            RestoreUICanvases();
            ClearRTArray();
        }

        void OnDestroy() {
            DestroyXFrameCamera();
            ClearRTArray();
        }

        void DestroyXFrameCamera() {
            if (xFrameCameraObj != null) {
                DestroyImmediate(xFrameCameraObj);
                xFrameCameraObj = null;
            }
            if (xFrameBillboardObj != null) {
                DestroyImmediate(xFrameBillboardObj);
                xFrameBillboardObj = null;
            }
            if (xFrameBillboardMat != null) {
                DestroyImmediate(xFrameBillboardMat);
                xFrameBillboardMat = null;
            }
            RestoreUICanvases();
        }


        void LateUpdate() {
#if UNITY_EDITOR
            _screenWidth = (int)(Screen.width * mainCamera.rect.width);
            _screenHeight = (int)(Screen.height * mainCamera.rect.height);

            if (repaintInspectorAction != null && Time.time - lastInspectorRefresh > 3f) {
                lastInspectorRefresh = Time.time;
                repaintInspectorAction();
            }
#endif

            if (enableClickEvents) DoRaycasting();
            if (_compositingMethod != XFRAME_COMPOSITING_METHOD.LightweightRenderingPipeline) {
                if (_niceFPSEnabled && Application.isPlaying) {
                    float niceTimeInterval = mainCamera.targetTexture != null ? 1f : 5f;
                    if (Time.time - lastNiceTimeCheck > niceTimeInterval) {
                        avgFPSNice = (avgFPSNice + fps) * 0.5f;
                        lastNiceTimeCheck = Time.time;
                        niceMode = avgFPSNice >= _niceFPS;
                        if (niceMode) {
                            if (mainCamera.targetTexture != null) {
                                // Clear XFrame elements
                                mainCamera.targetTexture = null;
                                ResetCameraAspect();
                                if (xFrameCamera != null) {
                                    xFrameCamera.enabled = false;
                                }
                                if (xFrameBillboardObj != null) {
                                    xFrameBillboardObj.SetActive(false);
                                }
                                RestoreUICanvases();
                                rtDownsampling = 1;
                            }
                            frameCount += _targetFPS;
                        }
                    }
                } else {
                    niceMode = false;
                }

                if (!niceMode) {
                    if (_method != XFRAME_DOWNSAMPLING_METHOD.Disabled && (xFrameCameraObj == null || (_compositingMethod != XFRAME_COMPOSITING_METHOD.SecondCameraBlit && xFrameBillboardObj == null))) {
                        CheckCamera();
                    }
                    if (xFrameCamera != null && !xFrameCamera.enabled) {
                        if (xFramePost != null) {
                            xFramePost.xFrameManager = this;
                        }
                        xFrameCamera.enabled = true;
                    }
                    if (xFrameBillboardObj != null && !xFrameBillboardObj.activeSelf) {
                        xFrameBillboardObj.SetActive(true);
                    }
                }
            }
            if (!Application.isPlaying) {
                rtDownsampling = 0;
                avgDownsampling = 1f;
                cameraIsStatic = true;
                CheckRT();
                return;
            }
            // Camera moved?
            if (camMovDetectThreshold++ > 5) {
                Vector3 camRot = mainCamera.transform.rotation.eulerAngles;
                if (mainCamera.transform.position != oldCameraPos || camRot != oldCameraRot) {
                    cameraIsStatic = false;
                    oldCameraPos = mainCamera.transform.position;
                    oldCameraRot = camRot;
                    camMovDetectThreshold = 0;
                } else {
                    cameraIsStatic = true;
                }
            }

            // Compute fps
            frameCount++;
            if (Time.realtimeSinceStartup > nextPeriod) {
                fps = (int)(frameCount / FPS_UPDATE_RATE);
                frameCount = 0;
                nextPeriod += FPS_UPDATE_RATE;
                if (_method == XFRAME_DOWNSAMPLING_METHOD.Disabled) {
                    avgDownsampling = 1f;
                } else {
                    if (fps >= _targetFPS) {
                        avgDownsampling += Mathf.Min(_fpsChangeSpeedUp * (fps - _targetFPS), _fpsChangeSpeedUp);
                    } else {
                        avgDownsampling -= Mathf.Min(_fpsChangeSpeedDown * (_targetFPS - fps), _fpsChangeSpeedDown);
                    }
                    avgDownsampling = Mathf.Clamp(avgDownsampling, activeDownsampling, 1f);
                }

                // Additional adjustments
                if (_reducePixelLights) {
                    int newPixelLightCount = avgDownsampling < 1f ? 1 : oldPixelLightCount;
                    if (QualitySettings.pixelLightCount != newPixelLightCount) {
                        QualitySettings.pixelLightCount = newPixelLightCount;
                    }
                }
                if (_manageShadows) {
                    ManageShadows();
                }
                CheckRT();
            }

        }

        void OnPreRender() {
            if (_compositingMethod == XFRAME_COMPOSITING_METHOD.LightweightRenderingPipeline)
                return;

            // Check nice FPS
            if (niceMode) return;

            if (Input.GetMouseButtonDown(0)) {
                UpdateUICanvases();
            }

            if (xFrameCamera != null) {
                if (xFrameCamera.enabled && _method == XFRAME_DOWNSAMPLING_METHOD.Disabled) {
                    xFrameCamera.enabled = false;
                } else if (!xFrameCamera.enabled && method != XFRAME_DOWNSAMPLING_METHOD.Disabled) {
                    xFrameCamera.enabled = true;
                }
            }

            if (mainCamera.targetTexture == null && _method != XFRAME_DOWNSAMPLING_METHOD.Disabled) {
                CheckRT();
            }

            if (mainCamera.targetTexture != null)
                mainCamera.targetTexture.DiscardContents();

            switch (_compositingMethod) {
                case XFRAME_COMPOSITING_METHOD.SecondCameraBillboardWorldSpace:
                    if (xFrameBillboardMat != null && xFrameBillboardMat.mainTexture != rt) {
                        xFrameBillboardMat.mainTexture = rt;
                    }
                    break;
            }

        }

        //								void OnGUI () {
        //													GUI.Label (new Rect (10, 10, 300, 20), "X-FRAME DEMO");
        //								}



        #endregion

        #region Camera setup stuff

        void UpdateSettings() {

            switch (_method) {
                case XFRAME_DOWNSAMPLING_METHOD.AdaptativeDownsampling:
                    if (_boostFrameRate) {
                        Application.targetFrameRate = 300;
                        QualitySettings.vSyncCount = 0;
                    }
                    break;
                case XFRAME_DOWNSAMPLING_METHOD.HorizontalDownsampling:
                    if (_boostFrameRate) {
                        Application.targetFrameRate = 300;
                        QualitySettings.vSyncCount = 0;
                    }
                    break;
                case XFRAME_DOWNSAMPLING_METHOD.QuadDownsampling:
                    if (_boostFrameRate) {
                        Application.targetFrameRate = 300;
                        QualitySettings.vSyncCount = 0;
                    }
                    break;
                case XFRAME_DOWNSAMPLING_METHOD.Disabled:
                    Application.targetFrameRate = -1;
                    QualitySettings.vSyncCount = oldVSyncCount;
                    break;
            }
        }

        void CheckCamera() {

            if (xFrameCameraObj == null) {
                xFrameCameraObj = FindGameObject(XFRAME_CAMERA_INSTANCED);
                xFrameCamera = null;
            }
            if (xFrameBillboardObj == null) {
                xFrameBillboardObj = FindGameObject(XFRAME_BILLBOARD_INSTANCED);
            }

            if ((_method == XFRAME_DOWNSAMPLING_METHOD.Disabled || _compositingMethod == XFRAME_COMPOSITING_METHOD.LightweightRenderingPipeline) && xFrameCameraObj != null) {
                DestroyXFrameCamera();
            }

            if (_method == XFRAME_DOWNSAMPLING_METHOD.Disabled || _compositingMethod == XFRAME_COMPOSITING_METHOD.LightweightRenderingPipeline)
                return;

            if (xFrameCameraObj == null) {
                xFrameCameraObj = new GameObject(XFRAME_CAMERA_INSTANCED);
                xFrameCamera = null;
            }
            if (xFrameCamera == null) {
                xFrameCamera = xFrameCameraObj.GetComponent<Camera>();
                if (xFrameCamera == null)
                    xFrameCamera = xFrameCameraObj.AddComponent<Camera>();
            }
            if (xFrameCameraObj.GetComponent<FlareLayer>() == null)
                xFrameCameraObj.AddComponent<FlareLayer>();
            switch (_compositingMethod) {
                case XFRAME_COMPOSITING_METHOD.SecondCameraBlit:
                    SetupSecondCameraBlitMode();
                    break;
                case XFRAME_COMPOSITING_METHOD.SecondCameraBillboardWorldSpace:
                    SetupSecondCameraBillboardWorldSpaceMode();
                    break;
            }
            isDirty = true;
        }

        GameObject FindGameObject(string name) {
            // Try to find it as a child of camera
            Transform[] children = transform.GetComponentsInChildren<Transform>(true);
            for (int k = 0; k < children.Length; k++) {
                if (children[k].name.Equals(name)) {
                    return children[k].gameObject;
                }
            }

            // Find it elsewhere
            GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int k = 0; k < gos.Length; k++) {
                GameObject go = gos[k];
                if (go != null && go.name.Equals(name)) {
                    // Ensure this is not parented to other camera
                    Transform parent = go.transform;
                    bool otherCamera = false;
                    while (parent.parent != null) {
                        parent = parent.parent;
                        Camera cam = parent.GetComponent<Camera>();
                        if (cam != null && cam != mainCamera) {
                            otherCamera = true;
                            break;
                        }
                    }
                    if (!otherCamera) {
                        return go;
                    }
                }
            }
            return null;
        }

        void SetupSecondCameraBlitMode() {
            if (mainCamera != null) {
                xFrameCamera.CopyFrom(mainCamera);
                xFrameCamera.depth++;
            }
            xFrameCamera.renderingPath = RenderingPath.Forward;
            xFrameCamera.transform.position = GetFarCameraPos();
            xFrameCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
            xFrameCamera.transform.SetParent(mainCamera.transform, true);
            xFrameCamera.targetTexture = null;
            xFrameCamera.clearFlags = _cameraClearFlags;
            xFrameCamera.depthTextureMode = DepthTextureMode.None;
            xFrameCamera.nearClipPlane = 0.3f;
            xFrameCamera.farClipPlane = 1f;
            xFrameCamera.cullingMask = 0;
            xFrameCamera.allowMSAA = _antialias > 1;
            xFrameCamera.allowHDR = false;
            xFrameCamera.useOcclusionCulling = false;
            xFramePost = xFrameCameraObj.GetComponent<PostFrameCompositor>() ?? xFrameCameraObj.gameObject.AddComponent<PostFrameCompositor>();
            xFramePost.xFrameManager = this;
        }

        void SetupSecondCameraBillboardWorldSpaceMode() {
            if (mainCamera != null) {
                xFrameCamera.CopyFrom(mainCamera);
                xFrameCamera.depth++;
            }
            xFrameCamera.renderingPath = RenderingPath.Forward;
            xFrameCamera.transform.position = GetFarCameraPos();
            xFrameCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
            xFrameCamera.transform.SetParent(mainCamera.transform, true);
            xFrameCamera.targetTexture = null;
            xFrameCamera.clearFlags = _cameraClearFlags;
            xFrameCamera.depthTextureMode = DepthTextureMode.None;
            xFrameCamera.nearClipPlane = 0.3f;
            xFrameCamera.farClipPlane = 1f;
            xFrameCamera.cullingMask = 1 << XFRAME_BILLBOARD_LAYER;
            xFrameCamera.useOcclusionCulling = false;
            xFrameCamera.orthographic = true;
            xFrameCamera.orthographicSize = 0.5f;
            xFrameCamera.allowMSAA = _antialias > 1;
            xFramePost = xFrameCameraObj.GetComponent<PostFrameCompositor>();
            if (xFramePost != null) {
                DestroyImmediate(xFramePost);
                xFramePost = null;
            }
            if (xFrameBillboardObj == null) {
                xFrameBillboardObj = Instantiate(Resources.Load<GameObject>("Prefabs/XFrameBillboard"));
                xFrameBillboardObj.hideFlags = HideFlags.DontSave;
                xFrameBillboardObj.name = XFRAME_BILLBOARD_INSTANCED;
                xFrameBillboardObj.layer = XFRAME_BILLBOARD_LAYER;
                Renderer billboardRenderer = xFrameBillboardObj.GetComponent<Renderer>();
                xFrameBillboardMat = new Material(Shader.Find("XFrame/XFrame Unlit"));
                billboardRenderer.sharedMaterial = xFrameBillboardMat;
            } else {
                Renderer billboardRenderer = xFrameBillboardObj.GetComponent<Renderer>();
                xFrameBillboardMat = billboardRenderer.sharedMaterial;
            }
            xFrameBillboardObj.transform.SetParent(xFrameCamera.transform);
            xFrameBillboardObj.transform.rotation = Quaternion.Euler(0, 0, 0);
            xFrameBillboardObj.transform.localPosition = new Vector3(0, 0, 0.5f);
            xFrameBillboardObj.transform.localScale = new Vector3(xFrameCamera.aspect, 1f, 1f);


        }

        Vector3 GetFarCameraPos() {
            cameraNumber++;
            if (cameraNumber > 10) {
                cameraNumber = -10;
            }
            return new Vector3(0, cameraNumber * 100f, 1000f);
        }

        #endregion

        #region Downsampling stuff

        float rtClamp(float d) {
            return Mathf.Clamp(d, 0.1f, 1f);
        }

        int getAntialiasLevel() {
            return (int)(Mathf.Pow(2, _antialias - 1));
        }

        RenderTexture FetchRT(int width, int height) {
            RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            rt.isPowerOfTwo = false;
            rt.hideFlags = HideFlags.DontSave;
            rt.filterMode = _filtering == XFRAME_FILTERING_MODE.Bilinear ? FilterMode.Bilinear : FilterMode.Point;
            rt.wrapMode = TextureWrapMode.Clamp;
            if (_antialias > 1)
                rt.antiAliasing = getAntialiasLevel();
            rt.Create();
            return rt;
        }

        RenderTexture PrepareAdaptativeRenderTexture() {
            int index = Mathf.RoundToInt(avgDownsampling * 10f);
            if (rtArray == null || rtArray.Length < index)
                return null;
            RenderTexture rt = rtArray[index - 1];
            float f = index * 0.1f;
            int width = (int)(screenWidth * f);
            int height = (int)(screenHeight * f);
            if (width <= 0 || (rt != null && rt.width == width && rt.height == height && rt.IsCreated()))
                return rt;
            CheckAndReleaseRT(rt);
            rt = FetchRT(width, height);
            rtArray[index - 1] = rt;
            return rt;
        }

        RenderTexture PrepareVerticalRenderTexture() {
            if (rtArray == null || rtArray.Length == 0)
                return null;
            int index = cameraIsStatic ? 0 : 1;
            RenderTexture rt = rtArray[index];
            int width = (int)(screenWidth * activeDownsampling);
            int height = screenHeight;
            if (width <= 0 || (rt != null && rt.width == width && rt.height == height && rt.IsCreated()))
                return rt;
            CheckAndReleaseRT(rt);
            rt = FetchRT(width, height);
            rtArray[index] = rt;
            return rt;
        }

        RenderTexture PrepareQuadRenderTexture() {
            if (rtArray == null || rtArray.Length == 0)
                return null;
            int index = cameraIsStatic ? 0 : 1;
            RenderTexture rt = rtArray[index];
            int width = (int)(screenWidth * activeDownsampling);
            int height = (int)(screenHeight * activeDownsampling);
            if (width <= 0 || (rt != null && rt.width == width && rt.height == height && rt.IsCreated()))
                return rt;
            CheckAndReleaseRT(rt);
            rt = FetchRT(width, height);
            rtArray[index] = rt;
            return rt;
        }

        void CheckAndReleaseRT(RenderTexture rt) {
            if (rt == null)
                return;
            if (mainCamera.targetTexture == rt) {
                RenderTexture.active = null;
                mainCamera.targetTexture = null;
                ResetCameraAspect();
            }
            rt.Release();
        }

        void CheckRT() {

            if (mainCamera == null)
                return;

            if (_compositingMethod == XFRAME_COMPOSITING_METHOD.LightweightRenderingPipeline) {
                _method = XFRAME_DOWNSAMPLING_METHOD.AdaptativeDownsampling;
                if (renderScale != null) {
                    if (Mathf.Abs(avgDownsampling - rtDownsampling) < 0.05f)
                        return;
                    rtDownsampling = avgDownsampling;
                    renderScale.SetValue(pipelineAsset, rtDownsampling, null);
                }
                return;
            }

            if (niceMode)
                return;

            RenderTexture rt = null;
            switch (_method) {
                case XFRAME_DOWNSAMPLING_METHOD.AdaptativeDownsampling:
                    if (mainCamera.targetTexture != null && Mathf.Abs(avgDownsampling - rtDownsampling) < 0.05f)
                        return;
                    rtDownsampling = avgDownsampling;
                    ResetCameraAspect();
                    rt = PrepareAdaptativeRenderTexture();
                    break;
                case XFRAME_DOWNSAMPLING_METHOD.HorizontalDownsampling:
                    rt = PrepareVerticalRenderTexture();
                    if (rt != null) {
                        rtDownsampling = activeDownsampling;
                        float aspectRatio = (float)rt.width / (rt.height * rtDownsampling);
                        if (aspectRatio != mainCamera.aspect) {
                            mainCamera.aspect = aspectRatio;
                        }
                    }
                    break;
                case XFRAME_DOWNSAMPLING_METHOD.QuadDownsampling:
                    rt = PrepareQuadRenderTexture();
                    if (rt != null) {
                        rtDownsampling = activeDownsampling;
                        float aspectRatio = (float)rt.width / rt.height;
                        if (aspectRatio != mainCamera.aspect) {
                            mainCamera.aspect = aspectRatio;
                        }
                    }
                    break;
            }
            if (mainCamera.targetTexture != rt) {
                mainCamera.targetTexture = rt;
                UpdateUICanvases();
            }
        }

        void ResetCameraAspect() {
            float aspect = (float)screenWidth / screenHeight;
            mainCamera.aspect = aspect;
        }

        public void UpdateUICanvases() {
            // Make sure UI Canvas is not Screen Overlay or it won't work
            canvases = FindObjectsOfType<Canvas>();
            for (int k = 0; k < canvases.Length; k++) {
                Canvas canvas = canvases[k];
                if (canvas.renderMode == RenderMode.WorldSpace || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == mainCamera)) {
                    canvas.scaleFactor = rtDownsampling;
                }
                if (canvas.worldCamera == mainCamera || canvas.worldCamera == null || canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
                    GraphicRaycasterProxy proxy = canvas.GetComponent<GraphicRaycasterProxy>();
                    if (proxy == null) {
                        proxy = canvas.gameObject.AddComponent<GraphicRaycasterProxy>();
                    }
                    proxy.xframe = this;
                }
            }
        }

        public void RestoreUICanvases() {
            canvases = FindObjectsOfType<Canvas>();
            for (int k = 0; k < canvases.Length; k++) {
                Canvas canvas = canvases[k];
                if (canvas.renderMode == RenderMode.WorldSpace || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == mainCamera)) {
                    canvas.scaleFactor = 1f;
                }
                GraphicRaycasterProxy grp = canvas.GetComponent<GraphicRaycasterProxy>();
                if (grp != null) {
                    DestroyImmediate(grp);
                }
            }
        }

        #endregion

        #region Quality settings stuff

        void ResetShadows() {
            if (lights == null)
                return;
            int lightsCount = lights.Count;
            for (int k = 0; k < lightsCount; k++) {
                Light light = lights[k];
                if (light == null)
                    continue;
                if (oldLightShadows.ContainsKey(light)) {
                    LightShadows lightShadow = oldLightShadows[light];
                    light.shadows = lightShadow;
                }
            }
        }

        void ManageShadows() {
            if (lights == null || Time.time - lastLightCheckTime > 10f) {
                lastLightCheckTime = Time.time;

                Light[] newLights = FindObjectsOfType(typeof(Light)) as Light[];
                if (lights == null) {
                    lights = new List<Light>(newLights);
                } else {
                    for (int k = 0; k < newLights.Length; k++) {
                        Light light = newLights[k];
                        if (light == null)
                            continue;
                        if (!lights.Contains(light))
                            lights.Add(light);
                    }
                }
            }
            // Annotate lights
            int lightsCount = lights.Count;
            for (int k = 0; k < lightsCount; k++) {
                Light light = lights[k];
                if (light == null)
                    continue;
                LightShadows oldLightShadow;
                if (oldLightShadows.ContainsKey(light)) {
                    oldLightShadow = oldLightShadows[light];
                } else {
                    oldLightShadow = light.shadows;
                    oldLightShadows.Add(light, oldLightShadow);
                }
            }
            // Check for good shadows
            if (avgDownsampling >= 1f) {
                for (int k = 0; k < lightsCount; k++) {
                    Light light = lights[k];
                    if (light == null)
                        continue;
                    LightShadows oldLightShadow = oldLightShadows[light];
                    if (light.shadows != oldLightShadow) {
                        light.shadows = oldLightShadow;
                    }
                }
            } else {
                // Reduce shadow quality
                // Look for soft shadows first
                bool pendingHardShadows = true;
                if (avgDownsampling > 0.5f) {
                    for (int k = 0; k < lightsCount; k++) {
                        Light light = lights[k];
                        if (light == null)
                            continue;
                        if (light.shadows == LightShadows.Soft) {
                            light.shadows = LightShadows.Hard;
                            pendingHardShadows = false;
                        }
                    }
                }
                if (pendingHardShadows) {
                    for (int k = 0; k < lightsCount; k++) {
                        Light light = lights[k];
                        if (light == null)
                            continue;
                        if (light.shadows != LightShadows.None) {
                            light.shadows = LightShadows.None;
                        }
                    }
                }
            }
        }


        #endregion

        #region Useful functions

        /// <summary>
        /// Returns an adjusted screen position that takes into account current downscaling
        /// </summary>
        /// <returns>The correct screen position.</returns>
        public Vector3 AdjustScreenPosition(Vector3 position) {

            if (_method == XFRAME_DOWNSAMPLING_METHOD.Disabled || niceMode || rt == null) {
                return position;
            }

            position.x *= (float)rt.width / screenWidth;

            if (_method != XFRAME_DOWNSAMPLING_METHOD.HorizontalDownsampling) {
                position.y *= (float)rt.height / screenHeight;
            }

            return position;
        }


        /// <summary>
        /// Returns a ray from camera in the direction defined by a screen position
        /// </summary>
        /// <returns>The point to ray.</returns>
        public Ray ScreenPointToRay(Camera camera, Vector3 position) {
            return camera.ScreenPointToRay(AdjustScreenPosition(position));
        }

        void DoRaycasting() {
            RaycastHit hit;
            bool pressed = Input.GetMouseButtonDown(0);
            bool released = Input.GetMouseButtonUp(0);
            if (pressed || released) {
                Ray ray = ScreenPointToRay(mainCamera, AdjustScreenPosition(Input.mousePosition));
                if (Physics.Raycast(ray, out hit)) {
                    GameObject o = hit.collider.gameObject;
                    if (pressed) {
                        o.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                    } else if (released) {
                        o.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                        PointerEventData pointerEventData = new PointerEventData(null);
                        pointerEventData.button = PointerEventData.InputButton.Left;
                        o.SendMessage("OnPointerClick", pointerEventData, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a RaycastResult struct with information about the raycasting
        /// </summary>
        /// <returns>The raycast.</returns>
        /// <param name="pointerPosition">Pointer position.</param>
        public RaycastResult Raycast(Camera camera, Vector3 pointerPosition) {
            Ray ray = ScreenPointToRay(camera, pointerPosition);
            RaycastHit hit;
            RaycastResult result = new RaycastResult();
            if (Physics.Raycast(ray, out hit)) {
                result.distance = hit.distance;
                result.gameObject = hit.collider.gameObject;
                result.worldPosition = hit.point;
                result.worldNormal = hit.normal;
                result.screenPosition = pointerPosition;
            }
            return result;
        }

        /// <summary>
        /// Returns true if pointer is currently over an UI element
        /// </summary>
        public bool IsPointerOverUIElement() {
            return IsPointerOverUIElement(Input.mousePosition);
        }


        /// <summary>
        /// Return true if pointer is over an UI element
        /// </summary>
        /// <param name="pointerPosition">Mouse or pointer position.</param>
        public bool IsPointerOverUIElement(Vector3 pointerPosition) {
            if (canvases == null) return false;
            if (hitResults == null) {
                hitResults = new List<RaycastResult>();
            }
            for (int k = 0; k < canvases.Length; k++) {
                Canvas canvas = canvases[k];
                if (canvas == null) continue;
                GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
                if (gr == null) continue;
                PointerEventData ped = new PointerEventData(EventSystem.current);
                ped.position = canvas.renderMode != RenderMode.ScreenSpaceOverlay ? AdjustScreenPosition(pointerPosition) : pointerPosition;
                hitResults.Clear();
                gr.Raycast(ped, hitResults);
                if (hitResults.Count > 0) return true;
            }
            return false;
        }


        /// <summary>
        /// Returns true if pointer is currently over an UI element
        /// </summary>
        public bool IsPointerOverUIButton() {
            return IsPointerOverUIButton(Input.mousePosition);
        }


        /// <summary>
        /// Return true if pointer is over an UI selectable element (button, toggle)
        /// </summary>
        /// <param name="pointerPosition">Mouse or pointer position.</param>
        public bool IsPointerOverUIButton(Vector3 pointerPosition) {
            if (canvases == null) return false;
            if (hitResults == null) {
                hitResults = new List<RaycastResult>();
            }
            for (int k = 0; k < canvases.Length; k++) {
                Canvas canvas = canvases[k];
                if (canvas == null) continue;
                GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
                if (gr == null) continue;
                PointerEventData ped = new PointerEventData(EventSystem.current);
                ped.position = canvas.renderMode != RenderMode.ScreenSpaceOverlay ? AdjustScreenPosition(pointerPosition) : pointerPosition;
                hitResults.Clear();
                gr.Raycast(ped, hitResults);
                int count = hitResults.Count;
                for (int j = 0; j < count; j++) {
                    Selectable s = hitResults[j].gameObject.GetComponent<Selectable>();
                    if (s != null && s.interactable) return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Returns true if pointer is over a gameobject (needs to have a collider)
        /// </summary>
        /// <param name="pointerPosition">Position.</param>
        public bool IsPointerOverGameObject(Camera camera, Vector3 pointerPosition) {
            Ray ray = ScreenPointToRay(camera, pointerPosition);
            return Physics.Raycast(ray);
        }

        #endregion


    }
}



