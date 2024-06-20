using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class AnimatorListener : MonoBehaviour
{
    [SerializeField]ThirdPersonController thirdPersonController;
    private void OnFootstep(AnimationEvent animationEvent)
    {
        thirdPersonController.OnFootstep(animationEvent);
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        thirdPersonController.OnLand(animationEvent);
    }
}
