using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScreenScript : MonoBehaviour
{
    public string sceneToLoad = "LoadingScene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

}
