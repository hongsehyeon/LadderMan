using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using FusionUtilsEvents;

public class LobbyCanvas : MonoBehaviour
{
    private GameMode _gameMode;

    public string Nickname = "Player";
    public GameLauncher Launcher;

    public FusionEvent OnPlayerJoinedEvent;
    public FusionEvent OnPlayerLeftEvent;
    public FusionEvent OnShutdownEvent;
    public FusionEvent OnPlayerDataSpawnedEvent;

    [Space]
    [SerializeField] private GameObject _initPainel;
    [SerializeField] private GameObject _lobbyPainel;
    [SerializeField] private TextMeshProUGUI _lobbyPlayerText;
    [SerializeField] private TextMeshProUGUI _lobbyRoomName;
    [SerializeField] private Button _startButton;
    [Space]
    [SerializeField] private GameObject _modeButtons;
    [SerializeField] private TMP_InputField _nickname;
    [SerializeField] private TMP_InputField _room;

    private void OnEnable()
    {
        OnPlayerJoinedEvent.RegisterResponse(ShowLobbyCanvas);
        OnShutdownEvent.RegisterResponse(ResetCanvas);
        OnPlayerLeftEvent.RegisterResponse(UpdateLobbyList);
        OnPlayerDataSpawnedEvent.RegisterResponse(UpdateLobbyList);
    }

    private void OnDisable()
    {
        OnPlayerJoinedEvent.RemoveResponse(ShowLobbyCanvas);
        OnShutdownEvent.RemoveResponse(ResetCanvas);
        OnPlayerLeftEvent.RemoveResponse(UpdateLobbyList);
        OnPlayerDataSpawnedEvent.RemoveResponse(UpdateLobbyList);
    }

    /// <summary>
    /// 게임 모드 설정 (Fusion.GameMode)
    /// 4 = Host, 5 = Join
    /// </summary>
    /// <param name="gameMode"></param>
    public void SetGameMode(int gameMode)
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Lobby);
        _gameMode = (GameMode)gameMode;
        _modeButtons.SetActive(false);
        _nickname.transform.parent.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fusion 로직 시작
    /// </summary>
    public void StartLauncher()
    {
        Launcher = FindObjectOfType<GameLauncher>();
        Nickname = _nickname.text;
        PlayerPrefs.SetString("Nick", Nickname);
        Launcher.Launch(_gameMode, _room.text);
        _nickname.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    /// <summary>
    /// 로비 나가기
    /// </summary>
    public void LeaveLobby()
    {
        _ = LeaveLobbyAsync();
    }

    /// <summary>
    /// 게임 시작. 맵 로딩 시작
    /// </summary>
    public void StartButton()
    {
        FusionHelper.LocalRunner.SessionInfo.IsOpen = false;
        FusionHelper.LocalRunner.SessionInfo.IsVisible = false;
        LoadingManager.Instance.LoadNextLevel(FusionHelper.LocalRunner);
    }

    /// <summary>
    /// 로비 퇴장을 위한 비동기 로직
    /// </summary>
    /// <returns></returns>
    private async Task LeaveLobbyAsync()
    {
        if (FusionHelper.LocalRunner.IsServer)
        {
            CloseLobby();
        }
        await FusionHelper.LocalRunner?.Shutdown(destroyGameObject: false);
    }

    /// <summary>
    /// 로비 닫기
    /// </summary>
    public void CloseLobby()
    {
        foreach (var player in FusionHelper.LocalRunner.ActivePlayers)
        {
            if (player != FusionHelper.LocalRunner.LocalPlayer)
                FusionHelper.LocalRunner.Disconnect(player);
        }
    }

    /// <summary>
    /// 캔버스 초기화
    /// </summary>
    /// <param name="player"></param>
    /// <param name="runner"></param>
    private void ResetCanvas(PlayerRef player, NetworkRunner runner)
    {
        _initPainel.SetActive(true);
        _modeButtons.SetActive(true);
        _lobbyPainel.SetActive(false);
        _startButton.gameObject.SetActive(runner.IsServer);
    }

    /// <summary>
    /// 로비 캔버스 열기
    /// </summary>
    /// <param name="player"></param>
    /// <param name="runner"></param>
    public void ShowLobbyCanvas(PlayerRef player, NetworkRunner runner)
    {
        _initPainel.SetActive(false);
        _lobbyPainel.SetActive(true);
    }

    /// <summary>
    /// 로비 정보 갱신
    /// </summary>
    /// <param name="playerRef"></param>
    /// <param name="runner"></param>
    public void UpdateLobbyList(PlayerRef playerRef, NetworkRunner runner)
    {
        _startButton.gameObject.SetActive(runner.IsServer);
        string players = default;
        string isLocal;
        foreach (var player in runner.ActivePlayers)
        {
            isLocal = player == runner.LocalPlayer ? " (You)" : string.Empty;
            players += GameManager.Instance.GetPlayerData(player, runner)?.Nick + isLocal + " \n";
        }
        _lobbyPlayerText.text = players;
        _lobbyRoomName.text = $"Room: {runner.SessionInfo.Name}";
    }
}