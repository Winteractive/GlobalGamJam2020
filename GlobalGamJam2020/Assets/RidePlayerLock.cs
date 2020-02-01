using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidePlayerLock : PlayerPart, IMediatorListener
{
    public bool isRiding = false;
    private Transform mountedPlayer;
    public float yOffset = 0.8f;

    private void Update()
    {
        if (isRiding)
        {
            if (mountedPlayer)
            {
                transform.position = mountedPlayer.position + new Vector3(0, yOffset, 0);
            }
        }
    }
    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        GlobalMediator.AddListener(this);
    }

    private void MountPlayer(GameObject bottomPlayer)
    {
        Debug.Log("MOUNTING PLAYER");
        mountedPlayer = bottomPlayer.transform;
        transform.position = mountedPlayer.position + new Vector3(0, yOffset, 0);
        isRiding = true;
    }

    private void DismountPlayer()
    {
        Debug.Log("DISMOUNTING PLAYER");
        mountedPlayer = null;
        isRiding = false;
    }

    public void OnMediatorMessageReceived(GameEvents events, GeneralData data)
    {
        if (events.HasFlag(GameEvents.PLAYER_IS_MOUNTING))
        {
            Debug.Log("PlayerOnPLayer");
            if (data is PlayerTriggerBoxData tagData)
            {
                Debug.Log("PlayerOnPLayer: " + tagData.enterExit);
                if (tagData.id == playerNumber && tagData.enterExit)
                {
                    MountPlayer(tagData.collidingObject);
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_CHARGE_RELEASED))
        {
            if (data is PlayerChargeReleaseData chargedata)
            {
                if (chargedata.id == playerNumber)
                {
                    DismountPlayer();
                }
            }
        }
    }
}
