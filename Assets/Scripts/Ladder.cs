using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : NetworkBehaviour
{
    private Ladder _prevLadder;
    private Ladder _nextLadder;
    public Ladder PrevLadder { get { return _prevLadder; } set { _prevLadder = value; } }
    public Ladder NextLadder { get { return _nextLadder; } set { _nextLadder = value; } }

    [SerializeField] Transform _ladderSpawnPos;
    public Transform LadderSpawnPos { get { return _ladderSpawnPos; } }

    public GameObject Outline;

    public PlayerLadderController Owner { get; set; }

    public SpriteRenderer SR;


    [Networked]
    public Color LadderColor { get; set; }


    private void Start()
    {
        SR.color = LadderColor;
    }
    public override void Spawned()
    {
        base.Spawned();
        SR.color = LadderColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lava"))
        {
            Owner.RemoveLadder(this);
            Destroy(gameObject);
        }
    }
}
