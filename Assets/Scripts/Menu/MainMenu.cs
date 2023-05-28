using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string startGameScene;
    public GameObject aboutScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(startGameScene);
    }

    public void OpenCloseAbout()
        {
            aboutScreen.SetActive(!aboutScreen.activeSelf);
        }
    
    public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Game has been closed");
                
        }
}
