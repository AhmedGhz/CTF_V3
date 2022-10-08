using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileManager;

public class AutoShoot : MonoBehaviour
{
    public GameObject target;
    public GameObject projectile;
    public ProjectileThrow shootPoint;
    public float limit = 2f;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= limit && target != null)
        {
            timer = 0f;
            shootPoint.ShootAtObject(projectile, target.transform.position);
            
        }
    }
}
