using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    public int playersInGoal;
    public bool active;

    void Start()
    {
        playersInGoal = 0;
        active = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active) return;
        if (collision.CompareTag("Player"))
        {
            playersInGoal++;

            if (playersInGoal == 2)
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
