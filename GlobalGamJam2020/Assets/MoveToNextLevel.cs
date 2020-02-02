using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextLevel : MonoBehaviour
{
    public float oneLevelHeight;

    // Start is called before the first frame update
    void Start()
    {
        GlobalMediator.AddListener(RecieveEvents);
    }

    private void RecieveEvents(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.PLAYER_INPUT:
                    break;
                case GameEvents.PLAYER_GROUND_CHECK:
                    break;
                case GameEvents.PLAYER_CHARGE_START:
                    break;
                case GameEvents.PLAYER_CHARGE_RELEASED:
                    break;
                case GameEvents.PLAYER_CHARGE_CANCELLED:
                    break;
                case GameEvents.PLAYER_TAKE_DAMAGE:
                    break;
                case GameEvents.PLAYER_COLLIDE_WITH_PLAYER:
                    break;
                case GameEvents.PLAYER_REPAIRED:
                    break;
                case GameEvents.PLAYER_GOT_MOUNTED:
                    break;
                case GameEvents.PLAYER_IS_MOUNTING:
                    break;
                case GameEvents.PLAYER_SLEEP:
                    break;
                case GameEvents.RESTART_LEVEL:
                    break;
                case GameEvents.GAME_STARTED:
                    break;
                case GameEvents.PLAYER_REPAIR_TRIGGER_BOX:
                    break;
                case global::GameEvents.PLAYER_GOT_DISMOUNTED:
                    break;
                case GameEvents.PLAYER_FORCE_DISMOUNT:
                    break;
                case global::GameEvents.PLAYER_GOT_DISMOUNTED:
                    break;
                case GameEvents.LEVEL_WON:
                    StartCoroutine(MoveToNext());
                    break;
                case GameEvents.LEVEL_START:
                    break;
            }
        }
    }

    private IEnumerator MoveToNext()
    {
        yield return new WaitForSeconds(1f);
        LeanTween.moveY(this.gameObject, this.transform.position.y + oneLevelHeight, 1.5f).setEaseInOutBack();
        yield break;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
