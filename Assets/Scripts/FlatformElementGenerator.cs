using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatformElementGenerator : NetworkBehaviour
{
    [SerializeField] private GameObject[] _elementPrefab;

    [Networked] public NetworkBool IsSpawned { get; set; }
    public override void Spawned()
    {
        if (Runner.IsServer == false) return;

        if (IsSpawned == false)
        {
            int idx = Random.Range(-99, _elementPrefab.Length);

            if (idx >= 0)
                Runner.Spawn(_elementPrefab[idx], transform.position, Quaternion.identity);

            IsSpawned = true;
        }
    }

}
