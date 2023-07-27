﻿using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LadderManager : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _ladderPrefab;

    private Ladder Temp; // TEST용 코드


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
    /// 처음 root Ladder를 설치할 때 사용하는 함수
    /// </summary>
    /// <param name="transform">설치 위치</param>
    public void InstallLadder(Transform transform)
    {
        Runner.Spawn(_ladderPrefab, transform.position, Quaternion.identity, Object.InputAuthority);
    }


    /// <summary>
    /// TEST용 코드
    /// </summary>
    public void InstallLadder()
    {
        Temp = Runner.Spawn(_ladderPrefab, new Vector2(0, 0), Quaternion.identity)
            .GetComponentInChildren<Ladder>();

    }


    /// <summary>
    /// 설치한 사다리 위에 사다리를 설치하는 함수
    /// </summary>
    /// <param name="ladder">자신과 닿아있는 사다리</param>
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
    /// TEST용 코드
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
    /// 사다리 회수 함수
    /// </summary>
    /// <param name="ladder">닿아있는 사다리</param>
    public void RecallLadder(Ladder ladder)
    {
        if (ladder.PrevLadder != null)
            ladder.PrevLadder.NextLadder = null;
        if (ladder.NextLadder != null)
            ladder.NextLadder.PrevLadder = null;
        Runner.Despawn(ladder.GetComponent<NetworkObject>());
    }

    /// <summary>
    /// TEST 함수
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
