using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SnazzlebotTools.ENPCHealthBars
{
    [CustomEditor(typeof(ENPCHealthBar))]
    public class ENPCHealthBarEditor : Editor
    {
        private ENPCHealthBar _healthBar;

        public void OnEnable()
        {
            _healthBar = target as ENPCHealthBar;
        }

        public override void OnInspectorGUI()
        {
            if (_healthBar == null) return;

            _healthBar.BarColor            = EditorGUILayout.ColorField("Color", _healthBar.BarColor);
            _healthBar.BarBackgroundColor  = EditorGUILayout.ColorField("Background Color", _healthBar.BarBackgroundColor);
            EditorGUILayout.Separator();
            _healthBar.Height              = EditorGUILayout.FloatField("Height", _healthBar.Height);
            _healthBar.Width               = EditorGUILayout.FloatField("Width", _healthBar.Width);
            _healthBar.YPosition           = EditorGUILayout.FloatField("Y Position", _healthBar.YPosition);
            _healthBar.OnlyShowIfDamaged   = EditorGUILayout.Toggle("Only show if damaged", _healthBar.OnlyShowIfDamaged, EditorStyles.toggle);
            EditorGUILayout.Separator();
            _healthBar.MaxValue            = EditorGUILayout.IntField("Max Value", _healthBar.MaxValue);
            _healthBar.Value               = EditorGUILayout.IntSlider("Value", _healthBar.Value, 0, _healthBar.MaxValue);
            EditorGUILayout.Separator();
            _healthBar.FaceCamera          = (EditorGUILayout.ObjectField("Face Camera", _healthBar.FaceCamera, typeof(Camera), true) as Camera) ?? Camera.main;
            EditorGUILayout.Separator();

            _healthBar.ShowName            = EditorGUILayout.BeginToggleGroup("Show Name", _healthBar.ShowName);
            {
                EditorGUI.indentLevel++;
                _healthBar.Name            = EditorGUILayout.TextField("Name", _healthBar.Name);
                _healthBar.NameColor       = EditorGUILayout.ColorField("Color", _healthBar.NameColor);
                _healthBar.NameFontSize    = EditorGUILayout.IntField("Font Size", _healthBar.NameFontSize);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndToggleGroup();

            _healthBar.ShowLevel           = EditorGUILayout.BeginToggleGroup("Show Level", _healthBar.ShowLevel);
            {
                EditorGUI.indentLevel++;
                _healthBar.Level               = EditorGUILayout.IntField("Level", _healthBar.Level);
                _healthBar.LevelColor          = EditorGUILayout.ColorField("Color", _healthBar.LevelColor);
                _healthBar.LevelBgColor        = EditorGUILayout.ColorField("Background Color", _healthBar.LevelBgColor);
                _healthBar.LevelFontSize       = EditorGUILayout.IntField("Font Size", _healthBar.LevelFontSize);
                _healthBar.LevelBackgroundSize = EditorGUILayout.FloatField("Background Size", _healthBar.LevelBackgroundSize);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndToggleGroup();
            
            _healthBar.ShowValue           = EditorGUILayout.BeginToggleGroup("Show value", _healthBar.ShowValue);
            {
                EditorGUI.indentLevel++;
                _healthBar.ValueColor      = EditorGUILayout.ColorField("Color", _healthBar.ValueColor);
                _healthBar.ValueFontSize   = EditorGUILayout.IntField("Font Size", _healthBar.ValueFontSize);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndToggleGroup();
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(_healthBar);
            }
        }
    }

    [InitializeOnLoad]
    public static class HealthBarHierarchyIcon
    {
        private static readonly Texture2D Icon;
        private static readonly Dictionary<int, GameObject> HealthBars;

        static HealthBarHierarchyIcon()
        {
            Icon = (Texture2D)AssetDatabase.LoadMainAssetAtPath("Assets/ENPCHealthBars/Editor/ENPCHealthBar Icon.png");

            if (Icon == null)
            {
                return;
            }

            HealthBars = new Dictionary<int, GameObject>();
            EditorApplication.hierarchyWindowChanged += HierarchyWindowChanged;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        private static void HierarchyWindowChanged()
        {
            HealthBars.Clear();

            var bars = Object.FindObjectsOfType<ENPCHealthBar>();
            foreach (var bar in bars.Select(x => x.transform.Find("HealthBar")).Where(x => x != null))
            {
                var instanceId = bar.gameObject.GetInstanceID();
                if (!HealthBars.ContainsKey(instanceId))
                {
                    HealthBars.Add(instanceId, bar.gameObject);
                }
            }
        }

        private static void HierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
        {
            if (!HealthBars.ContainsKey(instanceId) || HealthBars[instanceId] == null)
            {
                return;
            }

            var rect = new Rect(selectionRect);
            rect.x = rect.width - 15;
            rect.y += 2;
            rect.width = 15;

            GUI.Label(rect, Icon);
        }
    }
}