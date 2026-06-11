using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject treePrefab;
    public Terrain terrain;
    public int numberOfTrees = 200;
    
    [Tooltip("La altura Y del agua. Si el terreno está por debajo de esto, no planta árbol.")]
    public float waterLevel = 10f; 

    void Start()
    {
        SpawnTrees();
    }

    [ContextMenu("Generar Árboles Ahora (Solo en Editor)")]
    public void SpawnTrees()
    {
        if (terrain == null || treePrefab == null)
        {
            Debug.LogError("Falta asignar el terreno o el prefab del árbol.");
            return;
        }

        Vector3 terrainPos = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        int spawned = 0;
        int maxAttempts = numberOfTrees * 30; 
        int attempts = 0;

        while (spawned < numberOfTrees && attempts < maxAttempts)
        {
            attempts++;
            

            float randomX = Random.Range(0, terrainSize.x);
            float randomZ = Random.Range(0, terrainSize.z);

            Vector3 checkPos = new Vector3(randomX + terrainPos.x, 0, randomZ + terrainPos.z);
            

            float localHeight = terrain.SampleHeight(checkPos);
            float worldHeight = localHeight + terrainPos.y; 

            if (worldHeight >= waterLevel)
            {

                Vector3 treePos = new Vector3(checkPos.x, worldHeight, checkPos.z);
                
                Instantiate(treePrefab, treePos, Quaternion.Euler(0, Random.Range(0, 360f), 0), transform);
                spawned++;
            }
        }
        

        if (spawned == 0)
        {
            Debug.LogWarning("No se generó ningún árbol. El waterLevel (10) podría ser más alto que tus montañas.");
        }
        else
        {
            Debug.Log($"¡Éxito! Se han plantado {spawned} árboles en tierra firme.");
        }
    }
}