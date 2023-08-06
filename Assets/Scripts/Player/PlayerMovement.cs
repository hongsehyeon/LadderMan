using UnityEngine;
using Fusion;

[OrderAfter(typeof(NetworkPhysicsSimulation2D))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    private PlayerBehaviour _behaviour;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _ladderLayer;
    private NetworkTransform _nt;
    private InputHandler _inputController;
    private LevelBehaviour _levelBehaviour;

    [SerializeField] float _speed = 5f;
    [SerializeField] float _ladderSpeedMultiplier = 0.5f;
    [SerializeField] float _jumpForce = 10f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _gravityMultiplier = 3f;
    private float _velocity;

    private Collider2D _collider;
    [Networked]
    private NetworkBool IsGrounded { get; set; }
    [Networked]
    private NetworkBool IsLadder { get; set; }

    [Space()]
    [Header("Particle")]
    [SerializeField] private ParticleManager _particleManager;

    [Space()]
    [Header("Sound")]
    [SerializeField] private SoundChannelSO _sfxChannel;
    [SerializeField] private SoundSO _jumpSound;
    [SerializeField] private AudioSource _playerSource;


    [Space()]
    [Header("Animator")]
    [SerializeField] private GameObject _sprite;
    [SerializeField] private Animation _legAnim;
    [SerializeField] private AnimationClip _walkClip;
    [SerializeField] private AnimationClip _jumpClip;
    bool _canWalk;
    bool _canJump;
    bool _flip;


    [Space()]
    [Header("Sound")]
    public SoundSO _jumpSFX;
    private void Awake()
    {
        _nt = GetComponent<NetworkTransform>();
        _collider = GetComponentInChildren<Collider2D>();
        _behaviour = GetBehaviour<PlayerBehaviour>();
        _inputController = GetBehaviour<InputHandler>();
        _levelBehaviour = FindObjectOfType<LevelBehaviour>();
    }

    public override void Spawned()
    {
        Runner.SetPlayerAlwaysInterested(Object.InputAuthority, Object, true);
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + Vector2.down * _collider.bounds.extents.y, new Vector2(1f, 0.1f) * .85f);
    }
*/
    /// <summary>
    /// Detects grounded and wall sliding state
    /// </summary>
    private void DetectGround()
    {
        IsGrounded = (bool)Runner.GetPhysicsScene2D().OverlapBox((Vector2)transform.position + Vector2.down * _collider.bounds.extents.y, new Vector2(1f, 0.1f) * .85f, 0, _groundLayer);
        _canWalk = IsGrounded;
    }

    private void DetectLadder()
    {
        IsLadder = (bool)Runner.GetPhysicsScene2D().OverlapBox((Vector2)transform.position + Vector2.down * _collider.bounds.extents.y, new Vector2(1f, 0.1f) * .85f, 0, _ladderLayer);
        _collider.enabled = !IsLadder;
    }

    public bool GetGrounded() => IsGrounded;

    public override void FixedUpdateNetwork()
    {
        if (GetInput<InputData>(out var input))
        {
            var pressed = input.GetButtonPressed(_inputController.PrevButtons);
            _inputController.PrevButtons = input.Buttons;

            DetectGround();
            DetectLadder();

            Jump(pressed);
            UpdateMovement(input);

            if (_nt.transform.position.y > _levelBehaviour.Score)
                _levelBehaviour.Score = _nt.transform.position.y;
        }
    }

    private void UpdateMovement(InputData input)
    {
        Vector3 moveVector = Vector3.zero;
        moveVector.y = _velocity;

        if (input.GetButton(InputButton.LEFT) && _behaviour.InputsAllowed)
        {
            moveVector += _speed * Vector3.left;
            if (_canWalk)
            {
                _legAnim.clip = _walkClip;
                _legAnim.Play();
            }

            if (!_flip)
            {
                _sprite.transform.localScale = new Vector3(_sprite.transform.localScale.x * -1,_sprite.transform.localScale.y,_sprite.transform.localScale.z);
                _flip = !_flip;
            }
        }
        else if (input.GetButton(InputButton.RIGHT) && _behaviour.InputsAllowed)
        {
            moveVector += _speed * Vector3.right;

            if (_canWalk)
            {
                _legAnim.clip = _walkClip;
                _legAnim.Play();
            }

            if (_flip)
            {
                _sprite.transform.localScale = new Vector3(_sprite.transform.localScale.x * -1,_sprite.transform.localScale.y,_sprite.transform.localScale.z);
                _flip = !_flip;
            }
        }
        else if (IsLadder && input.GetButton(InputButton.UP) && _behaviour.InputsAllowed)
        {
            moveVector += _speed * _ladderSpeedMultiplier * Vector3.up;
        }
        else if (IsLadder && input.GetButton(InputButton.DOWN) && _behaviour.InputsAllowed)
        {
            moveVector += _speed * _ladderSpeedMultiplier * Vector3.down;
        }

        _nt.transform.Translate(moveVector * Runner.DeltaTime);
    }

    private void Jump(NetworkButtons pressedButtons)
    {
        
        if (!IsGrounded && !IsLadder)
            _velocity += _gravity * _gravityMultiplier * Runner.DeltaTime;
        else
            _velocity = 0f;

        if (pressedButtons.IsSet(InputButton.JUMP))
        {
            if (_behaviour.InputsAllowed)
            {
                if (IsGrounded)
                {
                    _velocity = _jumpForce;
                    _canWalk = false;
                    _legAnim.clip = _jumpClip;
                    _legAnim.Play();
                    RPC_PlayJumpEffects((Vector2)transform.position - Vector2.up * .5f);
                }
            }
        }
    }

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_PlayJumpEffects(Vector2 particlePos)
    {
        PlayJumpSound();
        //PlayJumpParticle(particlePos);
    }

    private void PlayJumpSound()
    {
        _sfxChannel.CallSoundEvent(_jumpSound, Object.HasInputAuthority ? null : _playerSource);
    }

    private void PlayJumpParticle(Vector2 pos)
    {
        _particleManager.Get(ParticleManager.ParticleID.Jump).transform.position = pos;
    }
}