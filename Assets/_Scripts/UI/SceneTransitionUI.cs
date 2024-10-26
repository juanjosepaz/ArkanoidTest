using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SceneTransitionUI : MonoBehaviour
{
    public static SceneTransitionUI Instance { get; private set; }

    [SerializeField] private RectTransform blockObject;
    [SerializeField] private float finalExitPosition;
    [SerializeField] private float timeTocompleteAnimation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        EnterSceneAnimation();
    }

    public void EnterSceneAnimation()
    {
        blockObject.DOAnchorPosX(finalExitPosition, timeTocompleteAnimation)
        .SetEase(Ease.Linear)
        .OnComplete(() => blockObject.gameObject.SetActive(false))
        .SetUpdate(true);
    }

    public void ExitSceneAnimation()
    {
        blockObject.anchoredPosition = new(-finalExitPosition, 0);

        blockObject.gameObject.SetActive(true);

        blockObject.DOAnchorPosX(0, timeTocompleteAnimation).SetUpdate(true);
    }

    public float GetTimeToCompleteAnimation()
    {
        return timeTocompleteAnimation;
    }
}
