using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidePlayerLock : PlayerPart, IMediatorListener
{
    public bool isRiding = false;

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        GlobalMediator.AddListener(this);
    }

    private void MountPlayer(GameObject bottomPlayer)
    {
        transform.SetParent(bottomPlayer.transform);
        isRiding = true;
    }

    private void DismountPlayer()
    {
        transform.SetParent(null);
        isRiding = false;
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if (events.HasFlag(GameEvents.PLAYER_ON_PLAYER_CHECK))
        {
            if (data is TagCheck.TagCheckMessage tagData)
            {
                if (tagData.playerNumber == playerNumber && tagData.triggerInside)
                {
                    //MountPlayer(tagData.objectInside);
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_CHARGE_RELEASED))
        {
            if (data is Charge.ChargeMessage chargedata)
            {
                if (chargedata.playerNumber == playerNumber)
                {
                    //DismountPlayer();
                }
            }
        }
    }
}
