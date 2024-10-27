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
    private int levelIndex;

    public void InitializeButton(PlayableLevelSO playableLevelSO)
    {
        levelImage.sprite = playableLevelSO.levelSprite;
        levelIndex = playableLevelSO.levelIndex;
        levelText.text = playableLevelSO.levelName;
    }

    public void PlayLevel()
    {
        EventSystem.current.SetSelectedGameObject(null);

        SceneManagerSingleton.Instance.LoadScene(levelIndex);
    }
}
