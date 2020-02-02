using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour, IMediatorListener
{
    public int id = 0;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    public StartZone nextZone;
    public bool isFirstLevel;


    private void OnEnable()
    {
        GlobalMediator.AddListener(this);
    }
    private void OnDisable()
    {
        GlobalMediator.RemoveListener(this);
    }

    public void OnMediatorMessageReceived(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.RESTART_LEVEL:
                    RespawnPlayers();
                    break;
                case GameEvents.GAME_STARTED:
                    if(nextZone)
                        nextZone.enabled = false;
                    break;
                case GameEvents.LEVEL_WON:
                 //   if (nextZone)
                 //       nextZone.enabled = true;
                 //   enabled = false;
                    break;
                case GameEvents.LEVEL_START:
                    RespawnPlayers();
                    break;
                default:
                    break;
            }
        }
    }

    public void RespawnPlayers()
    {
        GlobalMediator.SendMessage(GameEvents.PLAYER_RESPAWN, new PlayerRespawnData
        {
            id = 1,
            position = player1SpawnPoint.position
        });
        GlobalMediator.SendMessage(GameEvents.PLAYER_RESPAWN, new PlayerRespawnData
        {
            id = 2,
            position = player2SpawnPoint.position
        });
    }
    private void OnDrawGizmosSelected()
    {
        if(player1SpawnPoint)
            Gizmos.DrawWireSphere(player1SpawnPoint.position, 0.5f);
        if(player2SpawnPoint)
            Gizmos.DrawWireSphere(player2SpawnPoint.position, 0.5f);
    }
}
