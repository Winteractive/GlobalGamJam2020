using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : PlayerPart, IMediatorListener
{
    public int currenthealth;
    public int maxHealth;
    public float powerDamageThreshold = 15;

    private void Start()
    {
        currenthealth = maxHealth;
        GlobalMediator.AddListener(this);
    }

    public struct BreakMessage 
    {
        public int playerNumber;
    }
    public struct RepairMessage
    {
        public int playerNumber;
        public int repairAmount;

    }
    private void TakeDamage()
    {
        currenthealth--;
        // signal damage taken
        BreakCheck();
    }

    private void BreakCheck()
    {
        if (currenthealth <= 0)
        {
            Debug.Log("PLAYER "+ playerNumber + " IS DED");
            GlobalMediator.SendMessage(GameEvents.PLAYER_SLEEP, new PlayerData
            {
                id = playerNumber,
            });
        }
    }

    private void GetRepaired()
    {
        currenthealth++;
        if (currenthealth > maxHealth)
            currenthealth = maxHealth;
    }

    public void OnMediatorMessageReceived(GameEvents events, GeneralData data)
    {
        if (events.HasFlag(GameEvents.PLAYER_TAKE_DAMAGE))
        {
            
            //compare event player number to this player number
            TakeDamage();
        }
        if (events.HasFlag(GameEvents.PLAYER_REPAIRED))
        {
            //compare event player number to this player number
            if(data is PlayerData repairMessage)
            {
                if(playerNumber == repairMessage.id)
                {
                    GetRepaired();
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_CHARGE_RELEASED))
        {
            if (data is PlayerChargeReleaseData chargeData)
            {
                if (chargeData.releasedPower > powerDamageThreshold && chargeData.id == playerNumber)
                {
                    TakeDamage();
                }
            }
        }
    }
}
