using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class PlayerLadderController : NetworkBehaviour
{
    [SerializeField] private int _ladderAmount;
    [SerializeField] private float _sensingRadius;
    [SerializeField] private LayerMask _ladderLayerMask;

    [SerializeField] private List<Ladder> _myLadders = new List<Ladder>();


    [Header("Cooltime")]
    [SerializeField] private float _installCooltime;
    [SerializeField] private float _recallCooltime;
    TickTimer _ladderInstallTimer { get; set; }
    TickTimer _ladderRecallTimer { get; set; }

    //Outline
    GameObject _lastLadderObject;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,_sensingRadius);
    }

    public override void Spawned()
    {
        base.Spawned();
        _ladderInstallTimer = TickTimer.CreateFromSeconds(Runner, 0);
        _ladderRecallTimer = TickTimer.CreateFromSeconds(Runner, 0);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<InputData>(out var input))
        {
            if (input.GetButton(InputButton.INSTALL))
            {
                if (_ladderAmount <= 0) return;
                if (_ladderInstallTimer.Expired(Runner) == false) return;

                Collider2D col = SenseLadder();

                Ladder ladder;
                if(col == null) ladder = LadderManager.Instance.InstallLadder(this, transform);
                else ladder = LadderManager.Instance.InstallContinuedLadder(this, col.gameObject.GetComponentInChildren<Ladder>());

                _myLadders.Add(ladder);

                _ladderInstallTimer = TickTimer.CreateFromSeconds(Runner, _installCooltime);
            }
            if (input.GetButton(InputButton.RECALL))
            {
                if (_myLadders.Count <= 0) return;
                if (_ladderRecallTimer.Expired(Runner) == false) return;

                if (_myLadders.Last().NextLadder != null) return;

                Ladder myLadder = _myLadders.Last();
                _myLadders.Remove(_myLadders.Last());

                LadderManager.Instance.RecallLadder(myLadder);
                _ladderRecallTimer = TickTimer.CreateFromSeconds(Runner, _installCooltime);
            }
        }
    }

    private void Update()
    {
        Collider2D ladder = SenseLadder();

        if(ladder != null)
        {
            if (_lastLadderObject != null) _lastLadderObject.GetComponent<Ladder>().Outline.SetActive(false);

            _lastLadderObject = ladder.gameObject;
            _lastLadderObject.GetComponent<Ladder>().Outline.SetActive(true);
        }
        else
        {
            if (_lastLadderObject != null)
            {
                _lastLadderObject.GetComponent<Ladder>().Outline.SetActive(false);
                _lastLadderObject = null;
            }
        }
    }

    public Collider2D SenseLadder()
    {
        return Physics2D.OverlapCircle(transform.position, _sensingRadius, _ladderLayerMask);
    }

    public void RemoveLadder(Ladder ladder)
    {
        _myLadders.Remove(ladder);
    }
}