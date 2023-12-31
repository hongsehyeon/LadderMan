using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : NetworkBehaviour
{
    [SerializeField] SpriteRenderer SR;
    [SerializeField] Sprite[] sprites;

    int _dir;
    [SerializeField] private float _speed;


    float _nextMoveTime;

    [SerializeField] float _moveTime;
    [SerializeField] float _moveTimeTerm; 

    [SerializeField] LayerMask _groundLayer;

    public override void Spawned()
    {
        base.Spawned();
        SR.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (Time.time >= _nextMoveTime)
        {
            MoveSetting();
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position + new Vector3(_dir, 0, 0), Vector2.down, 1, _groundLayer);

        if(hitInfo.collider != null)
            transform.Translate(new Vector2(_dir * _speed * Runner.DeltaTime,0));
    }
    public void MoveSetting()
    { 
        if(_dir != 0)
        {
            _dir = 0;
            _nextMoveTime = Time.time + _moveTimeTerm;
        }
        else
        {
            _dir = (Random.Range(0, 2) == 0) ? -1 : 1;
            if (_dir == -1) SR.flipX = true;
            else if (_dir == 1) SR.flipX = false;
            _nextMoveTime = Time.time + _moveTime;
        }
    }
}
