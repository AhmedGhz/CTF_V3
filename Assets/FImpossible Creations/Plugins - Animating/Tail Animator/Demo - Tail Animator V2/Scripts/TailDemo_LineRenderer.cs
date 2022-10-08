using FIMSpace.FTail;
using System.Collections.Generic;
using UnityEngine;

public class TailDemo_LineRenderer : MonoBehaviour
{
    [Header("Tail will be generated on Start()", order = 0)]
    [Header("References", order = 1)]
    public LineRenderer Line;
    public TailAnimator2 TailWithSettings;
    public Vector3 GeneratePosition = Vector3.down;

    [Header("Parameters")]
    public int SegmentsCount = 10;
    public bool DetachForOptimization = false;

    [Header("Optional")]
    public bool DrawGizmos = true;

    private void Reset()
    {
        Line = GetComponent<LineRenderer>();
        if (!Line) Line = gameObject.AddComponent<LineRenderer>();
    }

    private void Update()
    {
        Line.positionCount = TailWithSettings.TailSegments.Count;

        for (int i = 0; i < TailWithSettings.TailSegments.Count; i++)
        {
            Line.SetPosition(i, TailWithSettings.TailSegments[i].ProceduralPosition);
        }
    }

    void Start()
    {
        List<Transform> tailSegments = new List<Transform>();
        Transform preVSegment = transform;

        if (TailWithSettings == null) TailWithSettings = GetComponent<TailAnimator2>();

        for (int i = 0; i < SegmentsCount; i++)
        {
            Vector3 targetPos = transform.position + transform.TransformVector(GeneratePosition * i);
            GameObject segment = new GameObject("Tail-LineRednerer-Segment[" + i + "]");
            segment.transform.rotation = transform.rotation;
            segment.transform.localScale = transform.lossyScale;
            segment.transform.SetParent(preVSegment, true);
            segment.transform.position = targetPos;
            preVSegment = segment.transform;

            tailSegments.Add(segment.transform);
        }

        if (TailWithSettings)
        {
            TailWithSettings.DetachChildren = DetachForOptimization;
            TailWithSettings.User_SetTailTransforms(tailSegments);
            TailWithSettings.enabled = true;
        }
        else
        {
            TailWithSettings = gameObject.AddComponent<TailAnimator2>();
            TailWithSettings.DetachChildren = DetachForOptimization;
            TailWithSettings.User_SetTailTransforms(tailSegments);
            TailWithSettings.enabled = true;
        }

        if ( Line)
        {
            Line.positionCount = TailWithSettings.TailSegments.Count;
        }
    }

    private void OnValidate()
    {
        if (SegmentsCount < 2) SegmentsCount = 2;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!DrawGizmos) return;

        Gizmos.matrix = transform.localToWorldMatrix;
        UnityEditor.Handles.matrix = transform.localToWorldMatrix;

        for (int i = 0; i < SegmentsCount; i++)
        {
            Vector3 targetPos = GeneratePosition * i;
            UnityEditor.Handles.SphereHandleCap(0, targetPos, Quaternion.identity, UnityEditor.HandleUtility.GetHandleSize(targetPos) * 0.09f, EventType.Repaint);
        }

        if (!Application.isPlaying)
        {
            if ( Line)
            {
                Line.positionCount = SegmentsCount;
                for (int i = 0; i < SegmentsCount; i++)
                {
                    Line.SetPosition(i, transform.TransformPoint(GeneratePosition * i));
                }
            }
        }

        Gizmos.matrix = Matrix4x4.identity;
        UnityEditor.Handles.matrix = Matrix4x4.identity;
    }
#endif
}
