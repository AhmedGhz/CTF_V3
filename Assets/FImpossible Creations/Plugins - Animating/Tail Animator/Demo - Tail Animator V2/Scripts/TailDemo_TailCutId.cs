using UnityEngine;

public class TailDemo_TailCutId : MonoBehaviour
{
    public int index;
    public TailDemo_SegmentedTailGenerator owner;

    void OnMouseDown()
    { 
        if (owner)
        {
            owner.ExmapleCutAt(index);
            Destroy(this);
        }
    }
}
