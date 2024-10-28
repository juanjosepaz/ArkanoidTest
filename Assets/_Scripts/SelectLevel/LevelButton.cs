using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Image levelImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private AudioClip gameStartSound;
    private int levelIndex;

    public void InitializeButton(PlayableLevelSO playableLevelSO)
    {
        levelImage.sprite = playableLevelSO.levelSprite;
        levelIndex = playableLevelSO.levelIndex;
        levelText.text = playableLevelSO.levelName;
    }

    public void PlayLevel()
    {
        StartCoroutine(StartLevelCoroutine());
    }

    private IEnumerator StartLevelCoroutine()
    {
        EventSystem.current.SetSelectedGameObject(null);

        SoundManager.Instance.PlaySound(gameStartSound);

        yield return new WaitForSeconds(gameStartSound.length);

        SceneManagerSingleton.Instance.LoadScene(levelIndex);
    }
}
