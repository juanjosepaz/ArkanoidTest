using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimationBehaviour : StateMachineBehaviour
{
    [SerializeField] private float waitTimeToShine;
    private float actualTime;
    private bool canTriggerAnimation;
    private const string SHINE_TRIGGER_VALUE = "Shine";

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        actualTime = waitTimeToShine;

        canTriggerAnimation = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!canTriggerAnimation) { return; }

        actualTime -= Time.deltaTime;

        if (actualTime <= 0)
        {
            animator.SetTrigger(SHINE_TRIGGER_VALUE);

            canTriggerAnimation = false;
        }
    }
}
