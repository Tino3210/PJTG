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
    [SerializeField]
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        // nothing
    }

    // Update is called once per frame
    void Update()
    {

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
