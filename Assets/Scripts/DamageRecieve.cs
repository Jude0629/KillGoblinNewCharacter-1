using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRecieve : MonoBehaviour
{
    [SerializeField]private PlayerHandler playerHandler;

    private void Start()
    {
        if(playerHandler==null)playerHandler = GetComponentInParent<PlayerHandler>();
    }
    public void Damage(float val=10f)
    {
        if (playerHandler != null)
        {
            playerHandler.HealthEffect(-val);
        }
    }
}
