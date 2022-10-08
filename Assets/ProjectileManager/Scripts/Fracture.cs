using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    public GameObject fracturedObject;
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(gameObject, 0f);
            Instantiate(fracturedObject, transform.position, Quaternion.identity);
        }
    }
}
