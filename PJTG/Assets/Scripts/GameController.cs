using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    // Timer management
    private float chrono = 0f;
    private bool chronoIsUp = false;
    public TextMeshProUGUI timerText;

    // Score management
    private int score = 0;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    private void Start() {
        this.chronoIsUp = true;
    }

    // Update is called once per frame
    private void Update() {
        if(chronoIsUp) {
            chrono += Time.deltaTime;            
            DisplayTime(chrono);
        }
    }

    private void DisplayTime(float timeToDisplay) {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddScore(int score) {
        this.score += score;
        scoreText.text = "Score : " + this.score;
    }
}
