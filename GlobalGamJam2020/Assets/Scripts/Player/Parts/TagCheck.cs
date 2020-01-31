using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class TagCheck : PlayerPart
{
    public string tagToTriggerOn;
    public GameEvents triggerEventsToTrigger;

    public struct TagCheckMessage
    {
        public int playerNumber;
        public bool triggerInside;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(tagToTriggerOn))
        {
            GlobalMediator.SendMessage(triggerEventsToTrigger, new TagCheckMessage
            {
                playerNumber = playerNumber,
                triggerInside = true
            });
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToTriggerOn))
        {
            GlobalMediator.SendMessage(triggerEventsToTrigger, new TagCheckMessage
            {
                playerNumber = playerNumber,
                triggerInside = false
            });
        }
    }

}
