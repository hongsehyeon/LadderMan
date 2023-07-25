using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LadderManager : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _ladderPrefab;

    private Ladder Temp; // TEST�� �ڵ�


    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            if (data.install == 1)
            {
                if (Temp == null)
                    InstallLadder();
                else
                    InstallContinuedLadder();
            }
            if (data.recall == 1)
            {
                RecallLadder();
            }
        }

    }

    /// <summary>
    /// ó�� root Ladder�� ��ġ�� �� ����ϴ� �Լ�
    /// </summary>
    /// <param name="transform">��ġ ��ġ</param>
    public void InstallLadder(Transform transform)
    {
        Runner.Spawn(_ladderPrefab, transform.position, Quaternion.identity, Object.InputAuthority);
    }


    /// <summary>
    /// TEST�� �ڵ�
    /// </summary>
    public void InstallLadder()
    {
        Temp = Runner.Spawn(_ladderPrefab, new Vector2(0, 0), Quaternion.identity)
            .GetComponentInChildren<Ladder>();

    }


    /// <summary>
    /// ��ġ�� ��ٸ� ���� ��ٸ��� ��ġ�ϴ� �Լ�
    /// </summary>
    /// <param name="ladder">�ڽŰ� ����ִ� ��ٸ�</param>
    public void InstallContinuedLadder(Ladder ladder)
    {
        Ladder lastLadder = ladder;

        while (lastLadder.NextLadder != null)
        {
            lastLadder = lastLadder.NextLadder;
        }

        NetworkObject ladderObj = NetworkManager.Instance.Runner.Spawn(_ladderPrefab, lastLadder.LadderSpawnPos.position, Quaternion.identity, Object.InputAuthority);
        Ladder newLadder = ladderObj.GetComponentInChildren<Ladder>();

        lastLadder.NextLadder = newLadder;
        newLadder.PrevLadder = lastLadder;
    }



    /// <summary>
    /// TEST�� �ڵ�
    /// </summary>
    public void InstallContinuedLadder()
    {
        Ladder lastLadder = Temp;
        while (lastLadder.NextLadder != null)
        {
            lastLadder = lastLadder.NextLadder;
        }

        NetworkObject ladderObj = NetworkManager.Instance.Runner.Spawn(_ladderPrefab, lastLadder.LadderSpawnPos.position, Quaternion.identity, Object.InputAuthority);

        lastLadder.NextLadder = ladderObj.GetComponentInChildren<Ladder>();
    }

    /// <summary>
    /// ��ٸ� ȸ�� �Լ�
    /// </summary>
    /// <param name="ladder">����ִ� ��ٸ�</param>
    public void RecallLadder(Ladder ladder)
    {
        if (ladder.PrevLadder != null)
            ladder.PrevLadder.NextLadder = null;
        if (ladder.NextLadder != null)
            ladder.NextLadder.PrevLadder = null;
        Runner.Despawn(ladder.GetComponent<NetworkObject>());
    }

    /// <summary>
    /// TEST �Լ�
    /// </summary>
    public void RecallLadder()
    {
        Ladder lastLadder = Temp;
        while (lastLadder.NextLadder != null)
        {
            lastLadder = lastLadder.NextLadder;
        }

        RecallLadder(lastLadder);
    }
}
