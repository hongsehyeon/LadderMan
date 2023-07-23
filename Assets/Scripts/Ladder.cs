using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private Ladder _nextLadder;
    public Ladder NextLadder { get { return _nextLadder; } set { _nextLadder = value; } }

    [SerializeField] Transform _ladderSpawnPos;
    public Transform LadderSpawnPos { get { return _ladderSpawnPos; } }


}
