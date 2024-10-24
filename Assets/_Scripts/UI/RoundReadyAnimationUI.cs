using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundReadyAnimationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundNumberText;
    [SerializeField] private GameObject roundGameObject;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private float waitTimeToShowReady;
    [SerializeField] private float waitTimeToEnAnimation;

    public void StartRoundAnimation(float waitTimeToStartAnimation)
    {
        StartCoroutine(StartRoundAnimationCoroutine(waitTimeToStartAnimation));
    }

    private IEnumerator StartRoundAnimationCoroutine(float waitTimeToStartAnimation)
    {
        roundNumberText.text = SceneManagerSingleton.Instance.GetActualSceneIndex().ToString();

        yield return new WaitForSeconds(waitTimeToStartAnimation);

        roundGameObject.SetActive(true);

        roundNumberText.gameObject.SetActive(true);

        yield return new WaitForSeconds(waitTimeToShowReady);

        readyGameObject.SetActive(true);

        yield return new WaitForSeconds(waitTimeToEnAnimation);

        roundGameObject.SetActive(false);

        roundNumberText.gameObject.SetActive(false);

        readyGameObject.SetActive(false);
    }

    public float GetAnimationTime()
    {
        return waitTimeToShowReady + waitTimeToEnAnimation;
    }
}
