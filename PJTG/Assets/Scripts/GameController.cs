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

    bool spawnEnemy = true;
    public GameObject enemyPrefab;
    public GameObject[] spawnPoints;
    float radius = 5f;

    // Start is called before the first frame update
    private void Start() {
        this.chronoIsUp = true;
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    private void Update() {
        if(chronoIsUp) {
            chrono += Time.deltaTime;            
            DisplayTime(chrono);
        }
    }

    // Display the timer as minutes and seconds
    private void DisplayTime(float timeToDisplay) {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Add the score on the user's score
    public void AddScore(int score) {
        this.score += score;
        scoreText.text = "Score : " + this.score;
    }

    IEnumerator SpawnEnemy()
    {
        while(spawnEnemy){
            int index = Random.Range(0, 4);
            Vector3 randomInsideUnitCircle = new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0);
            Vector3 randomPoint = spawnPoints[index].transform.position + randomInsideUnitCircle * radius * 0.5f;
            Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }
}
