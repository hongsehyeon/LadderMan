using UnityEngine;
using Fusion;

public class Lava : NetworkBehaviour
{
    /// <summary>
    /// 게임이 끝났는가?
    /// </summary>
    public NetworkBool IsEnd { get; set; } // TODO: 나중에 매니저 참조해서 가져오기

    /// <summary>
    /// 용암 수위 상승 스피드
    /// </summary>
    [SerializeField] private float _speed;

    public override void FixedUpdateNetwork()
    {
        if (IsEnd == true) return;
        transform.Translate(_speed * Runner.DeltaTime * Vector3.up);
    }
}
