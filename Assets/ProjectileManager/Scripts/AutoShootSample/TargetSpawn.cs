using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawn : MonoBehaviour
{
    public GameObject target;
    public AutoShoot auto;

    Transform[] spawnPoints;
    GameObject targetIns;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }

        SpawnTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetIns == null)
        {
            SpawnTarget();
        }
    }

    Transform getSpawnPoint()
    {
        int ran = Random.Range(0, spawnPoints.Length);
        return spawnPoints[ran];
    }

    void SpawnTarget()
    {
        targetIns = Instantiate(target, getSpawnPoint());
        auto.target = targetIns;
    }
}
