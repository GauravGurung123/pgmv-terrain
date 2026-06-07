using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Archivo de Configuración")]
    // Aquí arrastraremos tu archivo MyDungeonConfig
    public DungeonConfig config; 

    [Header("Textos de la Interfaz (Visuales)")]
    public TextMeshProUGUI sizeNumberText;
    public TextMeshProUGUI probabiltyANumberText; 
    public TextMeshProUGUI probabiltyBNumberText; 

    [Header("Sliders de la Interfaz")]
    public Slider sizeSlider; // Añadimos este para leer el tamaño final
    public Slider probabilityASlider;
    public Slider probabilityBSlider;

    [Header("Campos de Entrada (Input Fields)")]
    // Añade los campos para la semilla y los límites de las salas
    public TMP_InputField seedInputField;
    public TMP_InputField minRoomInputField;
    public TMP_InputField maxRoomInputField;

    [Header("Configuración de Escena")]
    public string sceneToLoad = "MainScene";

    void Start()
    {
        // Forzamos que al arrancar estén a 50-50 para que quede bien de inicio
        if (probabilityASlider != null && probabilityBSlider != null)
        {
            probabilityASlider.value = 50;
            probabilityBSlider.value = 50;
        }
    }

    public void UpdateSizeText(float value)
    {
        sizeNumberText.text = value.ToString(); 
    }

    public void UpdateProbabilityAText(float value)
    {
        probabiltyANumberText.text = (value.ToString() + "%"); 
        
        if (probabilityBSlider != null)
        {
            float opuesto = 100f - value;
            probabilityBSlider.SetValueWithoutNotify(opuesto);
            probabiltyBNumberText.text = (opuesto.ToString() + "%");
        }
    }

    public void UpdateProbabilityBText(float value)
    {
        probabiltyBNumberText.text = (value.ToString() + "%"); 
        
        if (probabilityASlider != null)
        {
            float opuesto = 100f - value;
            probabilityASlider.SetValueWithoutNotify(opuesto);
            probabiltyANumberText.text = (opuesto.ToString() + "%");
        }
    }

    public void startAdventure()
    {

        if (config != null)
        {

            if (seedInputField != null && !string.IsNullOrEmpty(seedInputField.text))
            {
                if (int.TryParse(seedInputField.text, out int resultadoSeed))
                    config.seed = resultadoSeed;
            }

            if (sizeSlider != null)
            {
                config.numberOfRooms = (int)sizeSlider.value;
            }

            if (minRoomInputField != null && !string.IsNullOrEmpty(minRoomInputField.text))
            {
                if (int.TryParse(minRoomInputField.text, out int resultadoMin))
                    config.minRoomSize = resultadoMin;
            }

            if (maxRoomInputField != null && !string.IsNullOrEmpty(maxRoomInputField.text))
            {
                if (int.TryParse(maxRoomInputField.text, out int resultadoMax))
                    config.maxRoomSize = resultadoMax;
            }

            if (probabilityASlider != null && probabilityBSlider != null)
            {
                config.roomTypeA_Prob = probabilityASlider.value / 100f;
                config.roomTypeB_Prob = probabilityBSlider.value / 100f;
            }
            
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(config);
            #endif
        }

        // Cargamos la escena de la isla
        SceneManager.LoadScene(sceneToLoad);
    }
}