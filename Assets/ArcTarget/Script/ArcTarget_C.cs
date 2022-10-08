//This code can be used for private or commercial projects but cannot be sold or redestibuted without written permission.
//Copyright Nik W. Kraus / Dark Cube Entertainment LLC.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class ArcTarget_C : MonoBehaviour
{

    [Tooltip("Arc Start Point transform.")]
    public Transform StartPoint;

    [Tooltip("Enable Mouse as pointer, Camera center point is used, when off.")]
    public bool UseMousePos = true;

    [Tooltip("Arc move delay, drag effect (1=slow, 10=fast)")]
    public float ArcDelay = 1.0f;

    [Tooltip("Arc end target Decal delay, drag effect (1=slow, 10=fast)")]
    public float TargetDelay = 1.0f;

    [Tooltip("Max distance the Arc can hit a target.")]
    public float HitDistance = 200.0f;

    [Tooltip("End Decal offset from hit point Normal.")]
    public float DecalOffset = .5f;

    [Tooltip("Assign plane or spot light as target type.")]
    public GameObject TargetDecal;

    [Tooltip("Decal direction based on hit point normal (X Y Z only)")]
    public string TargetDirection = "X";

    [Tooltip("Arc Color")]
    public Color StartColor = new Color(0f, 1f, 1f, .5f);
    public Color EndColor = new Color(1f, 0f, 0f, .5f);

    [Tooltip("Number of sections in arc detail")]
    public int SectionDetail = 10;

    [Tooltip("Arc width")]
    public float StartSize = 1.0f;
    public float EndSize = 1.0f;

    [Tooltip("Arc hight in the world space")]
    public float ArcHeight = 10.0f;

    private Vector3 TargetDir;
    private LineRenderer lineRenderer;
    private Vector3 MousePos;
    private Ray ray = new Ray(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
    private RaycastHit hit;
    private Vector3 middle;
    private Vector3 end;
    private Quaternion rot;
    private bool TargetTypeLight = false;
    private Light TargetLight;
    private float g;
    private float dist;
    private int i;

    ///////////////////////Start
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer.material == null)
            lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

        if (TargetDecal)
        {
            if (TargetDecal.GetComponent<Light>())
            {
                TargetTypeLight = true;
                TargetLight = TargetDecal.GetComponent<Light>();
                TargetLight.color = EndColor;
            }
            else {
                TargetDecal.GetComponent<Renderer>().material.color = EndColor;
                TargetDecal.GetComponent<Renderer>().material.SetColor("_TintColor", EndColor);
            }
        }

        if (!TargetDecal)
            Debug.Log("Target object has not been assigned, Please drag item into TargetDecal field.");

        if (!StartPoint)
        {
            StartPoint = gameObject.transform;
            Debug.Log("Start object has not been assigned, Please drag item into Start field.");
        }

        if (TargetDirection == "x" || TargetDirection == "y" || TargetDirection == "z" || TargetDirection == "X" || TargetDirection == "Y" || TargetDirection == "Z")
        {
        }
        else {
            TargetDirection = "x";
            Debug.Log("Direction can only be X, Y or Z");
        }

    }//End Start


    //Update
    void Update()
    {
        lineRenderer.startWidth = StartSize;
        lineRenderer.endWidth = EndSize;

        lineRenderer.startColor = StartColor;
        lineRenderer.endColor = EndColor;
        lineRenderer.positionCount = SectionDetail;

        if (SectionDetail < 2)
        {
            SectionDetail = 2;
        }

        //Check Direction
        if (TargetDirection == "x" || TargetDirection == "X")
        {
            TargetDirection = "X";
            TargetDir = StartPoint.right;
        }
        else if (TargetDirection == "y" || TargetDirection == "Y")
        {
            TargetDirection = "Y";
            TargetDir = StartPoint.up;
        }
        else if (TargetDirection == "z" || TargetDirection == "Z")
        {
            TargetDirection = "Z";
            TargetDir = StartPoint.forward;
        }
        else {
            TargetDir = StartPoint.up;
        }

        if (UseMousePos)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        else {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        }

        // RayCast
        if (Physics.Raycast(ray, out hit, HitDistance)){
            //Debug.DrawLine (ray.origin, hit.point, Color.red);
            lineRenderer.enabled = true;

            //Check if Target Decal is Light
            if (TargetDecal)
            {
                if (TargetTypeLight)
                {
                    rot = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
                    TargetLight.gameObject.SetActive(true);
                    MousePos = hit.point;
                }
                else {
                    rot = Quaternion.FromToRotation(TargetDir, hit.normal);
                    TargetDecal.gameObject.SetActive(true);
                    MousePos = hit.point;
                }
                TargetDecal.transform.position = Vector3.Slerp(TargetDecal.transform.position, hit.point + hit.normal * DecalOffset, Time.deltaTime * TargetDelay);
                TargetDecal.transform.rotation = rot;

            }//End Target Check
        }
        else {
            if (TargetDecal)
            {
                if (TargetTypeLight)
                {
                    TargetDecal.gameObject.SetActive(false);
                    MousePos = StartPoint.position;
                }
                else {
                    TargetDecal.gameObject.SetActive(false);
                    MousePos = StartPoint.position;
                }

                TargetDecal.transform.position = StartPoint.position;
            }

            lineRenderer.enabled = false;

        }// End RayCast


        //Find middle pos
        dist = Vector3.Distance(StartPoint.position, MousePos);
        middle = Vector3.Lerp(StartPoint.position, MousePos, .5f) + new Vector3(0, ArcHeight * dist / 5, 0);
        end = Vector3.Slerp(end, MousePos, Time.deltaTime * ArcDelay);

        //Line Render Arc
        for (i = 0; i < SectionDetail * 1f; i++)
        {
            g = i / (SectionDetail * 1f - 1);
            lineRenderer.SetPosition(i, Mathf.Pow(1 - g, 2) * StartPoint.position + 2 * g * (1 - g) * middle + Mathf.Pow(g, 2) * end);
        }

    }//end Update


    /////Icon
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "ArcIcon.png", true);
    }

    void OnDrawGizmosSelected()
    {
        if (Physics.Raycast(ray, out hit, HitDistance))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
    }
}