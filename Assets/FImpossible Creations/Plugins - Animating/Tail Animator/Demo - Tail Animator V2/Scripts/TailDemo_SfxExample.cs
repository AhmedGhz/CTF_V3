using UnityEngine;
using FIMSpace.FTail;

/// <summary>
/// FC: Example of detecting collision states on tail animator segments for audio effects etc.
/// </summary>
public class TailDemo_SfxExample : MonoBehaviour
{
    public TailAnimator2[] toDetectCollisionsForSFX;
    public AudioClip clipToPlay;
    [Range(0f, 1f)]
    public float volume = 0.8f;
    public float toPlayAgainDelay = 0.1f;
    public int playUpToSegment = 0;

    private bool[] wasCollision;

    private float toPlayTimer = 0f;

    void Start()
    {
        wasCollision = new bool[toDetectCollisionsForSFX.Length];
        for (int i = 0; i < wasCollision.Length; i++) wasCollision[i] = false;
    }

    void Update()
    {
        toPlayTimer += Time.deltaTime;
        if (toPlayTimer < toPlayAgainDelay) return;

        for (int i = 0; i < toDetectCollisionsForSFX.Length; i++)
        {
            TailAnimator2 tail = toDetectCollisionsForSFX[i];
            bool stay = false;

            for (int s = 0; s < tail.TailSegments.Count; s++)
            {
                if (!wasCollision[i]) // Wasn't collision - let's detect enter
                {
                    if (tail.TailSegments[s].CollisionContactFlag) { stay = true; wasCollision[i] = true; OnTailCollisionEnterFirst(tail, s); }
                }
                else // Was collision - let's detect exit
                {
                    // If any segment have collision flag that means collision not ended
                    if (tail.TailSegments[s].CollisionContactFlag)
                    { OnTailCollisionStay(tail, s); stay = true; }
                }
            }

            // Was collision in last frame but now all segments are out
            if (wasCollision[i])
            {
                if (!stay)
                {
                    wasCollision[i] = false;
                    OnTailCollisionExitAll(tail);
                }
            }

        }

    }


    void OnTailCollisionEnterFirst(TailAnimator2 tail, int segment)
    {
        if (clipToPlay != null)
        {
            if (playUpToSegment > 0) if (segment > playUpToSegment) return;
            AudioSource.PlayClipAtPoint(clipToPlay, tail.TailSegments[segment].ProceduralPosition, volume);
            toPlayTimer = 0f;
        }
    }


    /// <summary> Only with 'perSegmentDetection </summary>
    void OnTailCollisionStay(TailAnimator2 tail, int segment)
    {

    }

    void OnTailCollisionExitAll(TailAnimator2 tail)
    {
    }
}