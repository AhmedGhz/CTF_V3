using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace SnazzlebotTools.ENPCHealthBars
{
    [ExecuteInEditMode]
    public class ENPCHealthBar : MonoBehaviour
    {
        [HideInInspector, SerializeField]
        private Transform _healthBar;

        private RectTransform _barTransform;
        private Slider _slider;
        private Image _backImage;
        private Image _frontImage;
        private Text _valueText;
        private Text _nameText;
        private Image _levelBg;
        private Text _levelText;

        [HideInInspector, SerializeField]
        private Color _barColor = Color.red;

        public Color BarColor
        {
            get
            {
                if (_frontImage != null) { _barColor = _frontImage.color; }
                return _barColor;
            }
            set
            {
                _barColor = value;
                if (_frontImage != null) { _frontImage.color = _barColor; }
            }
        }

        [HideInInspector, SerializeField]
        private Color _bgColor = new Color(0, 0, 0, 0.196f);

        public Color BarBackgroundColor
        {
            get
            {
                if (_backImage != null) { _bgColor = _backImage.color; }
                return _bgColor;
            }
            set
            {
                _bgColor = value;
                if (_backImage != null) { _backImage.color = _bgColor; }
            }
        }

        [HideInInspector, SerializeField]
        private float _height = 25;

        public float Height
        {
            get
            {
                if (_barTransform != null) { _height = _barTransform.sizeDelta.y; }
                return _height;
            }
            set
            {
                _height = value;
                if (_barTransform != null) { _barTransform.sizeDelta = new Vector2(_barTransform.sizeDelta.x, _height); }
            }
        }

        [HideInInspector, SerializeField]
        private float _width = 150;

        public float Width
        {
            get
            {
                if (_barTransform != null) { _width = _barTransform.sizeDelta.x; }
                return _width;
            }
            set
            {
                _width = value;
                if (_barTransform != null) { _barTransform.sizeDelta = new Vector2(_width, _barTransform.sizeDelta.y); }
            }
        }

        [HideInInspector, SerializeField]
        private int _maxValue = 100;

        /// <summary>
        /// The amount of full health the NPC starts with before taking damage.
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                if (_slider != null) { _slider.maxValue = _maxValue; }
                UpdateValueText();
            }
        }

        [HideInInspector, SerializeField]
        private int _value = 100;

        [HideInInspector, SerializeField]
        private bool _showValue;

        public bool ShowValue
        {
            get { return _showValue; }
            set
            {
                _showValue = value;
                if (_valueText != null) { _valueText.gameObject.SetActive(_showValue); }
            }
        }

        [HideInInspector, SerializeField]
        private bool _showName;

        public bool ShowName
        {
            get { return _showName; }
            set
            {
                _showName = value;
                if (_nameText != null) { _nameText.gameObject.SetActive(_showName); }
            }
        }

        [HideInInspector, SerializeField]
        private bool _showLevel;

        public bool ShowLevel
        {
            get { return _showLevel; }
            set
            {
                _showLevel = value;
                if (_levelBg == null || _levelText == null) return;
                _levelBg.gameObject.SetActive(_showLevel);
                _levelText.gameObject.SetActive(_showLevel);
            }
        }

        [HideInInspector, SerializeField]
        private Color _valueColor = Color.white;

        public Color ValueColor
        {
            get
            {
                if (_valueText != null) { _valueColor = _valueText.color; }
                return _valueColor;
            }
            set
            {
                _valueColor = value;
                if (_valueText != null) { _valueText.color = _valueColor; }
            }
        }

        [HideInInspector, SerializeField]
        private int _valueFontSize = 14;

        public int ValueFontSize
        {
            get
            {
                if (_valueText != null) { _valueFontSize = _valueText.fontSize; }
                return _valueFontSize;
            }
            set
            {
                _valueFontSize = value;
                if (_valueText != null) { _valueText.fontSize = _valueFontSize; }
            }
        }

        [HideInInspector, SerializeField]
        private Color _nameColor = Color.white;

        public Color NameColor
        {
            get
            {
                if (_nameText != null) { _nameColor = _nameText.color; }
                return _nameColor;
            }
            set
            {
                _nameColor = value;
                if (_nameText != null) { _nameText.color = _nameColor; }
            }
        }

        [HideInInspector, SerializeField]
        private int _nameFontSize = 14;

        public int NameFontSize
        {
            get
            {
                if (_nameText != null) { _nameFontSize = _nameText.fontSize; }
                return _nameFontSize;
            }
            set
            {
                _nameFontSize = value;
                if (_nameText != null)
                {
                    _nameText.fontSize = _nameFontSize;
                    _nameText.GetComponent<RectTransform>().sizeDelta = new Vector2(0, _nameFontSize + (_nameFontSize / 4));
                }
            }
        }

        [HideInInspector, SerializeField]
        private Color _levelBgColor = Color.white;

        public Color LevelBgColor
        {
            get
            {
                if (_levelBg != null) { _levelBgColor = _levelBg.color; }
                return _levelBgColor;
            }
            set
            {
                _levelBgColor = value;
                if (_levelBg != null) { _levelBg.color = _levelBgColor; }
            }
        }

        [HideInInspector, SerializeField]
        private Color _levelColor = Color.white;

        public Color LevelColor
        {
            get
            {
                if (_levelText != null) { _levelColor = _levelText.color; }
                return _levelColor;
            }
            set
            {
                _levelColor = value;
                if (_levelText != null) { _levelText.color = _levelColor; }
            }
        }

        [HideInInspector, SerializeField]
        private float _levelBackgroundSize = 25;

        public float LevelBackgroundSize
        {
            get
            {
                if (_levelBg != null) { _levelBackgroundSize = _levelBg.GetComponent<RectTransform>().sizeDelta.x; }
                return _levelBackgroundSize;
            }
            set
            {
                _levelBackgroundSize = value;
                if (_levelBg != null)
                {
                    var rectSize = new Vector2(_levelBackgroundSize, _levelBackgroundSize);
                    _levelBg.GetComponent<RectTransform>().sizeDelta = rectSize;
                    if (_levelText != null)
                    {
                        _levelText.GetComponent<RectTransform>().sizeDelta = rectSize;
                    }
                }
            }
        }

        [HideInInspector, SerializeField]
        private int _levelFontSize = 14;

        public int LevelFontSize
        {
            get
            {
                if (_levelText != null) { _levelFontSize = _levelText.fontSize; }
                return _levelFontSize;
            }
            set
            {
                _levelFontSize = value;
                if (_levelText != null) { _levelText.fontSize = _levelFontSize; }
            }
        }

        /// <summary>
        /// The value of the NPC's health. Use this to increase or reduce health.
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;

                if (_slider == null) return;

                if (_value > MaxValue)
                {
                    _value = MaxValue;
                }
                else if (_value < 0)
                {
                    _value = 0;
                }

                _slider.value = _value;
                UpdateValueText();

                if (_value == 0 && BarBackgroundColor.a <= 0)
                {
                    _healthBar.gameObject.SetActive(false);
                }
                else if (!_healthBar.gameObject.activeSelf)
                {
                    _healthBar.gameObject.SetActive(true);
                }
            }
        }

        public string Name
        {
            get
            {
                return _nameText != null ? _nameText.text : null;
            }
            set
            {
                if (_nameText != null) { _nameText.text = value; }
            }
        }

        public int Level
        {
            get
            {
                var result = 0;
                if (_levelText != null)
                {
                    int.TryParse(_levelText.text, out result);
                }

                return result;
            }
            set
            {
                if (_levelText != null) { _levelText.text = value.ToString(); }
            }
        }

        [HideInInspector, SerializeField]
        private float _yPosition = 1.5f;

        public float YPosition
        {
            get { return _yPosition; }
            set
            {
                _yPosition = value;

                if (_barTransform != null)
                {
                    _barTransform.anchoredPosition = new Vector2(_barTransform.anchoredPosition.x, _yPosition);
                }
            }
        }

        [HideInInspector, SerializeField]
        private Camera _faceCamera;

        public Camera FaceCamera
        {
            get { return _faceCamera; }
            set { _faceCamera = value; }
        }

        public bool OnlyShowIfDamaged;

        void Awake()
        {
            InitCanvas();
            InitSlider();
            InitNameText();
            InitLevelText();
            InitValueText();
        }

        void Reset()
        {
            _healthBar = transform.Find("HealthBar");
            BarColor = Color.red;
            BarBackgroundColor = new Color(0, 0, 0, 0.196f);
            Height = 25;
            Width = 150;
            YPosition = 1.5f;
            MaxValue = 100;
            Value = 100;
            FaceCamera = Camera.main;

            ShowName = false;
            Name = "Name";
            NameColor = Color.white;
            NameFontSize = 14;

            ShowLevel = false;
            Level = 1;
            LevelColor = Color.white;
            LevelBgColor = Color.black;
            LevelBackgroundSize = 25;
            LevelFontSize = 14;

            ShowValue = false;
            ValueColor = Color.white;
            ValueFontSize = 14;
        }

        private void InitCanvas()
        {
            //Init the main healthbar canvas.
            _healthBar = _healthBar ?? transform.Find("HealthBar");
            if (_healthBar == null)
            {
                _healthBar = new GameObject("HealthBar", typeof(Canvas)).transform;
                _healthBar.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                _healthBar.SetParent(transform, false);

                var barTransform = _healthBar.GetComponent<RectTransform>();
                barTransform.anchorMin = Vector2.zero;
                barTransform.anchorMax = Vector2.zero;
                barTransform.sizeDelta = new Vector2(Width, Height);
                barTransform.anchoredPosition = new Vector2(0, YPosition);
                _barTransform = barTransform;

                var scaler = _healthBar.gameObject.AddComponent<CanvasScaler>();
                scaler.dynamicPixelsPerUnit = 4;
            }
            else
            {
                _barTransform = _healthBar.GetComponent<RectTransform>();
            }
        }

        private void InitSlider()
        {
            var sliderTransform = _healthBar.Find("Slider");
            if (sliderTransform == null)
            {
                sliderTransform = new GameObject("Slider", typeof(Slider)).transform;
                RectTransformFillParent(sliderTransform.GetComponent<RectTransform>());
                sliderTransform.SetParent(_healthBar, false);

                var slider = sliderTransform.GetComponent<Slider>();
                slider.interactable = false;
                slider.maxValue = MaxValue;
                slider.value = Value;
                slider.transition = Selectable.Transition.None;
                _slider = slider;
            }
            else
            {
                _slider = sliderTransform.GetComponent<Slider>();
            }

            //Background
            var fillArea = sliderTransform.Find("Fill Area");
            if (fillArea == null)
            {
                fillArea = new GameObject("Fill Area", typeof(RectTransform), typeof(Image)).transform;
                RectTransformFillParent(fillArea.GetComponent<RectTransform>());
                fillArea.SetParent(sliderTransform, false);

                var backImage = fillArea.GetComponent<Image>();
                backImage.sprite = Resources.Load<Sprite>("ENPCHealthBar");
                backImage.type = Image.Type.Sliced;
                backImage.color = BarBackgroundColor;
                _backImage = backImage;
            }
            else
            {
                _backImage = fillArea.GetComponent<Image>();
            }

            //Foreground
            var fill = fillArea.Find("Fill");
            if (fill == null)
            {
                fill = new GameObject("Fill", typeof(RectTransform), typeof(Image)).transform;
                var fillTransform = fill.GetComponent<RectTransform>();
                RectTransformFillParent(fillTransform);
                fill.SetParent(fillArea, false);

                var frontImage = fill.GetComponent<Image>();
                frontImage.sprite = _backImage.sprite;
                frontImage.type = Image.Type.Sliced;
                frontImage.color = BarColor;

                _frontImage = frontImage;
                _slider.fillRect = fillTransform;
            }
            else
            {
                _frontImage = fill.GetComponent<Image>();
            }
        }

        private void InitNameText()
        {
            var textTransform = _healthBar.Find("Name");
            if (textTransform == null)
            {
                textTransform = new GameObject("Name", typeof(Text)).transform;
                var rectTransform = textTransform.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.sizeDelta = new Vector2(0, 16);
                rectTransform.pivot = new Vector2(.5f, 0);
                textTransform.SetParent(_healthBar, false);

                var textItem = textTransform.GetComponent<Text>();
                textItem.text = "Name";
                textItem.alignment = TextAnchor.MiddleCenter;
                textItem.gameObject.SetActive(ShowName);
                textItem.color = NameColor;
                textItem.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                textItem.fontStyle = FontStyle.Bold;
                textItem.supportRichText = false;
                textItem.horizontalOverflow = HorizontalWrapMode.Overflow;
                _nameText = textItem;
            }
            else
            {
                _nameText = textTransform.GetComponent<Text>();
            }
        }

        private void InitLevelText()
        {
            var tfm = _healthBar.Find("Level Bg");
            if (tfm == null)
            {
                tfm = new GameObject("Level Bg", typeof (Image)).transform;
                var rectTfm = tfm.GetComponent<RectTransform>();
                rectTfm.anchorMin = new Vector2(0, .5f);
                rectTfm.anchorMax = new Vector2(0, .5f);
                rectTfm.sizeDelta = new Vector2(25, 25);
                rectTfm.pivot = new Vector2(1, .5f);
                tfm.SetParent(_healthBar, false);

                var image = tfm.GetComponent<Image>();
                image.sprite = Resources.Load<Sprite>("ENPCHealthBar-levelbg");
                image.color = Color.black;
                image.gameObject.SetActive(ShowLevel);
                _levelBg = image;
            }
            else
            {
                _levelBg = tfm.GetComponent<Image>();
            }

            tfm = _healthBar.Find("Level");
            if (tfm == null)
            {
                tfm = new GameObject("Level", typeof(Text)).transform;
                var rectTfm = tfm.GetComponent<RectTransform>();
                rectTfm.anchorMin = new Vector2(0, .5f);
                rectTfm.anchorMax = new Vector2(0, .5f);
                rectTfm.sizeDelta = new Vector2(25, 25);
                rectTfm.pivot = new Vector2(1, .5f);
                tfm.SetParent(_healthBar, false);

                var textItem = tfm.GetComponent<Text>();
                textItem.text = "1";
                textItem.alignment = TextAnchor.MiddleCenter;
                textItem.color = LevelColor;
                textItem.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                textItem.fontStyle = FontStyle.Bold;
                textItem.supportRichText = false;
                textItem.gameObject.SetActive(ShowLevel);
                textItem.horizontalOverflow = HorizontalWrapMode.Overflow;
                textItem.verticalOverflow = VerticalWrapMode.Overflow;
                _levelText = textItem;
            }
            else
            {
                _levelText = tfm.GetComponent<Text>();
            }
        }

        private void InitValueText()
        {
            var valueTextTransform = _healthBar.Find("Value");
            if (valueTextTransform == null)
            {
                valueTextTransform = new GameObject("Value", typeof(Text)).transform;
                RectTransformFillParent(valueTextTransform.GetComponent<RectTransform>());
                valueTextTransform.GetComponent<RectTransform>().offsetMax = new Vector2(-5,0);
                valueTextTransform.SetParent(_healthBar, false);

                var textItem = valueTextTransform.GetComponent<Text>();
                textItem.text = MaxValue.ToString(CultureInfo.InvariantCulture);
                textItem.alignment = TextAnchor.MiddleRight;
                textItem.gameObject.SetActive(ShowValue);
                textItem.color = ValueColor;
                textItem.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                textItem.fontStyle = FontStyle.Bold;
                textItem.supportRichText = false;
                textItem.horizontalOverflow = HorizontalWrapMode.Overflow;
                textItem.verticalOverflow = VerticalWrapMode.Overflow;
                _valueText = textItem;
            } 
            else
            {
                _valueText = valueTextTransform.GetComponent<Text>();
            }
        }

        void Update()
        {
            if (!Application.isPlaying) { return; }

            if (_healthBar == null) { return; }

            if (OnlyShowIfDamaged && MaxValue == Value)
            {
                _healthBar.GetComponent<Canvas>().enabled = false;
            }
            else
            {
                _healthBar.GetComponent<Canvas>().enabled = true;
            }

            if (FaceCamera != null && _healthBar.gameObject.activeSelf)
            {
                _healthBar.LookAt(_healthBar.position + FaceCamera.transform.rotation * Vector3.forward,
                FaceCamera.transform.rotation * Vector3.up);
            }
        }

        private void UpdateValueText()
        {
            if (_valueText == null) { return; }

            _valueText.text = Value.ToString(CultureInfo.InvariantCulture);

            if (Value < MaxValue)
            {
                _valueText.text += " / " + MaxValue;
            }
        }

        void OnDisable()
        {
            if (_healthBar != null)
            {
                _healthBar.GetComponent<Canvas>().enabled = false;
            }
        }

        void OnEnable()
        {
            if (_healthBar != null)
            {
                _healthBar.GetComponent<Canvas>().enabled = true;
            }
        }

        void OnDestroy()
        {
            if (Application.isEditor && !Application.isPlaying && _healthBar != null)
            {
                DestroyImmediate(_healthBar.gameObject);
                return;
            }

            if (_healthBar != null)
            {
                Destroy(_healthBar.gameObject);
            }
        }

        private static void RectTransformFillParent(RectTransform rTransform)
        {
            rTransform.anchorMin        = Vector2.zero;
            rTransform.anchorMax        = Vector2.one;
            rTransform.sizeDelta        = Vector2.zero;
            rTransform.localScale       = Vector3.one;
            rTransform.anchoredPosition = Vector2.zero;
        }
    }
}