using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [Networked]
    NetworkBool _isSpawned { get; set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_isSpawned == false) {
            MapManager.Instance.CreateMapToNextPos();
            _isSpawned = true;
        }
        }
    }
}