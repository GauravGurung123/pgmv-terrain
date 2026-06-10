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

    [Header("UI Feedback")]
    public GameObject errorIcon;

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
        // 1. Apagamos o icono por defecto cada vez que preme o botón
        if (errorIcon != null) errorIcon.SetActive(false);

        // 2. Comprobamos se algún campo vital está baleiro
        if (string.IsNullOrEmpty(seedInputField.text) || 
            string.IsNullOrEmpty(minRoomInputField.text) || 
            string.IsNullOrEmpty(maxRoomInputField.text))
        {
            if (errorIcon != null) errorIcon.SetActive(true);
            return; 
        }

        
        int resultadoMin = int.Parse(minRoomInputField.text);
        int resultadoMax = int.Parse(maxRoomInputField.text);

        
        if (resultadoMin > resultadoMax)
        {
            if (errorIcon != null) errorIcon.SetActive(true);
            return; 
        }

     

        if (config != null)
        {
            config.seed = int.Parse(seedInputField.text);
            config.numberOfRooms = (int)sizeSlider.value;
            config.minRoomSize = resultadoMin;
            config.maxRoomSize = resultadoMax;
            config.roomTypeA_Prob = probabilityASlider.value / 100f;
            config.roomTypeB_Prob = probabilityBSlider.value / 100f;
            
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(config);
            #endif
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}