using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class MapManager : NetworkBehaviour
{

    #region Singleton

    static MapManager _instance;
    public static MapManager Instance { get => _instance; }
    #endregion

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public override void Spawned()
    {
        base.Spawned();
        if (Runner.IsServer)
        {
            CreateMapToNextPos();
        }
    }

    /// <summary>
    /// 맵 플랫폼 프리팹 모음
    /// </summary>
    [SerializeField] List<NetworkPrefabRef> _mapPrefab;

    /// <summary>
    /// 맵 플랫폼 프리팹 간격
    /// </summary>
    [SerializeField] int _mapPosInterval;

    Vector2 _currentMapSpawnPosition;


    /// <summary>
    /// 맵 생성 함수 
    /// <br></br>
    /// 프리팹 중 하나를 랜덤으로 생성함
    /// </summary>
    [ContextMenu("Create")]
    public void CreateMapToNextPos()
    {
        int randIdx = Random.Range(0, _mapPrefab.Count);
        Runner.Spawn(_mapPrefab[randIdx], _currentMapSpawnPosition, Quaternion.identity);

        _currentMapSpawnPosition = new Vector2(_currentMapSpawnPosition.x, _currentMapSpawnPosition.y + _mapPosInterval);
    }
}
