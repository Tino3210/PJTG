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

        var enemie = _enemiesQueue.Dequeue();
        //TODO : Check if qte as finished or not
        HitAnimation(CharacterAnimation.Up);
        Destroy(enemie);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemies")) return;

        _enemiesQueue.Enqueue(other.gameObject);
    }
    
    public void Hit()
    {
        _hp -= 1;
        if(_hp <= 0) Death();
    }

    public void HitAnimation(CharacterAnimation ca)
    {
        switch (ca)
        {
            case CharacterAnimation.Up:
                _animator.SetTrigger("onHitUp");
            break;
            case CharacterAnimation.Down:
                _animator.SetTrigger("onHitDown");
            break;
            case CharacterAnimation.Left:
                _animator.SetTrigger("onHitLeft");
            break;
            case CharacterAnimation.Right:
                _animator.SetTrigger("onHitRight");
            break;
            default:

            break;
        }
    }

    public void Death()
    {
        _animator.SetBool("isDeath", true);
    }
}
