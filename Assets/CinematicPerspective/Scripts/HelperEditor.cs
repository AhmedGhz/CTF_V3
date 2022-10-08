using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CinematicPerspective
{
    /// <summary>
    /// This class will only run its methods on Editor Mode
    /// </summary>
    public class HelperEditor
    {
#if UNITY_EDITOR
        internal static void DrawIcon(GameObject gameObject, int idx)
        {

            var largeIcons = GetTextures("sv_label_", string.Empty, 0, 8);

            var egu = typeof(EditorGUIUtility);
            var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            var setIcon = egu.GetMethod("SetIconForObject", flags, null, new System.Type[] { typeof(UnityEngine.Object), typeof(Texture2D) }, null);

            if (idx == -1)
            {
                var args = new object[] { gameObject, (Texture2D)null };
                setIcon.Invoke(null, args);
            }
            else
            {
                var icon = largeIcons[idx];
                var args = new object[] { gameObject, icon.image };
                setIcon.Invoke(null, args);
            }

        }

        internal static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
        {

            GUIContent[] array = new GUIContent[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = EditorGUIUtility.IconContent(baseName + (startIndex + i) + postFix);
            }
            return array;

        }
#endif
    }
}
