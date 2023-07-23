using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LadderManager : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _ladderPrefab;

    private Ladder Temp;


    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Debug.Log("OutPut: " + data.isMouseClick);
            if(data.isMouseClick == true)
            {
                if (Temp == null)
                    InstallLadder();
                else
                    InstallContinuedLadder();
            }
        }

    }
    public void InstallLadder(Transform transform)
    {
        Runner.Spawn(_ladderPrefab, transform.position, Quaternion.identity, Object.InputAuthority);
    }

    public void InstallLadder()
    {
        Temp = Runner.Spawn(_ladderPrefab, new Vector2(0,0), Quaternion.identity)
            .GetComponentInChildren<Ladder>();
        
    }

