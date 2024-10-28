using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int firstLevelSceneIndex;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject selectLevelButton;
    [SerializeField] private GameObject mainMenuGameObject;
    [SerializeField] private GameObject levelSelectionMenuGameObject;
    [SerializeField] private AudioClip gameStartSound;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameplayDataManager.Instance.InitializeDataManager();
        EventSystem.current.SetSelectedGameObject(playButton);
        highScoreText.text = PlayerPrefs.GetInt(GameplayDataManager.HIGH_SCORE, 0).ToString();
    }

    public void StartGameButton()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        EventSystem.current.SetSelectedGameObject(null);

        SoundManager.Instance.PlaySound(gameStartSound);

        yield return new WaitForSeconds(gameStartSound.length);

        SceneManagerSingleton.Instance.LoadScene(firstLevelSceneIndex);
    }

    public void SelectLevelButton()
    {
        mainMenuGameObject.SetActive(false);
        levelSelectionMenuGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton);
        SoundManager.Instance.PlayTextEnableSound();
    }

    public void CloseLevelSelecctionMenu()
    {
        mainMenuGameObject.SetActive(true);
        levelSelectionMenuGameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(selectLevelButton);
        SoundManager.Instance.PlayTextEnableSound();
    }

    public void QuitGameButton()
    {
        Debug.Log("Quit Game");

        SoundManager.Instance.PlayTextEnableSound();

        Application.Quit();
    }
}
