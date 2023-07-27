using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : NetworkBehaviour
{
    /// <summary>
    /// 맵 플랫폼 프리팹 모음
    /// </summary>
    [SerializeField] List<NetworkPrefabRef> _mapPrefab;

    /// <summary>
    /// 맵 플랫폼 프리팹 간격
    /// </summary>
    [SerializeField] int _mapPosInterval;

    Vector2 _currentMapSpawnPosition;

    public void CreateMapToNextPos()
    {
        int randIdx = Random.Range(0, _mapPrefab.Count);
        Runner.Spawn(_mapPrefab[randIdx], _currentMapSpawnPosition, Quaternion.identity);

        _currentMapSpawnPosition = new Vector2(_currentMapSpawnPosition.x, _currentMapSpawnPosition.y + _mapPosInterval);
    }
}
