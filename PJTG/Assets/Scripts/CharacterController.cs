using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

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
    public AnimatorController _animator2;
    public AnimatorController _animator3;

    private Queue<GameObject> _enemiesQueue;
    private bool _isOnHit = false;
    private GameObject _currentEnemy;

    public ParticleSystem deathParticles;
    public Shake shakeCamera;

    public ParticleSystem playerDeathParticles;
    public GameObject deadHead;

    public SpriteRenderer[] bodyPieces;

    public GameObject[] arms0;
    public GameObject[] arms1;
    public GameObject[] arms2;

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
        Debug.Log("HandleEnemies queue : " + _enemiesQueue.Count);
        if(_enemiesQueue.Count <= 0) return;
        Debug.Log("HandleEnemies onHit : " + _isOnHit);
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
        Debug.Log("Health : " + _hp);        
        StartCoroutine(Blink());                
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
        Debug.Log("OnHitEnd : " + _isOnHit);
        if(!_isOnHit || _currentEnemy == null) return;
        Debug.Log("OnHitEnd");
        this.deathParticles.transform.position = _currentEnemy.transform.position;
        this.deathParticles.Play();
        StartCoroutine(this.shakeCamera.ShakeCamera(0.2f, 0.2f));
        Destroy(_currentEnemy);
        _currentEnemy = null;
        _isOnHit = false;
        ChangeState();        
    }

    private void EnableBodyParts(bool choice) {
        for(int i = 0 ; i < this.bodyPieces.Length; i++) {
            this.bodyPieces[i].enabled = choice;
        }
    }

    private IEnumerator Blink() { 
        this.EnableBodyParts(false);

        yield return new WaitForSeconds(0.08f);

        this.EnableBodyParts(true);        
    }

    private IEnumerator PlayerDeath() {
        this.playerDeathParticles.Play();
        GetComponent<CircleCollider2D>().enabled = false;
        this.bodyPieces[0].gameObject.SetActive(false);
        this.bodyPieces[1].gameObject.SetActive(false);
        this.bodyPieces[2].gameObject.SetActive(false);
        this.deadHead.SetActive(true);

        yield return null;
    }
}