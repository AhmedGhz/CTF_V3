using FIMSpace.FTail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using FIMSpace.FEditor;
using UnityEditor;
#endif
using UnityEngine;

public class TailDemo_SkinnedMeshGenerator : MonoBehaviour
{
    [FPD_Header("Generate Tail Settings", 0, 4)]
    public int circlePoints = 16;
    public int LengthSegments = 18;
    public float ForwardLength = 5f;
    public float Fatness = .25f;
    [FPD_FixedCurveWindow(0f, 0f, 1f, 1f)]
    public AnimationCurve LengthScale = AnimationCurve.EaseInOut(0.5f, 1f, 1f, .1f);
    public bool RandomizeAtStart = false;
    public Material mat;
    public bool DrawGizmos = true;

    [FPD_Header("Skinning Settings", 6, 4)]
    public int BonesCount = 10;

    [FPD_Header("Tail Animtor Settings", 6, 4)]
    public bool AddTailAnimator = true;
    public bool DetachForOptimization = true;
    [Header("If not adding tail animator")]
    public TailAnimator2 TargetTailAnimator;
    public bool SetAsParent = true;

    Vector3[,] toDraw;
    List<VertGenHelper> toDrawHelpers;

    private void Start()
    {
        if ( RandomizeAtStart)
        {
            Keyframe[] k = new Keyframe[UnityEngine.Random.Range(9, 24)];
            float step = 1f / (float)(k.Length-1);

            // Generating random scale shape
            for (int i = 0; i < k.Length; i++)
            {
                float from = Mathf.Lerp(1.5f, 0.8f, step * i);
                float to = Mathf.Lerp(.4f, 0.1f, step * i);
                float val = UnityEngine.Random.Range(from, to);
                k[i] = new Keyframe(step * i, val);
            }

            LengthScale.keys = k;

            for (int i = 0; i < k.Length; i++)
            {
                LengthScale.SmoothTangents(i, UnityEngine.Random.Range(0.35f, .6f));
            }

            BonesCount += UnityEngine.Random.Range(-3, 8);
            Fatness *= UnityEngine.Random.Range(0.85f, 1.25f);
            ForwardLength *= UnityEngine.Random.Range(0.925f, 1.125f);
        }

        //StartCoroutine(GenerateFrameByFrame()); // Showing process of generating tail frame by frame
        //GenerateMesh(false); // Just generating tail mesh and adding meshFilter
        SkinMesh();
    }

    private void OnValidate()
    {
        if (BonesCount < 2) BonesCount = 2;
        if (Fatness < 0.01f) Fatness = 0.01f;
        if (ForwardLength < 0.01f) ForwardLength = 0.01f;
        if (LengthSegments < 2) LengthSegments = 2;
        if (circlePoints < 3) circlePoints = 3;

        toDraw = GetTailPoints();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (toDraw == null) return;

        Handles.color = new Color(0.23f, 1f, 0.5f, 0.4f);
        for (int b = 0; b < BonesCount; b++)
        {
            Vector3 bonePos = transform.position + transform.TransformVector(GetBonePos(b));
            Vector3 fBonePos = transform.position + transform.TransformVector(GetBonePos(b+1));

            FGUI_Handles.DrawBoneHandle(bonePos, fBonePos, transform.forward, 1f);
        }

        if (!DrawGizmos) return;
        #region Drawing points

        float cStep = 360f / (float)circlePoints;
        int i = 0;

        MeshFilter f = GetComponent<MeshFilter>();

        Gizmos.matrix = transform.localToWorldMatrix;
        Handles.matrix = transform.localToWorldMatrix;

        Gizmos.color = new Color(1f, 1f, 1f, 0.3f);

        for (int l = 0; l < toDraw.GetLength(0); l++) //length
            for (int c = 0; c < toDraw.GetLength(1); c++) //circle
            {
                Gizmos.DrawWireSphere(toDraw[l, c], Fatness * 0.1f);

                if (l < 2)
                {
                    Handles.Label(toDraw[l, c], "[" + i + "]");

                    if (f)
                    {
                        if (f.mesh)
                        {
                            Vector2 uv = f.mesh.uv[GetHelper(l, c)];
                            Handles.Label(toDraw[l, c] + Vector3.right * 0.2f * Fatness, "[" + Math.Round(uv.x, 2) + "," + Math.Round(uv.y, 2) + "]");
                        }
                    }
                }

                i++;
            }

        #endregion

    }
#endif

    #region Mesh Generator Part

    Vector3[,] GetTailPoints()
    {
        Vector3[,] points = new Vector3[LengthSegments, circlePoints];

        float cStep = 360f / (float)circlePoints;

        for (int l = 0; l < LengthSegments; l++)
        {
            float stepMul = Fatness * LengthScale.Evaluate((float)l / (float)(LengthSegments-1));

            for (int c = 0; c < circlePoints; c++)
            {
                Vector3 point = Vector3.forward * l * (ForwardLength/LengthSegments);
                point.y += Mathf.Sin(cStep * c * Mathf.Deg2Rad) * stepMul;
                point.x += Mathf.Cos(cStep * c * Mathf.Deg2Rad) * stepMul;
                points[l, c] = point;
            }
        }

        return points;
    }

