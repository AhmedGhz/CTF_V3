using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XFrameEffect {

    [RequireComponent(typeof(Canvas))]
    public class GraphicRaycasterProxy : GraphicRaycaster {

        [NonSerialized]
        public XFrameManager xframe;

        Canvas canvas;
        GraphicRaycaster gr;

        protected override void OnEnable() {
            canvas = this.GetComponent<Canvas>();
            gr = GetAnotherComponent<GraphicRaycaster>();
            if (gr != null) {
                blockingObjects = gr.blockingObjects;
                ignoreReversedGraphics = gr.ignoreReversedGraphics;
                gr.enabled = false;
            }
            base.OnEnable();
        }

        protected override void OnDisable() {
            if (gr != null) {
                gr.blockingObjects = blockingObjects;
                gr.ignoreReversedGraphics = ignoreReversedGraphics;
                gr.enabled = true;
            }
            base.OnDisable();
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList) {
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay) {
                if (xframe != null) {
                    if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null) {
                        canvas.worldCamera = xframe.mainCamera;
                    }
                    eventData.position = xframe.AdjustScreenPosition(eventData.position);
                }
            }
            base.Raycast(eventData, resultAppendList);
        }

        T GetAnotherComponent<T>() where T : Component {
            T[] components = GetComponents<T>();
            for (int k=0;k<components.Length;k++) {
                T c = components[k];
                if (c != this) return c;
            }
            return null;
        }
    }
}

