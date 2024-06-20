using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerHandler : MonoBehaviour
{
    private bool isMultiPlayer;
    private bool isMine;

    private float health = 0f;
    [SerializeField] private float defaultHealth = 100f;

    public bool isDead;

    [SerializeField]private Animator animator;

    [SerializeField]ThirdPersonController thirdPersonController;
    [SerializeField] ThirdPersonShooterController thirdPersonShooterController;
    [SerializeField] MultiPlayerHandler multiPlayerHandler;

    private void Awake()
    {
        health = defaultHealth;
    }
    public bool GetIsMultiPlayer()
    {
        return isMultiPlayer;
    }
    public void HealthEffect(float healthVal)
    {
        if(isDead) return;
        if (isMultiPlayer)
        {
            multiPlayerHandler.HealthEffect(healthVal);
        }
        else
        {
            ChangeHealth(healthVal);
        }
        
    }
    public void ChangeHealth(float healthVal)
    {
        health += healthVal;
        if (health <= 0f)
        {
            isDead = true;
            if(!isMine&&isMultiPlayer)
            {
                GamePlayHandler.Instance.kills++;
            }
            if (!isMultiPlayer || isMine)
            {
                GamePlayHandler.Instance.failPanel.SetActive(true);
                thirdPersonController.setAimState(false);
                thirdPersonShooterController.removeAimOnDead();
                Cursor.lockState = CursorLockMode.None;
            }
            animator.SetLayerWeight(1, 0f);
            animator.SetBool("dead", true);
            if(isMultiPlayer)
            {
                multiPlayerHandler.dead();
            }
            //animator.enabled = false;
            //die
        }
        else if (health > defaultHealth)
        {
            health = defaultHealth;
        }
    }

    public void SetMultiPlayerState(bool isMul,bool isMin)
    {
        isMultiPlayer = isMul;
        isMine = isMin;
    }
}
