using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    bool spawnEnemy = true;
    public GameObject enemyPrefab;
    public GameObject[] spawnPoints;
    float radius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        while(spawnEnemy){
            int index = Random.Range(0, 4);
            Vector3 randomInsideUnitCircle = new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0);
            Vector3 randomPoint = spawnPoints[index].transform.position + randomInsideUnitCircle * radius * 0.5f;
            Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
            yield return new WaitForSeconds(1f);
        }
    }
}
