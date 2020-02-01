using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class TagCheck : PlayerPart
{
    public string tagToTriggerOn;
    public GameEvents triggerEventsToTrigger;
    List<GameObject> objectsInside = new List<GameObject>();

    public struct TagCheckMessage
    {
        public int playerNumber;
        public bool triggerInside;
        public GameObject objectsInside;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToTriggerOn))
        {
            if(!objectsInside.Contains(collision.gameObject))
                objectsInside.Add(collision.gameObject);

            GlobalMediator.SendMessage(triggerEventsToTrigger, new TagCheckMessage
            {
                playerNumber = playerNumber,
                triggerInside = true,
                objectsInside = collision.gameObject
            });
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToTriggerOn) && collision.transform != transform.parent)
        {
            objectsInside.Remove(collision.gameObject);

            if (objectsInside.Count == 0)
            {
                GlobalMediator.SendMessage(triggerEventsToTrigger, new TagCheckMessage
                {
                    playerNumber = playerNumber,
                    triggerInside = false,
                    objectsInside = collision.gameObject
                });
            }
            
        }
    }

}
