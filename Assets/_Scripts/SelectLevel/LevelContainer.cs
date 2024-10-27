using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    [SerializeField] private List<PlayableLevelSO> playableLevelSOs;
    [SerializeField] private LevelButton levelButtonPrefab;
    [SerializeField] private Transform levelsContainer;

    private void Awake()
    {
        foreach (PlayableLevelSO levelSO in playableLevelSOs)
        {
            LevelButton levelButton = Instantiate(levelButtonPrefab, levelsContainer);
            levelButton.InitializeButton(levelSO);
        }
    }
}
