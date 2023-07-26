using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Random = UnityEngine.Random;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [SerializeField] private Animator _loadingScreenAnimator;

    private int _lastLevelIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(transform.parent);
    }

    public void ResetLastLevelsIndex()
    {
        _lastLevelIndex = 0;
    }

    /// <summary>
    /// Game ¾À ·Îµù
    /// </summary>
    /// <param name="runner"></param>
    public void LoadLevel(NetworkRunner runner)
    {
        runner.SetActiveScene("Game");
    }

    public void LoadRandomLevel(NetworkRunner runner)
    {
        int sceneIndex = Random.Range(1, SceneManager.sceneCountInBuildSettings);
        if (_lastLevelIndex == sceneIndex)
        {
            sceneIndex = sceneIndex + 1 >= SceneManager.sceneCountInBuildSettings ? sceneIndex - 1 : sceneIndex + 1;
        }
        _lastLevelIndex = sceneIndex;
        string scenePath = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(sceneIndex));
        runner.SetActiveScene(scenePath);
    }

    public void StartLoadingScreen()
    {
        _loadingScreenAnimator.Play("In");
    }

    public void FinishLoadingScreen()
    {
        _loadingScreenAnimator.Play("Out");
    }
}