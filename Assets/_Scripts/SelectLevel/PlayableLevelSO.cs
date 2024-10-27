using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Levels/New Level")]
public class PlayableLevelSO : ScriptableObject
{
    public int levelIndex;
    public Sprite levelSprite;
    public string levelName;
}
