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
    public int ndrops = 14;
    public Gradient DropGradient;
    public float colorSpeed = 1;
    public Transform Box;
     // Variable para controlar la velocidad de spawn
    public float spawnInterval = 0.1f; // Tiempo en segundos entre cada spawneo
    private float timeSinceLastSpawn = 0f;

    public float rotateSpeed = 10f;
    private float currentRotation = 0f;


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
        
        // Rotar el punto de origen
        RotateFirePoint();
    }

    private void SpawnRain()
    {
        float angleStep = 360f / ndrops;

        for (int i = 0; i < ndrops; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad + currentRotation * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            Vector3 pos = FirePoint.position + direction;

            if(Random.value < fillProbability)
                SpawnDrop(pos, direction);
        }
    }


    void SpawnDrop(Vector3 pos, Vector3 direction)
    {
        Drop go = Instantiate(Prefab, pos, Quaternion.identity);
        
        Color color = new Color(Random.value, Random.value, Random.value);

        float rdm = Mathf.Sin(Time.time * colorSpeed)/2+0.5f;
        color = DropGradient.Evaluate(rdm);
        //color = new Color(rdm, rdm, rdm);
        go.GetComponent<MeshRenderer>().material.color = color;


        go.Init(FireForce, direction);
        go.transform.SetParent(Box);
    }

    
    void RotateFirePoint()
    {
        currentRotation += rotateSpeed * Time.deltaTime;
        FirePoint.rotation = Quaternion.Euler(0f, currentRotation, 0f);
    }
}
