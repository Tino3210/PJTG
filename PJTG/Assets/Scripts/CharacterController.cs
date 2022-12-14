using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAnimation
{
    Up,
    Down,
    Left,
    Right,
}

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private int _hp = 3;
    private Animator _animator;

    private Queue<GameObject> _enemiesQueue;
    private bool _isOnHit = false;
    private GameObject _currentEnemy;

    public ParticleSystem deathParticles;
    public Shake shakeCamera;

    public ParticleSystem playerDeathParticles;
    public GameObject deadHead;

    public SpriteRenderer[] bodyPieces1;
    public SpriteRenderer[] bodyPieces2;
    public SpriteRenderer[] bodyPieces3;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;

    private GameObject currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = Player1;
        _enemiesQueue = new Queue<GameObject>();        
    }

    // Update is called once per frame
    void Update()
    {
        HandleEnemies();
    }

    private void HandleEnemies()
    {   
        if(_enemiesQueue.Count <= 0) return;
        if(_isOnHit) return;
        var enemie = _enemiesQueue.Dequeue();
        if(!enemie) return;
        var enemieEC = enemie.GetComponent<EnemyController>();        
        _currentEnemy = enemie;
        _isOnHit = true;

        if(enemieEC.QTEDone){                                                          
            HitAnimation(enemieEC.Direction);
            GameObject.Find("GameController").GetComponent<GameController>().AddScore(20);
        }else{            
            Hit();
        }        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {        
        if (collider.gameObject.tag == "Enemy")
        {
            _enemiesQueue.Enqueue(collider.gameObject);
        }     
    }
    
    public void Hit()
    {        
        _hp -= 1;
        StartCoroutine(Blink());
        AudioController.instance.Play("PlayerHit");
        if(_hp <= 0) Death();
        OnHitEnd();
    }

    private void ChangeState()
    {
        if(_hp == 2){
            currentPlayer.SetActive(false);
            currentPlayer = Player2;
            currentPlayer.SetActive(true);
        }else if(_hp == 1){
            currentPlayer.SetActive(false);
            currentPlayer = Player3;
            currentPlayer.SetActive(true);
        }
    }

    public void HitAnimation(int direction)
    {
        switch (direction)
        {
            case 2:
                currentPlayer.GetComponent<Animator>().SetTrigger("onHitUp");
            break;
            case 3:
                currentPlayer.GetComponent<Animator>().SetTrigger("onHitDown");
            break;
            case 0:
                currentPlayer.GetComponent<Animator>().SetTrigger("onHitLeft");
            break;
            case 1:
                currentPlayer.GetComponent<Animator>().SetTrigger("onHitRight");
            break;
            default:
                
            break;
        }
        AudioController.instance.Play("PlayerPunch");
    }

    public void Death()
    {
        currentPlayer.GetComponent<Animator>().SetBool("isDeath", true);
        GameObject.Find("GameController").GetComponent<GameController>().isDead = true;
        GameObject.Find("GameController").GetComponent<GameController>().spawnEnemy = false;
        StartCoroutine(PlayerDeath());
        GameObject.Find("GameCanvas").GetComponent<MenuController>().GameOver();
    }

    public void OnHitEnd()
    {
        if(!_isOnHit || _currentEnemy == null) return;
        this.deathParticles.transform.position = _currentEnemy.transform.position;
        this.deathParticles.Play();
        StartCoroutine(this.shakeCamera.ShakeCamera(0.2f, 0.2f));
        AudioController.instance.Play("MonsterDeath");
        Destroy(_currentEnemy);
        _currentEnemy = null;
        _isOnHit = false;
        ChangeState();        
    }

    private void EnableBodyParts(bool choice) {                
        if(_hp == 3){
            for(int i = 0 ; i < this.bodyPieces1.Length; i++) {            
                this.bodyPieces1[i].enabled = choice;
            }
            }else if(_hp == 2){
                for(int i = 0 ; i < this.bodyPieces2.Length; i++) {            
                    this.bodyPieces2[i].enabled = choice;
                }
            }else{
                for(int i = 0 ; i < this.bodyPieces3.Length; i++) {            
                     this.bodyPieces3[i].enabled = choice;
                }
            }
    }

    private IEnumerator Blink() { 
        this.EnableBodyParts(false);

        yield return new WaitForSeconds(0.08f);

        this.EnableBodyParts(true);        
    }

    private IEnumerator PlayerDeath() {
        currentPlayer.SetActive(false);
        this.playerDeathParticles.Play();
        AudioController.instance.Play("PlayerDeath");
        GetComponent<CircleCollider2D>().enabled = false;    
        this.bodyPieces3[0].gameObject.SetActive(false);
        this.bodyPieces3[1].gameObject.SetActive(false);
        this.bodyPieces3[2].gameObject.SetActive(false);
        this.deadHead.SetActive(true);

        yield return null;
    }
}