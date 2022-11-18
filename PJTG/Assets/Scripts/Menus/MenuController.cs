using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
    
    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
