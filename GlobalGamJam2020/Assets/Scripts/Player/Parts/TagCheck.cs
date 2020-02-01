using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class TagCheck : PlayerPart
{
    public string tagToTriggerOn;
    public GameEvents triggerEventsToTrigger;
    int numberOfObjectsInside;
    public struct TagCheckMessage
    {
        public int playerNumber;
        public bool triggerInside;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToTriggerOn))
        {
            numberOfObjectsInside++;
            GlobalMediator.SendMessage(triggerEventsToTrigger, new TagCheckMessage
            {
                playerNumber = playerNumber,
                triggerInside = true
            });
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToTriggerOn) && collision.transform != transform.parent)
        {
            numberOfObjectsInside--;
            if(numberOfObjectsInside == 0)
            {
                GlobalMediator.SendMessage(triggerEventsToTrigger, new TagCheckMessage
                {
                    playerNumber = playerNumber,
                    triggerInside = false
                });
            }
            
        }
    }

}
