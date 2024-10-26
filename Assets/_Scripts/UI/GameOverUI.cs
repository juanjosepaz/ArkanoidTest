using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup backgroundCanvasGroup;
    [SerializeField] private GameObject gameOverGameObject;
    [SerializeField] private GameObject newHighScoreGameObject;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject mainMenuButton;

    [Header("Animation Time")]
    [SerializeField] private float waitAnimationTime;
    [SerializeField] private float fadeAnimationTime;
    [SerializeField] private float timeToActivateText;

    private void OnEnable()
    {
        GameManager.OnGameOver += GameOverAnimation;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= GameOverAnimation;
    }

    private void GameOverAnimation()
    {
        StartCoroutine(GameOverAnimationCoroutine());
    }

    private IEnumerator GameOverAnimationCoroutine()
    {
        backgroundCanvasGroup.interactable = true;

        backgroundCanvasGroup.blocksRaycasts = true;

        var (finalScore, isANewHighScore) = GameplayDataManager.Instance.GetFinalScoreValues();

        finalScoreText.text = $"FINAL SCORE : {finalScore}";

        yield return new WaitForSeconds(waitAnimationTime);

        backgroundCanvasGroup.DOFade(1f, fadeAnimationTime).SetUpdate(true);

        yield return new WaitForSeconds(fadeAnimationTime * 2);

        gameOverGameObject.SetActive(true);

        yield return new WaitForSeconds(timeToActivateText);

        finalScoreText.gameObject.SetActive(true);

        if (isANewHighScore)
        {
            yield return new WaitForSeconds(timeToActivateText * 2);

            newHighScoreGameObject.SetActive(true);
        }

        yield return new WaitForSeconds(timeToActivateText * 2);

        mainMenuButton.SetActive(true);

        yield return new WaitForSeconds(timeToActivateText);

        EventSystem.current.SetSelectedGameObject(mainMenuButton);
    }

    public void MainMenuButton()
    {
        SceneManagerSingleton.Instance.LoadMainMenu();

        EventSystem.current.SetSelectedGameObject(null);
    }
}
