using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderController : NetworkBehaviour
{
    int _ladderAmount;

    GameObject _contactLadder;
    void Start()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (GetInput<InputData>(out var input))
        {
            if (input.GetButton(InputButton.INSTALL))
            {
                if (_ladderAmount <= 0) return;

                if (_contactLadder == null) LadderManager.Instance.InstallLadder(transform);
                else LadderManager.Instance.InstallContinuedLadder(_contactLadder.GetComponentInChildren<Ladder>());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            _contactLadder = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            _contactLadder = null;
        }
    }
}
