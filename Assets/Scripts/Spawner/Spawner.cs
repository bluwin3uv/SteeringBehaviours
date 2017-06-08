using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject prefab;
    public float spwanRate = 10f;
    [HideInInspector]
    public List<GameObject> objects = new List<GameObject>();
    private float spawnTimer = 0f;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
	
    Vector3 GererateRandomPoint()
    {
        // set Halfscale to half of transforms scale
        Vector3 haflScale = transform.localScale  / 2;
        // set randompoint to zero
        Vector3 randomPoint = Vector3.zero;
        // set random point x, y , z to random range between
        randomPoint.x = Random.Range(-haflScale.x, haflScale.x);
        randomPoint.y = transform.position.y;
        randomPoint.z = Random.Range(-randomPoint.z, randomPoint.z);
        // -halfscale to halfscale

        return randomPoint;
    }

    public void Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject clone = Instantiate(prefab);
        objects.Add(clone);
        clone.transform.position = transform.position + position;
        clone.transform.rotation = transform.rotation;
    }
     

	void Update ()
    {
        spawnTimer = spawnTimer + Time.deltaTime;
        if(spawnTimer > spwanRate)
        {
            Vector3 randomPoint = GererateRandomPoint();
            Spawn(randomPoint, transform.rotation);
            spawnTimer = 0;
        }
	}
}
