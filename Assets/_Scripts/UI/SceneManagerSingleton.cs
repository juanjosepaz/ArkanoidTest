using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerSingleton : MonoBehaviour
{
    public static SceneManagerSingleton Instance { get; private set; }

    [SerializeField] private float timeOffsetAnimation;
    private const int MAIN_MENU_SCENE_INDEX = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        LoadScene(MAIN_MENU_SCENE_INDEX);
    }

    public void LoadNextScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAnimation(sceneIndex));
    }

    private IEnumerator LoadSceneAnimation(int sceneIndex)
    {
        float timeTocompleteAnimation = SceneTransitionUI.Instance.GetTimeToCompleteAnimation() + timeOffsetAnimation;

        SceneTransitionUI.Instance.ExitSceneAnimation();

        yield return new WaitForSecondsRealtime(timeTocompleteAnimation);

        GamePausedCheck();

        SceneManager.LoadScene(sceneIndex);
    }

    private void GamePausedCheck()
    {
        Time.timeScale = 1f;

        PauseMenuUI.isGamePaused = false;
    }

    public int GetActualSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
