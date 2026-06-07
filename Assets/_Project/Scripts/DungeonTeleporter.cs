using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonTeleporter : MonoBehaviour
{
    [Header("Configuración del Portal")]
    public string sceneToLoad = "Dungeon Scene";

    // Esta función la llama Unity automáticamente cuando un objeto físico entra en el portal
    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si el objeto que nos ha tocado tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Teletransportando a la escena: " + sceneToLoad + "!");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
