using UnityEngine;
using Fusion;

public class Lava : NetworkBehaviour
{
    /// <summary>
    /// 게임이 끝났는가?
    /// </summary>
    public NetworkBool IsEnd { get; set; } // TODO: 나중에 매니저 참조해서 가져오기

    private LevelBehaviour _levelBehaviour;

    public override void Spawned()
    {
        _levelBehaviour = FindObjectOfType<LevelBehaviour>();
    }

    public override void FixedUpdateNetwork()
    {
        if (IsEnd == true) return;
        transform.Translate((0.1f + _levelBehaviour.Score * 0.02f) * Runner.DeltaTime * Vector3.up);
    }
}
