﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using FusionUtilsEvents;

public class FusionHelper : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRunner LocalRunner;

    public NetworkPrefabRef PlayerDataNO;

    public FusionEvent OnPlayerJoinedEvent;
    public FusionEvent OnPlayerLeftEvent;
    public FusionEvent OnShutdownEvent;
    public FusionEvent OnDisconnectEvent;

    /// <summary>
    /// 플레이어 입장 콜백
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            runner.Spawn(PlayerDataNO, inputAuthority: player);
        }

        if (runner.LocalPlayer == player)
        {
            LocalRunner = runner;
        }

        OnPlayerJoinedEvent?.Raise(player, runner);
    }

    /// <summary>
    /// 플레이어 퇴장 콜백
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        OnPlayerLeftEvent?.Raise(player, runner);
    }

    /// <summary>
    /// 강제 종료 콜백
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="shutdownReason"></param>
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        OnShutdownEvent?.Raise(runner: runner);
    }

    /// <summary>
    /// 서버 연결 종료 콜백
    /// </summary>
    /// <param name="runner"></param>
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        OnDisconnectEvent?.Raise(runner: runner);
    }

    #region UnusedCallbacks
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    #endregion
}