using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int firstLevelSceneIndex;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject selectLevelButton;
    [SerializeField] private GameObject mainMenuGameObject;
    [SerializeField] private GameObject levelSelectionMenuGameObject;

    private void Start()
    {
        GameplayDataManager.Instance.InitializeDataManager();
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void StartGameButton()
    {
        SceneManagerSingleton.Instance.LoadScene(firstLevelSceneIndex);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SelectLevelButton()
    {
        mainMenuGameObject.SetActive(false);
        levelSelectionMenuGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void CloseLevelSelecctionMenu()
    {
        mainMenuGameObject.SetActive(true);
        levelSelectionMenuGameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(selectLevelButton);
    }

    public void QuitGameButton()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}
