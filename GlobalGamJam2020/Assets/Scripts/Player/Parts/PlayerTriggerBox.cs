using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class PlayerTriggerBox : PlayerPart
{
    public string tagToTriggerOn;
    public GameEvents eventsToTrigger;
    List<GameObject> objectsInside = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToTriggerOn))
        {
            if(!objectsInside.Contains(collision.gameObject))
                objectsInside.Add(collision.gameObject);

            GlobalMediator.SendMessage(eventsToTrigger, new PlayerTriggerBoxData
            {
                id = playerNumber,
                enterExit = true,
                collidingObject = collision.gameObject
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
                GlobalMediator.SendMessage(eventsToTrigger, new PlayerTriggerBoxData
                {
                    id = playerNumber,
                    enterExit = false,
                    collidingObject = collision.gameObject
                });
            }
            
        }
    }

}
