using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int firstLevelSceneIndex;
    [SerializeField] private GameObject mainMenuGameObject;
    [SerializeField] private GameObject levelSelectionMenuGameObject;

    private void Start()
    {
        GameplayDataManager.Instance.InitializeDataManager();
    }

    public void StartGameButton()
    {
        SceneManagerSingleton.Instance.LoadScene(firstLevelSceneIndex);
    }

    public void SelectLevelButton()
    {
        mainMenuGameObject.SetActive(false);
        levelSelectionMenuGameObject.SetActive(true);
    }

    public void CloseLevelSelecctionMenu()
    {
        mainMenuGameObject.SetActive(true);
        levelSelectionMenuGameObject.SetActive(false);
    }

    public void QuitGameButton()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}
