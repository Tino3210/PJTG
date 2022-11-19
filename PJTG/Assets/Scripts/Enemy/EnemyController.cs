using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour {
    string[] PossibleQTE = {"Q","D","X","F","U","L","P","B"};
    public GameObject TextPrefab;
    public float MoveSpeed = 3f;
    Vector3 PlayerPosition = new Vector3(0f,0f,0f);
    public bool QTEDone = false;
    public int Direction;
    public List<GameObject> Sequence;
    public GameObject Texts;
    int indexQTE = 0;
    public EnemyType Type;

    // Start is called before the first frame update
    void Start() {   
        Sequence = new List<GameObject>();
        for(int i = 0; i<Random.Range(1,6); i++){
            GameObject s = Instantiate(TextPrefab, new Vector3 (0,0,0), Quaternion.identity);
            s.GetComponent<TextMeshPro>().text = PossibleQTE[Random.Range(0,8)];
            Sequence.Add(s);
            s.transform.SetParent(Texts.transform, false);
            s.transform.localPosition = new Vector3(-0.22f+i*0.5f,-0.9f,0);
        }
        Texts.transform.position = new Vector3(0,0,0);
        Texts.transform.localPosition = new Vector3((Sequence.Count-1)*-0.25f,-0.2f,0);
    }

    // Update is called once per frame
    void Update() {
        //QTE
        bool hasPressed = false;
        if(!QTEDone){
            foreach(string pqte in PossibleQTE){
                if (Input.GetKeyDown(pqte.ToLower()))
                {
                    hasPressed = true;
                    if(pqte.Equals(Sequence[indexQTE].GetComponent<TextMeshPro>().text)){
                        Sequence[indexQTE].GetComponent<TextMeshPro>().color = new Color(0,255,0);
                        indexQTE++;
                        if(indexQTE == Sequence.Count){
                            QTEDone = true;
                        }
                    }
                }
            }
            if(!hasPressed){
                if (Input.anyKeyDown)
                {
                    if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2)) {
                    } else {
                        foreach(GameObject s in Sequence){
                            s.GetComponent<TextMeshPro>().color = new Color(255,255,255);
                            indexQTE = 0;
                        }
                    }
                }
            }
        }

        if(!GameObject.Find("GameController").GetComponent<GameController>().isDead) {
            //Enemy displacement
            Vector3 displacement = PlayerPosition - transform.position;

            displacement = displacement.normalized;

            if (Vector2.Distance (PlayerPosition, transform.position) > 1.0f) {
                transform.position += (displacement * MoveSpeed * Time.deltaTime);
            } else {
                //do whatever the enemy has to do with the player
                //Destroy(gameObject, 1f);
            }
        }
    }
}
