using UnityEngine;

public class WaterZone : MonoBehaviour
{
    [Header("Punto de Reaparición")]
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        // Si lo que toca el agua tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Conseguimos el CharacterController del jugador
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null && spawnPoint != null)
            {
                // ¡CUIDADO! En Unity, para mover a un personaje con CharacterController,
                // hay que apagarlo un milisegundo antes de cambiar su posición,
                // si no, el motor de físicas ignora el teletransporte.
                cc.enabled = false; 
                
                // Movemos al jugador al punto de spawn
                other.transform.position = spawnPoint.position;
                
                // Volvemos a encender el componente
                cc.enabled = true;

                Debug.Log("¡Hacks detectados! El jugador tocó el agua y volvió al spawn.");
            }
        }
    }
}