using UnityEngine;
using Fusion;

public class GameLauncher : MonoBehaviour
{
    public GameObject LauncherPrefab;

    /// <summary>
    /// FusionLauncher 실행
    /// </summary>
    /// <param name="_gameMode">게임 모드</param>
    /// <param name="_room">룸 이름</param>
    public void Launch(GameMode _gameMode, string _room)
    {
        FusionLauncher launcher = FindObjectOfType<FusionLauncher>();
        if (launcher == null)
            launcher = Instantiate(LauncherPrefab).GetComponent<FusionLauncher>();

        LevelManager lm = FindObjectOfType<LevelManager>();
        lm.Launcher = launcher;

        launcher.Launch(_gameMode, _room, lm);
    }
}