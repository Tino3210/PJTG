using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public Animator _transition;
    public float _transitionTime = 1f;

    public void MainMenu() {
        AudioController.instance.Play("ButtonClick");
        SceneManager.LoadScene(0);
    }
    
    public void PlayGame() {
        AudioController.instance.Play("ButtonClick");
        SceneManager.LoadScene(1);
    }

    public void GameOver() {
        StartCoroutine(LoadLevel(2)); 
    }

    public void QuitGame() {
        AudioController.instance.Play("ButtonClick");
        Application.Quit();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(2);

        _transition.SetTrigger("start");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
