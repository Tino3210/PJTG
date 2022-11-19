using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float MoveSpeed = 3f;
    Vector3 PlayerPosition = new Vector3(0f,0f,0f);
    public bool QTEDone = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 displacement = PlayerPosition - transform.position;

        displacement = displacement.normalized;

        if (Vector2.Distance (PlayerPosition, transform.position) > 1.0f) {
            transform.position += (displacement * MoveSpeed * Time.deltaTime);
        } else {
            //do whatever the enemy has to do with the player
            Destroy(gameObject, 1f);
        }
    }
}
