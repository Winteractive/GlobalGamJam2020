using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairZone : PlayerPart, IMediatorListener
{
    List<int> playersInsideTrigger = new List<int>();
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
                GlobalMediator.SendMessage(GameEvents.PLAYER_REPAIRED, new PlayerHealth.RepairMessage { playerNumber = playerToRepair, repairAmount = repairAmountPerUpdate });
                GlobalMediator.SendMessage(GameEvents.PLAYER_REPAIRED, playerNumber);
            }
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (playersInsideTrigger.Count == 0)
            timer = 0;
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if(events.HasFlag(GameEvents.PLAYER_REPAIRED))
        {
            if(data is TagCheck.TagCheckMessage tagMessage)
            {
                if(tagMessage.playerNumber == playerNumber)
                {
                    if(tagMessage.triggerInside)
                    {
                        playersInsideTrigger.Add(tagMessage.objectInside.GetComponent<Player>().playerNumber);

                    }
                    else
                    {
                        playersInsideTrigger.Remove(tagMessage.objectInside.GetComponent<Player>().playerNumber);
                    }
                }
            }
        }
    }
}
