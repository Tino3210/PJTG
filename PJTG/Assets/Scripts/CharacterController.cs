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

    public SpriteRenderer[] bodyPieces;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
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
        if(_hp <= 0) Death();
    }

    public void HitAnimation(int direction)
    {
        switch (direction)
        {
            case 2:
                _animator.SetTrigger("onHitUp");
            break;
            case 3:
                _animator.SetTrigger("onHitDown");
            break;
            case 0:
                _animator.SetTrigger("onHitLeft");
            break;
            case 1:
                _animator.SetTrigger("onHitRight");
            break;
            default:

            break;
        }
    }

    public void Death()
    {
        _animator.SetBool("isDeath", true);
        GameObject.Find("GameController").GetComponent<GameController>().isDead = true;
        GameObject.Find("GameController").GetComponent<GameController>().spawnEnemy = false;
        StartCoroutine(PlayerDeath());
    }

    public void OnHitEnd()
    {
        if(!_isOnHit || !_currentEnemy) return;
        this.deathParticles.transform.position = _currentEnemy.transform.position;
        this.deathParticles.Play();
        StartCoroutine(this.shakeCamera.ShakeCamera(0.2f, 0.2f));
        Destroy(_currentEnemy);
        _currentEnemy = null;
        _isOnHit = false;        
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

        OnHitEnd();
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