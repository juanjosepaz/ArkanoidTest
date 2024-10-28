using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VictoryPanelUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject victoryTextGameObject;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject newHighScoreGameObject;
    [SerializeField] private GameObject mainMenuButton;

    [Header("Animation Time")]
    [SerializeField] private float waitTimeToActivateAnimation;
    [SerializeField] private float timeToActivateText;
    [SerializeField] private float timeToActivateFeedBack;

    [Header("Sounds")]
    [SerializeField] private AudioClip endingMusic;

    private void Start()
    {
        StartCoroutine(StartVictoryPanelAnimationCoroutine());
    }

    private IEnumerator StartVictoryPanelAnimationCoroutine()
    {
        var (finalScore, isANewHighScore) = GameplayDataManager.Instance.GetScoreValues();

        finalScoreText.text = $"FINAL SCORE : {finalScore}";

        yield return new WaitForSeconds(waitTimeToActivateAnimation);

        SoundManager.Instance.PlaySound(endingMusic);

        SoundManager.Instance.PlayTextEnableSound();

        victoryTextGameObject.SetActive(true);

        yield return new WaitForSeconds(timeToActivateText);

        finalScoreText.gameObject.SetActive(true);

        SoundManager.Instance.PlayTextEnableSound();

        if (isANewHighScore)
        {
            yield return new WaitForSeconds(timeToActivateFeedBack);

            newHighScoreGameObject.SetActive(true);

            SoundManager.Instance.PlayTextEnableSound();
        }

        yield return new WaitForSeconds(timeToActivateFeedBack);

        mainMenuButton.SetActive(true);

        SoundManager.Instance.PlayTextEnableSound();

        yield return new WaitForSeconds(timeToActivateFeedBack);

        EventSystem.current.SetSelectedGameObject(mainMenuButton);
    }

    public void MainMenuButton()
    {
        SceneManagerSingleton.Instance.LoadMainMenu();

        SoundManager.Instance.StopAllSounds();

        EventSystem.current.SetSelectedGameObject(null);
    }
}
