using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairZone : PlayerPart,IMediatorListener
{
    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
       if(events.HasFlag(GameEvents.PLAYER_REPAIR))
        {
            if(data is TagCheck.TagCheckMessage tagMessage)
            {
                if(tagMessage.playerNumber == playerNumber)
                {
                    foreach (var item in tagMessage.objectsInside)
                    {

                    }
                }
            }
        }
    }
}
