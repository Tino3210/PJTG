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
            GameObject.Find("GameController").GetComponent<GameController>().AddScore(20);
        }else{            
            Hit();
            HitAnimation(enemieEC.Direction);
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
        Debug.Log("Death: " + _hp);
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
        GameObject.Find("GameCanvas").GetComponent<MenuController>().GameOver();
    }

    public void OnHitEnd()
    {
        if(!_isOnHit || !_currentEnemy) return;        
        Destroy(_currentEnemy);
        _currentEnemy = null;
        _isOnHit = false;        
    }
}
