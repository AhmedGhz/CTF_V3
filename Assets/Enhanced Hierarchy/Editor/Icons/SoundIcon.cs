using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace EnhancedHierarchy.Icons {
    public sealed class SoundIcon : IconBase {

        private static AudioSource audioSource;
        private static AnimBool currentAnim;
        private static readonly Dictionary<AudioSource, AnimBool> sourcesAnim = new Dictionary<AudioSource, AnimBool>();
        private static Texture icon;

        public override string Name { get { return "Audio Source Icon"; } }
        public override float Width {
            get {
                if (audioSource == null || currentAnim == null)
                    return 0f;

                return currentAnim.faded * (base.Width - 2f);
            }
        }

        public override Texture2D PreferencesPreview { get { return AssetPreview.GetMiniTypeThumbnail(typeof(AudioSource)); } }

        //public override string PreferencesTooltip { get { return "Some tag for the tooltip here"; } }

        static SoundIcon() {
            EditorApplication.update += () => {
                if (!Preferences.IsButtonEnabled(new SoundIcon()))
                    return;

                foreach (var kvp in sourcesAnim)
                    if (kvp.Key && kvp.Value != null)
                        kvp.Value.target = kvp.Key.isPlaying;
            };
        }

        public override void Init() {
            if (!EnhancedHierarchy.IsGameObject)
                return;

            var comps = EnhancedHierarchy.Components;
            audioSource = null;

            for (var i = 0; i < comps.Count; i++)
                if (comps[i] is AudioSource) {
                    audioSource = comps[i] as AudioSource;
                    break;
                }

            if (!audioSource)
                return;

            if (!sourcesAnim.TryGetValue(audioSource, out currentAnim)) {
                sourcesAnim[audioSource] = currentAnim = new AnimBool(audioSource.isPlaying);
                currentAnim.valueChanged.AddListener(EditorApplication.RepaintHierarchyWindow);
            }
        }

        public override void DoGUI(Rect rect) {
            if (!EnhancedHierarchy.IsRepaintEvent || !EnhancedHierarchy.IsGameObject || !audioSource || Width <= 1f)
                return;

            using(ProfilerSample.Get()) {
                if (!icon)
                    icon = EditorGUIUtility.ObjectContent(null, typeof(AudioSource)).image;

                rect.yMax -= 1f;
                rect.yMin += 1f;

                GUI.DrawTexture(rect, icon, ScaleMode.StretchToFill);
            }
        }
    }
}
