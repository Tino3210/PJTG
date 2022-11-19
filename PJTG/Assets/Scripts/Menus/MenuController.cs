using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public Animator _transition;
    public float _transitionTime = 1f;

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
    
    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    public void GameOver() {
        StartCoroutine(LoadLevel(2)); 
    }

    public void QuitGame() {
        Application.Quit();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        _transition.SetTrigger("start");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
