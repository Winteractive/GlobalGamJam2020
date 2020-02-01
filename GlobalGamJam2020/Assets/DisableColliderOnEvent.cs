using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliderOnEvent : PlayerPart, IMediatorListener
{
    public GameEvents disableEvents;
    public GameEvents enableEvents;
    Collider2D collider2d;

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        collider2d = GetComponent<Collider2D>();
        GlobalMediator.AddListener(this);

    }
    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if (events.HasFlag(disableEvents))
        {
            if (data is int playerNumber)
            {
                if(playerNumber == this.playerNumber)
                {
                    collider2d.enabled = false;
                }
            }
        }
        if (events.HasFlag(enableEvents))
        {
            if (data is int playerNumber)
            {
                if (playerNumber == this.playerNumber)
                {
                    collider2d.enabled = true;
                }
            }
        }
    }
}
