using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Drop Prefab;
    public Transform FirePoint;

    public float FireForce = 10;

    public float fillProbability = 0.9f;

    public int ndrops = 5;

    public Gradient DropGradient;
    public float colorSpeed = 1;

    public Transform Box;

     // Variable para controlar la velocidad de spawn
    public float spawnInterval = 0.1f; // Tiempo en segundos entre cada spawneo

    private float timeSinceLastSpawn = 0f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizar el tiempo transcurrido desde el último spawneo
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnRain();
            timeSinceLastSpawn = 0f; // Reiniciar el contador de tiempo
        }
    }

    private void SpawnRain()
    {
         for (int j = 0; j < ndrops; j++)
        {
            
            for (int i = 0; i < ndrops; i++)
            {
                Vector3 pos = FirePoint.position;

                if(Random.value < fillProbability)
                    SpawnDrop(pos);
            }
        }
    }


    void SpawnDrop(Vector3 pos)
    {
        Drop go = Instantiate(Prefab, FirePoint.position, FirePoint.rotation);
        
        Color color = new Color(Random.value, Random.value, Random.value);

        float rdm = Mathf.Sin(Time.time * colorSpeed)/2+0.5f;
        color = DropGradient.Evaluate(rdm);
        //color = new Color(rdm, rdm, rdm);
        go.GetComponent<MeshRenderer>().material.color = color;


        go.Init(FireForce);
        go.transform.SetParent(Box);
    }
}
