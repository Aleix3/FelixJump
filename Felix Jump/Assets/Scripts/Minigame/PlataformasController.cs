using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaController : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject metaFinal;
    public GameObject plataforma1Hole;
    public GameObject plataforma2Hole;
    public GameObject ballPrefab;
    public GameObject plataformaPinchos;

    private GameObject plataformaToSpawn;

    [Header("Generador")]
    public int numeroPlataforma = 0;
    public float distanciaSpawn = 5f;
    public float startingY = 0;

    private float lastOrientationY = 0;

    // Radio para la ubicación de plataformaPinchos en el borde de la plataforma
    public float radioPlataforma = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numeroPlataforma; i++)
        {
            plataformaToSpawn = (Random.Range(0, 2) == 0) ? plataforma1Hole : plataforma2Hole;

            float rotationY = Random.Range(45, 315);
            while (Mathf.Abs(rotationY - lastOrientationY) < 45)
            {
                rotationY = Random.Range(45, 315);
            }
            lastOrientationY = rotationY;

            GameObject nuevaPlataforma = Instantiate(plataformaToSpawn, new Vector3(0, startingY + (distanciaSpawn * (i + 1)), 0), Quaternion.Euler(0, rotationY, 0), CylinderController.instance.cylinder.transform);

            // Añadir el prefab plataformaPinchos en una posición aleatoria dentro de la nueva plataforma
            SpawnPlataformaPinchos(nuevaPlataforma);
        }

        Instantiate(metaFinal, new Vector3(0, startingY + (distanciaSpawn * (numeroPlataforma + 1)), 0), Quaternion.identity, CylinderController.instance.cylinder.transform);

        // Pelota
        GameObject ball = Instantiate(ballPrefab, new Vector3(0, 0.4f, -0.7f), Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void SpawnPlataformaPinchos(GameObject plataforma)
    {
        // Elegir un ángulo aleatorio para la posición de plataformaPinchos
        float anguloAleatorio = Random.Range(0f, 360f);
        float radianes = anguloAleatorio * Mathf.Deg2Rad;

        // Calcular posición en el borde de la plataforma
        Vector3 posicionPinchos = new Vector3(Mathf.Cos(radianes) * radioPlataforma, 0, Mathf.Sin(radianes) * radioPlataforma);

        // Instanciar plataformaPinchos como hijo de la plataforma con la rotación adecuada
        GameObject pinchos = Instantiate(plataformaPinchos, plataforma.transform.position + posicionPinchos, Quaternion.identity, plataforma.transform);

        // Hacer que plataformaPinchos mire hacia el centro de la plataforma
        pinchos.transform.LookAt(plataforma.transform.position);
        pinchos.transform.Rotate(90, 0, 0); // Ajustar si es necesario para alinearlo correctamente
    }
}
