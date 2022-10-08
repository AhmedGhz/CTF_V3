using UnityEngine;

public class TailDemo_EnableOnPlayer : MonoBehaviour
{
    public MonoBehaviour ToEnable;
    public bool putPlayerAsChild = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ToEnable.enabled = true;
            if (putPlayerAsChild) collision.transform.SetParent(transform, true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ToEnable.enabled = false;
            if (putPlayerAsChild) collision.transform.SetParent(null, true);
        }
    }
}
