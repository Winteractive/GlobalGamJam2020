using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalMediator
{
    public delegate void Listener(GameEvents events, object data);

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
    public static void SendMessage(GameEvents gameEvent, object data = null)
    {
        allListeners?.Invoke(gameEvent, data);
    }

    public static void OnMediatorMessageReceived(GameEvents gameEvent, object data)
    {
        throw new NotImplementedException();
    }
}
/// <summary>
/// Events that the Mediator Uses
/// </summary>
[Flags]public enum GameEvents
{
    NONE       = 0,
}

public interface IMediatorListener
{
    void OnMediatorMessageReceived(GameEvents events, object data);
}