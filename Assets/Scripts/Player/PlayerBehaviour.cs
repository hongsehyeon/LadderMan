using System.Collections;
using UnityEngine;
using Fusion;
using static Unity.Collections.Unicode;

public class PlayerBehaviour : NetworkBehaviour
{
    public Transform CameraTransform;

    [Networked(OnChanged = nameof(OnNickChanged))]
    public NetworkString<_16> Nickname { get; set; }
    [Networked]
    public Color PlayerColor { get; set; }

    public int PlayerID { get; private set; }

    private NetworkTransform _nt;
    private InputHandler _inputController;
    private Collider2D _collider;
    private Collider2D _hitCollider;

    [Networked]
    private TickTimer RespawnTimer { get; set; }
    [Networked(OnChanged = nameof(OnSpawningChange))]
    private NetworkBool Respawning { get; set; }
    [Networked]
    private NetworkBool Finished { get; set; }
    [Networked]
    public NetworkBool InputsAllowed { get; set; }


    [SerializeField] private ParticleManager _particleManager;

    [Space()]
    [Header("Sound")]
    [SerializeField] private SoundChannelSO _sfxChannel;
    [SerializeField] private SoundSO _deathSound;
    [SerializeField] private AudioSource _playerSource;

    private void Awake()
    {
        _inputController = GetBehaviour<InputHandler>();
        _nt = GetBehaviour<NetworkTransform>();
        _collider = GetComponentInChildren<Collider2D>();
    }

    public override void Spawned()
    {
        PlayerID = Object.InputAuthority;

        if (Object.HasInputAuthority)
        {
            //Set Interpolation data source to predicted if is input authority.
            //_nt.InterpolationDataSource = InterpolationDataSources.Predicted;
            CameraManager camera = FindObjectOfType<CameraManager>();
            camera.CameraTarget = CameraTransform;

            if (Nickname == string.Empty)
            {
                RPC_SetNickname(PlayerPrefs.GetString("Nick"));
            }
            GetComponentInChildren<SpriteRenderer>().sortingOrder += 1;
        }
        GetComponentInChildren<NicknameText>().SetupNick(Nickname.ToString());
        GetComponentInChildren<SpriteRenderer>().color = PlayerColor;
        _particleManager.ClearParticles();
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SetNickname(string nick)
    {
        Nickname = nick;
    }

    public static void OnNickChanged(Changed<PlayerBehaviour> changed)
    {
        changed.Behaviour.OnNickChanged();
    }

    public void SetInputsAllowed(bool value)
    {
        InputsAllowed = value;
    }

    private void SetRespawning()
    {
        if (Runner.IsServer)
        {
            RPC_DeathEffects();
        }
    }

    /// <summary>
    /// Confirm death effect on GFX in case a client-side predicted death was wrong.
    /// </summary>
    /// <param name="changed"></param>
    public static void OnSpawningChange(Changed<PlayerBehaviour> changed)
    {
        if (changed.Behaviour.Respawning)
        {
            changed.Behaviour.SetGFXActive(false);
        }
        else
        {
            changed.Behaviour.SetGFXActive(true);
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_DeathEffects()
    {
        _particleManager.Get(ParticleManager.ParticleID.Death).transform.position = transform.position;
        PlayDeathSound();
    }

    private void SetGFXActive(bool value)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(value);
    }

    private void OnNickChanged()
    {
        GetComponentInChildren<NicknameText>().SetupNick(Nickname.ToString());
    }

    public override void FixedUpdateNetwork()
    {

        DetectCollisions();

        if (GetInput<InputData>(out var input) && InputsAllowed)
        {
            if (input.GetButtonPressed(_inputController.PrevButtons).IsSet(InputButton.RESPAWN) && !Respawning)
            {
                RequestRespawn();
            }
        }

        if (Respawning)
        {
            if (RespawnTimer.Expired(Runner))
            {
                StartCoroutine(Respawn());
            }
        }
    }

    private void PlayDeathSound()
    {
        _sfxChannel.CallSoundEvent(_deathSound, Object.HasInputAuthority ? null : _playerSource);
    }

    private IEnumerator Respawn()
    {
        _nt.TeleportToPosition(PlayerSpawner.PlayerSpawnPos);
        yield return new WaitForSeconds(.1f);
        Respawning = false;
        SetInputsAllowed(true);
    }

    private void FinishRace()
    {
        if (Finished) { return; }

        if (Object.HasInputAuthority)
        {
            GameManager.Instance.SetPlayerSpectating(this);
        }

        if (Runner.IsServer)
        {
            FindObjectOfType<LevelBehaviour>().PlayerOnFinishLine(Object.InputAuthority, this);
            Finished = true;
        }
    }

    public void RequestRespawn()
    {
        Respawning = true;
        SetInputsAllowed(false);
        RespawnTimer = TickTimer.CreateFromSeconds(Runner, 1f);
        SetRespawning();
    }

    private void DetectCollisions()
    {
        _hitCollider = Runner.GetPhysicsScene2D().OverlapBox(transform.position, _collider.bounds.size * .9f, 0, LayerMask.GetMask("Interact"));
        if (_hitCollider != default)
        {
            if (_hitCollider.tag.Equals("Kill") && !Respawning)
            {
                RequestRespawn();
            }
            else if (_hitCollider.tag.Equals("Finish") && !Finished)
            {
                FinishRace();
            }
        }
    }

    public void Stun(float duration)
    {
        if (InputsAllowed)
        {
            InputsAllowed = false;
            StartCoroutine(StunCoroutine(duration));
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        InputsAllowed = true;
    }
}
