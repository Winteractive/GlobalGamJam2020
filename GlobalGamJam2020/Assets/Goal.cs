using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int necessaryPeopleInGoal = 2;
    public int playersInGoal;
    public bool active;

    void Start()
    {
        playersInGoal = 0;
        active = true;
        GlobalMediator.AddListener(RecieveEvents);
    }

    private void RecieveEvents(GameEvents events, GeneralData data)
    {
        if (events.HasFlag(GameEvents.RESET_GAME))
        {
            active = true;
            playersInGoal = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active) return;
        if (collision.CompareTag("Player"))
        {
            playersInGoal++;

            if (playersInGoal == necessaryPeopleInGoal)
            {
                active = false;
                GlobalMediator.SendMessage(GameEvents.LEVEL_WON);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!active) return;
        if (collision.CompareTag("Player"))
        {
            playersInGoal--;
            if (playersInGoal < 0)
            {
                playersInGoal = 0;
            }
        }

    }
}
