using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Contains Controller rumble functions for global and specific purposes.
/// </summary>
public static class RumbleManager
{
    public static Dictionary<int, Gamepad> playerToGampad = new Dictionary<int, Gamepad>();
    static Dictionary<int, Rumble> playerToGampadCorutine = new Dictionary<int, Rumble>();

    public static Coroutine executionForPauseRumbles;


    public static void Initialize()
    {
        GlobalMediator.AddListener(OnMediatorMessage);
    }
    public static void OnMediatorMessage(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.PLAYER_INPUT:
                    break;
                case GameEvents.PLAYER_GROUND_CHECK:
                    //   GroundCheckData gData = (GroundCheckData)data;
                    //   RumbleController(gData.id, 0.2f, 0.3f);
                    break;
                case GameEvents.PLAYER_CHARGE_START:
                    PlayerData pData = (PlayerData)data;
                    RumbleController(pData.id, 0.15f, 0.2f);
                    break;
                case GameEvents.PLAYER_CHARGE_RELEASED:
                    pData = (PlayerData)data;
                    RumbleController(pData.id, 0.2f, 0.33f);
                    break;
                case GameEvents.PLAYER_CHARGE_CANCELLED:
                    pData = (PlayerData)data;
                    RumbleController(pData.id, 0.2f, 0.125f);
                    break;
                case GameEvents.PLAYER_TAKE_DAMAGE:
                    break;
                case GameEvents.PLAYER_COLLIDE_WITH_PLAYER:
                    RumbleAllControllers(0.2f, 0.6f);
                    break;
                case GameEvents.PLAYER_REPAIRED:
                    break;
                case GameEvents.PLAYER_GOT_MOUNTED:
                    pData = (PlayerData)data;
                    RumbleController(pData.id, 0.2f, 0.4f);
                    break;
                case GameEvents.PLAYER_IS_MOUNTING:
                    pData = (PlayerData)data;
                    RumbleController(pData.id, 0.15f, 0.2f);
                    break;
                case GameEvents.PLAYER_SLEEP:
                    break;
            }
        }
    }

    public static void AddGamepadToPlayer(int playerNumber, Gamepad gamepad)
    {
        if (playerToGampad.ContainsKey(playerNumber))
        {
            UpdateGampadToPlayer(playerNumber, gamepad);
            return;
        }
        playerToGampad.Add(playerNumber, gamepad);
    }

    public static void RemovePlayerGamepad(int playerNumber)
    {
        playerToGampad.Remove(playerNumber);
    }
    public static void UpdateGampadToPlayer(int playerNumber, Gamepad gamepad)
    {
        if (playerToGampad.ContainsKey(playerNumber))
        {
            playerToGampad[playerNumber] = gamepad;
        }
    }
    /// <summary>
    /// Start Rumbling controller for player, with the same strength on both motors
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="bothMotorStrength"></param>
    public static void RumbleController(int playerNumber, float duration, float bothMotorStrength)
    {
        RumbleController(playerNumber, duration, bothMotorStrength, bothMotorStrength);
    }

    /// <summary>
    /// Start Rumbling controller for player, settings for right and left motor
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="rightMotorStrength"></param>
    /// <param name="leftMotorStrength"></param>
    public static void RumbleController(int playerNumber, float duration, float rightMotorStrength, float leftMotorStrength)
    {
        if (!playerToGampad.ContainsKey(playerNumber))
            return;

        playerToGampad[playerNumber].SetMotorSpeeds(leftMotorStrength, rightMotorStrength);

        if (playerToGampadCorutine.ContainsKey(playerNumber))
            PauseRumbleForPlayer(playerNumber);

        ResumeRumbleForPlayer(playerNumber);
        if (playerToGampadCorutine.ContainsKey(playerNumber))
        {
            playerToGampadCorutine[playerNumber].timer = duration;
        }
        else
        {
            playerToGampadCorutine.Add(playerNumber, new Rumble
            {
                timer = duration
            });
        }

        if (executionForPauseRumbles == null)
        {
            executionForPauseRumbles = GameManager.INSTANCE.StartCoroutine(PauseRumbleAfterTime());
        }
    }
    class Rumble
    {
        public float timer;
    }

    /// <summary>
    /// Pause rumble for all players after Rumble.Value.timer seconds
    /// </summary>
    /// <returns></returns>
    private static IEnumerator PauseRumbleAfterTime()
    {
        do
        {
            List<int> ToRemove = new List<int>(); // List of Rumbles to remove

            //Go through all players, that have started a rumble effect
            foreach (var item in playerToGampadCorutine)
            {
                //Note: The if a rumble effect is already going on when a player wants to make a new rumble effet. 
                // Then it keep rumbling until the new rumble effect should be completed

                item.Value.timer -= Time.unscaledDeltaTime; // Counts in realTime

                if (item.Value.timer < 0)
                {
                    //Pause a rumble effect for the player
                    PauseRumbleForPlayer(item.Key);
                    //Make sure to remove a completed rumble effect;
                    ToRemove.Add(item.Key);
                }
            }

            //Remove completed rumble effects
            foreach (var item in ToRemove)
            {
                playerToGampadCorutine.Remove(item);
            }
            yield return new WaitForEndOfFrame();
        } while (playerToGampadCorutine.Count > 0);

        executionForPauseRumbles = null;
    }
    /// <summary>
    /// Start rumbling with the previous rumble settings
    /// </summary>
    /// <param name="playerNumber"></param>
    public static void ResumeRumbleForPlayer(int playerNumber)
    {
        if (!playerToGampad.ContainsKey(playerNumber))
            return;

        playerToGampad[playerNumber].ResumeHaptics();
    }
    /// <summary>
    /// Pause rumble
    /// </summary>
    /// <param name="playerNumber"></param>
    public static void PauseRumbleForPlayer(int playerNumber)
    {
        if (!playerToGampad.ContainsKey(playerNumber))
            return;

        playerToGampad[playerNumber].PauseHaptics();
    }

    /// <summary>
    /// Resets settings to default. 
    /// </summary>
    /// <param name="playerNumber"></param>
    public static void ResetRumbleForPlayer(int playerNumber)
    {
        if (!playerToGampad.ContainsKey(playerNumber))
            return;

        playerToGampad[playerNumber].ResetHaptics();
    }
    /// <summary>
    /// Pause all controllers
    /// </summary>
    public static void PauseAllControllers()
    {
        foreach (var item in playerToGampad)
        {
            item.Value.PauseHaptics();
        }
    }
    /// <summary>
    /// Resume all controllers
    /// </summary>
    public static void ResumeAllControllers()
    {
        foreach (var item in playerToGampad)
        {
            item.Value.ResumeHaptics();
        }
    }

    /// <summary>
    /// Rumble all controllers
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="bothStrength"></param>
    public static void RumbleAllControllers(float duration, float bothStrength)
    {
        foreach (var item in playerToGampad)
        {
            RumbleController(item.Key, duration, bothStrength);
        }
    }
}
