using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;

public class EnemyController : MonoBehaviour {
    string[] PossibleQTE = {"S","D","J","K"};
    public GameObject TextPrefab;
    public float MoveSpeed = 2f;
    Vector3 PlayerPosition = new Vector3(0f,0f,0f);
    public bool QTEDone = false;
    public int Direction;
    public List<GameObject> Sequence;
    public GameObject Texts;
    int indexQTE = 0;
    public EnemyType Type;
    public int Difficulty;

    // Start is called before the first frame update
    void Start() {
        Sequence = new List<GameObject>();
        for(int i = 0; i < UnityEngine.Random.Range(Difficulty-2,Difficulty); i++){
            GameObject s = Instantiate(TextPrefab, new Vector3 (0,0,0), Quaternion.identity);
            s.GetComponent<TextMeshPro>().text = PossibleQTE[UnityEngine.Random.Range(0,8)];
            Sequence.Add(s);
            s.transform.SetParent(Texts.transform, false);
            s.transform.position = new Vector3(0,0,0);
            s.transform.localPosition = new Vector3(-0.22f+i*0.83f,-0.9f,0);
        if(Difficulty < 7 || Difficulty > 7){
            for(int i = 0; i < UnityEngine.Random.Range(Difficulty-2,Difficulty); i++){
                GameObject s = Instantiate(TextPrefab, new Vector3 (0,0,0), Quaternion.identity);
                s.GetComponent<TextMeshPro>().text = PossibleQTE[UnityEngine.Random.Range(0,PossibleQTE.Length)];
                Sequence.Add(s);
                s.transform.SetParent(Texts.transform, false);
                s.transform.position = new Vector3(0,0,0);
                s.transform.localPosition = new Vector3(-0.22f+i*0.5f,-0.9f,0);
            }
            Texts.transform.position = new Vector3(0,0,0);
            Texts.transform.localPosition = new Vector3((Sequence.Count-1)*-1f,-4f,0);
        }else{
            for(int i = 0; i < UnityEngine.Random.Range(1,Difficulty); i++){
                GameObject s = Instantiate(TextPrefab, new Vector3 (0,0,0), Quaternion.identity);
                s.GetComponent<TextMeshPro>().text = PossibleQTE[UnityEngine.Random.Range(0,PossibleQTE.Length)];
                Sequence.Add(s);
                s.transform.SetParent(Texts.transform, false);
                s.transform.position = new Vector3(0,0,0);
                s.transform.localPosition = new Vector3(-0.22f+i*0.5f,-0.9f,0);
            }
            Texts.transform.position = new Vector3(0,0,0);
            Texts.transform.localPosition = new Vector3((Sequence.Count-1)*-1f,-4f,0);
        }
        
    }

    // Update is called once per frame
    void Update() {
        //QTE
        bool hasPressed = false;
        if(!QTEDone){
            foreach(string pqte in PossibleQTE){
                if (Input.GetKeyDown(pqte.ToLower()))
                {
                    if(pqte.Equals(Sequence[indexQTE].GetComponent<TextMeshPro>().text)){
                        hasPressed = true;
                        Sequence[indexQTE].GetComponent<TextMeshPro>().color = new Color(0,255,0);
                        indexQTE++;
                        if(indexQTE == Sequence.Count){
                            QTEDone = true;
                            Texts.SetActive(false);
                            AudioController.instance.Play("QTEValid");
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
                            s.GetComponent<TextMeshPro>().color = new Color(0,0,0);
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

    public void ChangeEnemyType(EnemyType type){
        Type = type;
        string FilePath = "Sprites/Foods/"+Enum.GetName(typeof(EnemyType),type).ToUpper().Substring(0,1) + Enum.GetName(typeof(EnemyType),type).Substring(1).ToLower();
        Debug.Log(FilePath);
        Texture2D SpriteTexture = Resources.Load<Texture2D>(FilePath);
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height),new Vector2(0.5f,0.5f), 100.0f);
    }
}
