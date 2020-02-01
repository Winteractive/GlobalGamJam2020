using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairZone : PlayerPart, IMediatorListener
{
    List<PlayerHealth> playersInsideTrigger = new List<PlayerHealth>();
    public int repairAmountPerUpdate;
    public float timeBeforeRepair = 0.5f;
    float timer;


    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        GlobalMediator.AddListener(this);
    }
    private void Update()
    {
        if (timer > timeBeforeRepair)
        {
            timer = 0;
            foreach (var playerToRepair in playersInsideTrigger)
            {
                Debug.Log("Repair..");
                if(playerToRepair.currenthealth < playerToRepair.maxHealth)
                {
                    GlobalMediator.SendMessage(GameEvents.PLAYER_REPAIRED, new PlayerData { id = playerToRepair.playerNumber});

                }
            }
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (playersInsideTrigger.Count == 0)
            timer = 0;
    }

    public void OnMediatorMessageReceived(GameEvents events, GeneralData data)
    {
        if(events.HasFlag(GameEvents.PLAYER_REPAIRED))
        {
            if(data is PlayerTriggerBoxData tagMessage)
            {
                if(tagMessage.id == playerNumber)
                {
                    if(tagMessage.enterExit)
                    {
                        playersInsideTrigger.Add(tagMessage.collidingObject.GetComponent<PlayerHealth>());

                    }
                    else
                    {
                        playersInsideTrigger.Remove(tagMessage.collidingObject.GetComponent<PlayerHealth>());
                    }
                }
            }
        }
    }
}
