using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab;
    public float spawnTime;

    void Start()
    {
        StartCoroutine(SpawnBox());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    IEnumerator SpawnBox()
    {
        //Wait given time before doing anything:
        yield return new WaitForSeconds(spawnTime);

        //Start Particle Effect:


        //Spawn Selection Box:
        Instantiate(boxPrefab, transform.position, Quaternion.identity);

        //Destroy This Spawner:
        Destroy(gameObject);
    }
}
