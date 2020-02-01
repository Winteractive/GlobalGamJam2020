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

    private void TakeDamage()
    {
        currenthealth--;
        Debug.Log("health is now: " + currenthealth);
        // signal damage taken
        BreakCheck();
    }

    private void BreakCheck()
    {
        if (currenthealth <= 0)
        {
            Debug.Log("PLAYER "+ playerNumber + " IS DED");
            GlobalMediator.SendMessage(GameEvents.PLAYER_BREAK, new BreakMessage {
                playerNumber = playerNumber
            });
        }
    }

    private void GetRepaired()
    {
        currenthealth = maxHealth;
        //GlobalMediator.SendMessage(GameEvents.PLAYER_REPAIRED, new BreakMessage
        //{
        //    playerNumber = playerNumber
        //});
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if (events.HasFlag(GameEvents.PLAYER_TAKE_DAMAGE))
        {
            //compare event player number to this player number
            TakeDamage();
        }
        if (events.HasFlag(GameEvents.PLAYER_REPAIRED))
        {
            //compare event player number to this player number
        }
        if (events.HasFlag(GameEvents.PLAYER_CHARGE_RELEASED))
        {
            if (data is Charge.ChargeMessage chargeData)
            {
                if (chargeData.power > powerDamageThreshold && chargeData.playerNumber == playerNumber)
                {
                    TakeDamage();
                }
            }
        }
    }
}
