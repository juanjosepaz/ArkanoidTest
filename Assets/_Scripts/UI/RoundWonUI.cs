using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundWonUI : MonoBehaviour
{
    [SerializeField] private GameObject roundWonPanelGameObject;

    private void OnEnable()
    {
        GameManager.OnRoundWon += GameManager_OnRoundWon;
    }

    private void OnDisable()
    {
        GameManager.OnRoundWon -= GameManager_OnRoundWon;
    }

    private void GameManager_OnRoundWon()
    {
        roundWonPanelGameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        SceneManagerSingleton.Instance.LoadNextScene();
    }

    public void MainMenuButton()
    {
        SceneManagerSingleton.Instance.LoadMainMenu();
    }
}
