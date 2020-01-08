using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    
    public GameObject boxPrefab;
    public float spawnTime;

    //Ref:
    ParticleSystem spawnParticle;

    void Start()
    {
        spawnParticle = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(SpawnBox());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    IEnumerator SpawnBox()
    {
        //Wait given time before doing anything:
        yield return new WaitForSeconds(spawnTime);

        //Start Particle Effect:
        spawnParticle.Play();
        yield return new WaitForSeconds(.2f);

        //Spawn Selection Box:
        Instantiate(boxPrefab, transform.position, Quaternion.identity);

        //Destroy This Spawner when particle effect is over:
        yield return new WaitUntil(() => !spawnParticle.isPlaying);
        Destroy(gameObject);
    }
}