    private List<VertGenHelper> GetVertexHelpers()
    {
        List<VertGenHelper> g = new List<VertGenHelper>();

        Vector3[,] points = GetTailPoints();
        int i = 0;
        for (int l = 0; l < points.GetLength(0); l++) //length
        {
            for (int c = 0; c < points.GetLength(1); c++) //circle
            {
                VertGenHelper h = new VertGenHelper();
                h.index = i;
                h.l = l;
                h.c = c;
                h.p = points[l, c];
                h.n = h.p.normalized;
                i++;
                h.triangles = new List<int>();
                h.trianglesPos = new List<Vector3>();
                g.Add(h);
            }
        }


        // Settings triangles
        for (int v = 0; v < g.Count; v++)
        {
            VertGenHelper h = g[v];
            int t1, t2, t3;

            t1 = GetHelper(h.l, h.c + 1);
            if (t1 < g.Count)
            {
                t2 = GetHelper(h.l + 1, h.c);
                if (t2 < g.Count)
                {
                    h.triangles.Add(GetHelper(h.l, h.c));
                    h.triangles.Add(t1);
                    h.triangles.Add(t2);

                    h.trianglesPos.Add(g[h.triangles[0]].p);
                    h.trianglesPos.Add(g[h.triangles[1]].p);
                    h.trianglesPos.Add(g[h.triangles[2]].p);
                }
            }

            t1 = GetHelper(h.l + 1, h.c);
            if (t1 < g.Count)
            {
                t2 = GetHelper(h.l, h.c + 1);
                if (t2 < g.Count)
                {
                    t3 = GetHelper(h.l + 1, h.c + 1);

                    if (t3 < g.Count)
                    {
                        h.triangles.Add(t1);
                        h.triangles.Add(t2);
                        h.triangles.Add(t3);

                        h.trianglesPos.Add(g[h.triangles[0]].p);
                        h.trianglesPos.Add(g[h.triangles[1]].p);
                        h.trianglesPos.Add(g[h.triangles[2]].p);
                    }
                }
            }

        }


        return g;
    }

    public int GetHelper(int length, int circle)
    {
        if (circle >= circlePoints) circle -= circlePoints;
        return length * this.circlePoints + circle;
    }

    public IEnumerator GenerateFrameByFrame()
    {
        yield break;
        //        Material mat = null;

        //#if UNITY_EDITOR
        //        mat = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");
        //#endif

        //        Mesh tailMesh = new Mesh();

        //        List<VertGenHelper> h = GetVertexHelpers();

        //        List<Vector3> verts = new List<Vector3>();
        //        List<Vector3> normals = new List<Vector3>();
        //        List<int> triangles = new List<int>();

        //        for (int i = 0; i < h.Count; i++) verts.Add(h[i].p);

        //        for (int i = 0; i < h.Count; i++)
        //            for (int t = 0; t < h[i].triangles.Count; t++)
        //                triangles.Add(h[i].triangles[t]);

        //        for (int i = 0; i < h.Count; i++)
        //        {
        //            VertGenHelper v = h[i];
        //            v.n = new Vector3(v.p.x, v.p.y, 0f).normalized;
        //            normals.Add(v.n);
        //        }

        //        tailMesh.SetVertices(verts);

        //        MeshFilter f = gameObject.AddComponent<MeshFilter>();
        //        MeshRenderer r = gameObject.AddComponent<MeshRenderer>();

        //        List<int> ptriangles = new List<int>();

        //        for (int i = 0; i < verts.Count; i++)
        //        {
        //            ptriangles.Add(triangles[i * 3]);
        //            ptriangles.Add(triangles[i * 3+1]);
        //            ptriangles.Add(triangles[i * 3+2]);
        //            tailMesh.SetTriangles(ptriangles, 0);
        //            f.mesh = tailMesh;
        //            r.material = mat;
        //            yield return null;
        //        }


        //        tailMesh.SetTriangles(triangles, 0);
        //        tailMesh.SetNormals(normals);
        //        tailMesh.RecalculateNormals();
        //        tailMesh.RecalculateBounds();


        //        f.mesh = tailMesh;
        //        r.material = mat;

        //        yield break;
    }

