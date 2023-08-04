using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private Ladder _prevLadder;
    private Ladder _nextLadder;
    public Ladder PrevLadder { get { return _prevLadder; } set { _prevLadder = value; } }
    public Ladder NextLadder { get { return _nextLadder; } set { _nextLadder = value; } }

    [SerializeField] Transform _ladderSpawnPos;
    public Transform LadderSpawnPos { get { return _ladderSpawnPos; } }

    public GameObject Outline;

    public PlayerLadderController Owner { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lava"))
        {
            Owner.RemoveLadder(this);
            Destroy(gameObject);
        }
    }
}
