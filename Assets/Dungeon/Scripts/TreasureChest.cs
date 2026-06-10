using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureChest : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("O nome exacto da escena de vitoria")]
    public string rewardSceneName = "RewardScene";

    private bool alreadyOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        // Cando o xogador choca co cofre...
        if (other.CompareTag("Player") && !alreadyOpened)
        {
            alreadyOpened = true; 
            Debug.Log("Cargando escena de recompensa...");
            
            // Saltamos á pantalla final!
            SceneManager.LoadScene(rewardSceneName);
        }
    }
}