    public Mesh GenerateMesh(bool drawMesh)
    {
        Mesh tailMesh = new Mesh();

        List<VertGenHelper> h = GetVertexHelpers();
        List<Vector3> verts = new List<Vector3>();
        for (int i = 0; i < h.Count; i++) verts.Add(h[i].p);

        tailMesh.SetVertices(verts);

        List<int> triangles = new List<int>();

        for (int i = 0; i < h.Count; i++)
            for (int t = 0; t < h[i].triangles.Count; t++)
                triangles.Add(h[i].triangles[t]);

        tailMesh.SetTriangles(triangles, 0);

        List<Vector3> normals = new List<Vector3>();
        List<Vector4> tangents = new List<Vector4>();
        for (int i = 0; i < h.Count; i++)
        {
            VertGenHelper v = h[i];
            v.n = new Vector3(v.p.x, v.p.y, 0f).normalized;
            normals.Add(v.n);

            Vector3 tanHelp = Vector3.Cross(Vector3.forward, v.n);
            Vector4 tan = tanHelp; tan.w = 1f;
            tangents.Add(tan);
        }

        List<Vector2> uvs = new List<Vector2>();
        for (int i = 0; i < h.Count; i++)
        {
            VertGenHelper v = h[i];
            Vector2 uv = new Vector2();
            uv.x = v.l / (float)LengthSegments; //if (v.l % 2 == 0) uv.x = 0; else uv.x = 1f;

            float cc = v.c + (float)circlePoints * 0.25f; // For now I can't fix issue with texture not looping
            if (cc >= circlePoints) cc -= circlePoints;
            //float cc = v.c; if ( v.c == circlePoints -1) cc = v.c + 1;
            uv.y = cc / (float)circlePoints;

            uv = new Vector2(uv.y, uv.x); // Flip 90
            uvs.Add(uv);
        }

        tailMesh.SetNormals(normals);
        tailMesh.RecalculateNormals();
        tailMesh.SetTangents(tangents);
        tailMesh.uv = uvs.ToArray();
        tailMesh.RecalculateBounds();

        if (drawMesh)
        {
            MeshFilter f = gameObject.AddComponent<MeshFilter>();
            MeshRenderer r = gameObject.AddComponent<MeshRenderer>();
            f.mesh = tailMesh;

#if UNITY_EDITOR
            r.material = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");
#endif
            if (mat) r.material = mat;
        }

        return tailMesh;
    }

    private class VertGenHelper
    {
        public int index;
        public int l;
        public int c;
        public Vector3 p;
        public Vector3 n;
        public List<int> triangles;
        public List<Vector3> trianglesPos;
    }

    #endregion

    void SkinMesh()
    {
        Mesh tailMesh = GenerateMesh(false);

        Vector3[] pos = new Vector3[BonesCount];
        Quaternion[] rot = new Quaternion[BonesCount];

        for (int i = 0; i < BonesCount; i++)
        {
            pos[i] = GetBonePos(i); // Local position like object would be in zero position and rotation
            rot[i] = Quaternion.identity;
        }

        FTail_SkinningVertexData[] data = FTail_Skinning.CalculateVertexWeightingData(tailMesh, pos, rot, Vector3.zero, 2, 0.7f, 0.3f);
        SkinnedMeshRenderer meshRend = FTail_Skinning.SkinMesh(tailMesh, pos, rot, data);

        meshRend.transform.position = transform.position;
        meshRend.transform.rotation = transform.rotation;
        meshRend.transform.localScale = transform.lossyScale;

        meshRend.material = mat;

        AddTailAnimatorToSkinnedMesh(meshRend);
    }


    /// <summary>
    /// Bone position in zero local space
    /// </summary>
    private Vector3 GetBonePos(int index)
    {
        float ratio = ForwardLength / BonesCount;
        Vector3 bonePos = Vector3.forward * ratio * index;

        // You can optimize bone ratio to be calculated once instead of inside iteration
        //float boneRatio = LengthSegments * ForwardLength * Fatness;
        //boneRatio /= (float)BonesCount;

        //boneRatio = ForwardLength / LengthSegments;

        return Vector3.forward * ratio * index;
        //return Vector3.forward * boneRatio * index;
    }


    void AddTailAnimatorToSkinnedMesh(SkinnedMeshRenderer skin)
    {
        if (TargetTailAnimator) // Applying generated bones to be animated by choosed tail animator
        {
            if (SetAsParent)
            {
                skin.transform.SetParent(TargetTailAnimator.transform, true);
                skin.transform.localPosition = Vector3.zero;
                skin.transform.localRotation = Quaternion.identity;
                skin.transform.localScale = Vector3.one;
            }

            TargetTailAnimator.DetachChildren = DetachForOptimization;
            TargetTailAnimator.User_SetTailTransforms(skin.bones.ToList());
            TargetTailAnimator.enabled = true;
        }
        else
        {
            if (AddTailAnimator)
            {
                TailAnimator2 tail = skin.gameObject.AddComponent<TailAnimator2>();
                tail.StartBone = skin.bones[0];
                tail.EndBone = skin.bones[skin.bones.Length - 1];
                tail.DetachChildren = DetachForOptimization;
                //tail.SyncWithAnimator = false;
            }
        }

    }
}
