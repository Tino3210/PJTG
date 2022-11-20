using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    public bool IsEnd = false;

    // Timer management
    public static float chrono = 0f;
    private bool chronoIsUp = false;
    public TextMeshProUGUI timerText;

    // Score management
    public static int score = 0;
    public TextMeshProUGUI scoreText;

    // Enemies management
    public bool spawnEnemy = true;
    public GameObject enemyPrefab;
    public GameObject[] spawnPoints;
    private float radius = 1f;

    // Wave management
    private Wave currentWave;
    private Dictionary<int, Wave> waves;

    private int Difficulty = 3;

    GameObject boss;
    public bool isDead = false;
    
    // Start is called before the first frame update
    private void Start() {
        if(IsEnd) return;
        // Timer
        this.chronoIsUp = true;
        GameController.chrono = 0;

        // Waves initialization
        this.waves = new Dictionary<int, Wave>();
        // First wave
        Wave wave0 = new Wave(0, new[] {EnemyType.HAMBURGER1, EnemyType.KEBAB1, EnemyType.HAMBURGER3}, false);
        Wave wave1 = new Wave(10, new[] {EnemyType.HAMBURGER2, EnemyType.KEBAB2}, false);
        Wave wave2 = new Wave(20, new[] {EnemyType.HUILE, EnemyType.PIZZA}, false);
        Wave wave3 = new Wave(30, new[] {EnemyType.HUILE}, true);
        Wave wave4 = new Wave(60, new[] {EnemyType.HAMBURGER1, EnemyType.HAMBURGER2, EnemyType.HAMBURGER3, EnemyType.KEBAB1, EnemyType.KEBAB2, EnemyType.HUILE, EnemyType.PIZZA}, false);
        this.waves[wave0.TimeStamp] = wave0;
        this.waves[wave1.TimeStamp] = wave1;
        this.waves[wave2.TimeStamp] = wave2;
        this.waves[wave3.TimeStamp] = wave3;
        this.waves[wave4.TimeStamp] = wave4;

        this.currentWave = this.waves[0];

        // Enemy spawn
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    private void Update() {
		if(IsEnd) return;
        if(currentWave.IsBossWave && boss == null && currentWave.HasBossSpawned){
            currentWave.IsBossWave = false;
        }        
        if(chronoIsUp) {
            int timeIndex = System.Convert.ToInt32(chrono);
            // Start the wave according to its timestamp
            if(this.waves.ContainsKey(timeIndex)) {
                if(this.currentWave != waves[timeIndex]) {
                    this.currentWave = waves[timeIndex];
                    Difficulty++;
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
        GameController.score += score;
        scoreText.text = "Score : " + GameController.score;
    }

    IEnumerator SpawnEnemy() {
        // As long as the enemies can spawn
        while(spawnEnemy) {
            // EnemyType is chosen
            int random = Random.Range(0, this.currentWave.MaxPercentage);
            EnemyType type = this.currentWave.Enemies[this.currentWave.GetEnemyType(random)];

            // Is the new enemy a boss
            // if(this.currentWave.IsBossWave && !this.currentWave.HasBossSpawned) {
            //     int randomBoss = Random.Range(0, 11);
            //     if(randomBoss == 0) {
            //         this.currentWave.HasBossSpawned = true;
            //     }
            // }

            // Spawn the enemy
            int index = Random.Range(0, 4);
            Vector3 randomInsideUnitCircle = new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0);
            Vector3 randomPoint = spawnPoints[index].transform.position + randomInsideUnitCircle * radius * 0.5f;
            GameObject enemy = Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
            enemy.GetComponent<EnemyController>().ChangeEnemyType(type);
            enemy.GetComponent<EnemyController>().Direction = index;
            if(currentWave.IsBossWave && !currentWave.HasBossSpawned){
                boss = enemy;
                currentWave.HasBossSpawned = true;
                enemy.GetComponent<EnemyController>().Difficulty = 20;
                enemy.GetComponent<EnemyController>().MoveSpeed = 0.5f;
            }else if(currentWave.IsBossWave && currentWave.HasBossSpawned){
                Destroy(enemy);
            }else{
                enemy.GetComponent<EnemyController>().Difficulty = Difficulty;
                enemy.GetComponent<EnemyController>().MoveSpeed = Difficulty/2;
            }
            switch(Difficulty){
                case 3:
                    yield return new WaitForSeconds(1f);
                    break;
                case 4:
                    yield return new WaitForSeconds(2f);
                    break;
                case 5:
                    yield return new WaitForSeconds(2f);
                    break;
                case 6:
                    yield return new WaitForSeconds(2f);
                    break;
                case 7:
                    yield return new WaitForSeconds(2f);
                    break;
                default:
                    yield return new WaitForSeconds(2f);
                    break;
            }
        }
    }
}
