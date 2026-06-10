using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneScript : MonoBehaviour
{
    public string sceneToPut = "Dungeon Scene";
    public TextMeshProUGUI loadingText;

    [Header("Ajustes de Carga")]
    [Tooltip("Tiempo mínimo en segundos que durará la pantalla de carga (para que quede profesional)")]
    public float minimumLoadingTime = 3.5f;

    void Start()
    {
    
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToPut);

        asyncLoad.allowSceneActivation = false;

        float timer = 0f;


        while (asyncLoad.progress < 0.9f || timer < minimumLoadingTime)
        {

            timer += Time.deltaTime;


            int dots = Mathf.FloorToInt(timer * 2f) % 4;

            if (dots == 0) loadingText.text = "Loading";
            else if (dots == 1) loadingText.text = "Loading.";
            else if (dots == 2) loadingText.text = "Loading..";
            else if (dots == 3) loadingText.text = "Loading...";

            yield return null; 
        }

        yield return new WaitForSeconds(0.5f); 


        asyncLoad.allowSceneActivation = true;
    }
}
