using UnityEngine;
using Fusion;

public class FusionLauncher : MonoBehaviour
{
    private NetworkRunner _runner;
    private ConnectionStatus _status;

    public enum ConnectionStatus
    {
        Disconnected,
        Connecting,
        Failed,
        Connected,
        Loading,
        Loaded
    }

    /// <summary>
    /// Fusion ���� ���� ����
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="room"></param>
    /// <param name="sceneLoader"></param>
    public async void Launch(GameMode mode, string room, INetworkSceneManager sceneLoader)
    {
        SetConnectionStatus(ConnectionStatus.Connecting, "");
        DontDestroyOnLoad(gameObject);

        if (_runner == null)
            _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.name = name;
        _runner.ProvideInput = mode != GameMode.Server;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = room,
            SceneManager = sceneLoader
        });
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="status">���� ����</param>
    /// <param name="message"></param>
    public void SetConnectionStatus(ConnectionStatus status, string message)
    {
        _status = status;
    }
}