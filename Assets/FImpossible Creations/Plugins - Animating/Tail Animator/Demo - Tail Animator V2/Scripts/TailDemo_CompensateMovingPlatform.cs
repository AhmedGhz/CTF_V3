using FIMSpace.FTail;
using UnityEngine;

public class TailDemo_CompensateMovingPlatform : MonoBehaviour
{
    public TailAnimator2 tailAnimator;
    public Transform movingPlatform;
    [Range(0f, 1f)] public float LimitBlend = 1f;

    Vector3 prePos;

    private void Start()
    {
        prePos = movingPlatform.position;
    }

    void Update()
    {
        if (tailAnimator.IsInitialized == false) return;

        Vector3 platOffset = (movingPlatform.position - prePos) * LimitBlend * tailAnimator.MotionInfluence;

        for (int i = 0; i < tailAnimator.TailSegments.Count; i++)
        {
            tailAnimator.TailSegments[i].ProceduralPosition += platOffset;
            tailAnimator.TailSegments[i].PreviousPosition += platOffset;
        }

        tailAnimator.GetGhostChild().ProceduralPosition += platOffset;
        tailAnimator.GetGhostChild().PreviousPosition += platOffset;

        prePos = movingPlatform.position;
    }
}
