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

    // Enemies management
    private bool spawnEnemy = true;
    public GameObject enemyPrefab;
    public GameObject[] spawnPoints;
    private float radius = 5f;

    // Wave management
    private Wave currentWave;
    private Dictionary<int, Wave> waves;
    
    // Start is called before the first frame update
    private void Start() {
        // Timer
        this.chronoIsUp = true;

        // Waves initialization
        this.waves = new Dictionary<int, Wave>();
        // First wave
        Wave wave0 = new Wave(0, new[] {EnemyType.BURGER, EnemyType.FRIES}, false);
        Wave wave1 = new Wave(30, new[] {EnemyType.FRIES, EnemyType.SANDWICH}, false);
        Wave wave2 = new Wave(60, new[] {EnemyType.SANDWICH}, true);
        this.waves[wave0.TimeStamp] = wave0;
        this.waves[wave1.TimeStamp] = wave1;
        this.waves[wave2.TimeStamp] = wave2;

        this.currentWave = this.waves[0];

        // Enemy spawn
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    private void Update() {
        if(chronoIsUp) {
            int timeIndex = System.Convert.ToInt32(chrono);
            // Start the wave according to its timestamp
            if(this.waves.ContainsKey(timeIndex)) {
                if(this.currentWave != waves[timeIndex]) {
                    this.currentWave = waves[timeIndex];
                }
            }
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

    IEnumerator SpawnEnemy() {
        // As long as the enemies can spawn
        while(spawnEnemy) {
            // EnemyType is chosen
            int random = Random.Range(0, this.currentWave.MaxPercentage);
            EnemyType type = this.currentWave.Enemies[this.currentWave.GetEnemyType(random)];

            // Is the new enemy a boss
            if(this.currentWave.IsBossWave && !this.currentWave.HasBossSpawned) {
                int randomBoss = Random.Range(0, 11);
                if(randomBoss == 0) {
                    this.currentWave.HasBossSpawned = true;
                }
            }

            // Spawn the enemy
            int index = Random.Range(0, 4);
            Vector3 randomInsideUnitCircle = new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0);
            Vector3 randomPoint = spawnPoints[index].transform.position + randomInsideUnitCircle * radius * 0.5f;
            Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }
}
