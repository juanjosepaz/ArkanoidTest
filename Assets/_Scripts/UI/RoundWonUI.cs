using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoundWonUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup roundWonPanelCanvasGroup;
    [SerializeField] private GameObject roundWonMessajeGameObject;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject mainMenuButton;

    [Header("Animation")]
    [SerializeField] private float initialAlphaChangeTime;
    [SerializeField] private float enableButtonTime;

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
        StartCoroutine(StartRoundWonAnimation());
    }

    private IEnumerator StartRoundWonAnimation()
    {
        roundWonPanelCanvasGroup.DOFade(1f, initialAlphaChangeTime);

        yield return new WaitForSeconds(initialAlphaChangeTime * 2);

        roundWonPanelCanvasGroup.blocksRaycasts = true;

        roundWonPanelCanvasGroup.interactable = true;

        roundWonMessajeGameObject.SetActive(true);

        yield return new WaitForSeconds(enableButtonTime);

        continueButton.SetActive(true);

        mainMenuButton.SetActive(true);

        yield return new WaitForSeconds(enableButtonTime);

        EventSystem.current.SetSelectedGameObject(continueButton);
    }

    public void ContinueButton()
    {
        SceneManagerSingleton.Instance.LoadNextScene();

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void MainMenuButton()
    {
        SceneManagerSingleton.Instance.LoadMainMenu();

        EventSystem.current.SetSelectedGameObject(null);
    }
}
