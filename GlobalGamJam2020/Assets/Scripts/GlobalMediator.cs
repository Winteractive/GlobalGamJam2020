using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalMediator
{
    public delegate void Listener(GameEvents events, GeneralData data);

    static List<IMediatorListener> listeners = new List<IMediatorListener>();

    //Why Event? Well we can't have static classes inherit a Interface
    static event Listener allListeners;
    //It is faster to use ForEach do to event uses a nullCe

    /// <summary>
    /// Adds a objects that wants to listen to called events
    /// </summary>
    /// <param name="listener"></param>
    public static void AddListener(IMediatorListener listener)
    {
        if (listeners.Contains(listener))
            return;

        listeners.Add(listener);
        AddListener(listener.OnMediatorMessageReceived);
    }
    /// <summary>
    /// Adds a function that wants to listen to called events
    /// </summary>
    /// <param name="listener"></param>
    public static void AddListener(Listener listener)
    {
        allListeners += listener;
    }
    /// <summary>
    /// Removes a object that listened to called events
    /// </summary>
    /// <param name="listener"></param>
    public static void RemoveListener(IMediatorListener listener)
    {
        listeners.Remove(listener);
        RemoveListener(listener.OnMediatorMessageReceived);
    }
    /// <summary>
    /// Removes a function that listened to called events
    /// </summary>
    /// <param name="listener"></param>
    public static void RemoveListener(Listener listener)
    {
        allListeners -= listener;
    }

    /// <summary>
    /// Sends a message to all listeners with a event that they will handle
    /// </summary>
    /// <param name="events"></param>
    /// <param name="data"></param>
    public static void SendMessage(GameEvents gameEvent, GeneralData data = null)
    {
        allListeners?.Invoke(gameEvent, data);
    }

    //public static void OnMediatorMessageReceived(GameEvents gameEvent, object data)
    //{
    //    throw new NotImplementedException();
    //}
}
/// <summary>
/// Events that the Mediator Uses
/// </summary>
[Flags]
public enum GameEvents
{
    PLAYER_INPUT = 1 << 0,
    PLAYER_GROUND_CHECK = 1 << 1,
    PLAYER_CHARGE_START = 1 << 2,
    PLAYER_CHARGE_RELEASED = 1 << 3,
    PLAYER_CHARGE_CANCELLED = 1 << 4,
    PLAYER_TAKE_DAMAGE = 1 << 5,
    PLAYER_COLLIDE_WITH_PLAYER = 1 << 6,
    PLAYER_REPAIRED = 1 << 7,
    PLAYER_GOT_MOUNTED = 1 << 8,
    PLAYER_IS_MOUNTING = 1 << 9,
    PLAYER_SLEEP = 1 << 10,
    RESTART_LEVEL = 1 << 11,
    GAME_STARTED = 1 << 12,
    PLAYER_REPAIR_TRIGGER_BOX = 1 << 13,

    PLAYER_GOT_DISMOUNTED = 1 << 14,
    PLAYER_FORCE_DISMOUNT = 1 << 15,

    LEVEL_WON = 1 << 16,
    LEVEL_START = 1 << 17,
    RESET_GAME = 1 << 18
}


public abstract class GeneralData { }

public class PlayerData : GeneralData
{
    public int id;
}

public class PlayerInputData : PlayerData
{
    public Vector2 axis;
    public bool key_charge;
    public bool key_abort;
    public bool key_respawn;
    public bool key_pause;
}

public class GroundCheckData : PlayerData
{
    public bool isGrounded;
}

public class PlayerMountingData : PlayerData
{
    public GameObject characterImMounting;
}
public class PlayerTriggerBoxData : PlayerData
{
    public GameObject collidingObject;
    public bool enterExit;
}
public class PlayerChargeReleaseData : PlayerData
{
    public float releasedPower;
}
public class PlayerGotMountedData : PlayerData
{
    public Transform playerMounted;
}
// walking + direction + id
// stopped walking + id
// Started charging + id
// Stopped charging + id
// aborted charging + id
// became grounded + id
// someone standing on me + id
// someone stopped standing on me + id


public interface IMediatorListener
{
    void OnMediatorMessageReceived(GameEvents events, GeneralData data);
}