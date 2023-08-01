using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderController : NetworkBehaviour
{
    [SerializeField] private int _ladderAmount;
    [SerializeField] private float _sensingRadius;

    [SerializeField] private LayerMask _ladderLayerMask;
    void Start()
    {
        
    }

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

                if(col == null) LadderManager.Instance.InstallLadder(transform);
                else LadderManager.Instance.InstallContinuedLadder(col.gameObject.GetComponentInChildren<Ladder>());
            }
            /*if (input.GetButton(InputButton.RECALL))
            {
                Collider2D col = SenseLadder(_sensingRadius, _ladderLayerMask);

                LadderManager.Instance.RecallLadder(col.gameObject.GetComponentInChildren<Ladder>());
            }*/
        }
        
    }

    public Collider2D SenseLadder(float radius, LayerMask mask)
    {
        return Physics2D.OverlapCircle(transform.position, radius,mask);
    }
}
