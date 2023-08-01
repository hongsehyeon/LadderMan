using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerLadderController : NetworkBehaviour
{
    [SerializeField] private int _ladderAmount;
    [SerializeField] private float _sensingRadius;
    [SerializeField] private LayerMask _ladderLayerMask;

    [SerializeField] Stack<Ladder> _myLadders = new Stack<Ladder>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,_sensingRadius);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (GetInput<InputData>(out var input))
        {
            if (input.GetButton(InputButton.INSTALL))
            {
                if (_ladderAmount <= 0) return;

                Collider2D col = SenseLadder(_sensingRadius, _ladderLayerMask);

                Ladder ladder;
                if(col == null) ladder = LadderManager.Instance.InstallLadder(transform);
                else ladder = LadderManager.Instance.InstallContinuedLadder(col.gameObject.GetComponentInChildren<Ladder>());

                _myLadders.Push(ladder);
            }
            if (input.GetButton(InputButton.RECALL))
            {
                if (_myLadders.Count <= 0) return;

                if (_myLadders.Peek().NextLadder != null) return;

                Ladder myLadder = _myLadders.Pop();

                LadderManager.Instance.RecallLadder(myLadder);
            }
        }
    }

    public Collider2D SenseLadder(float radius, LayerMask mask)
    {
        return Physics2D.OverlapCircle(transform.position, radius,mask);
    }
}
