/*[Note]:
Should we have an interface for managers that are static with stuff like Initialize() and RemoveMe() functions? 
(Static classes cannot have Interfaces. This is do to  "Static member cannot be referenced through an instance." and Interfaces cannot implement static methods. This is becouase interfaces works with instances objects/functions)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
///  Sorts player devices to correct players
/// </summary>
public static class InputManager
{
    public static int numberOfPlayers = 2;
    public delegate void UpdateActionMaps();
    public static bool initialized = false;
    public static Dictionary<int, List<InputDevice>> playerIdToDevices = new Dictionary<int, List<InputDevice>>();
    private static Dictionary<int, UpdateActionMaps> playerIdUpdateActionMap = new Dictionary<int, UpdateActionMaps>();
    public static void Initialize()
    {
        SetUpPlayerNumbers(numberOfPlayers);
        RevaluateAllDevices();
        InputSystem.onDeviceChange += InputSystem_onDeviceChange;
        initialized = true;
    }

    static InputDevice prevInputDevice;
    static InputDeviceChange prevInputDeviceChange;
    /// <summary>
    /// Updates state of device
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private static void InputSystem_onDeviceChange(InputDevice arg1, InputDeviceChange arg2)
    {
        if (prevInputDevice == arg1 && prevInputDeviceChange == arg2)
            return;

        prevInputDevice = arg1;
        prevInputDeviceChange = arg2;

        switch (arg2)
        {
            case InputDeviceChange.Added:
                Debug.Log($" Added {arg1.name} with DeviceId: {arg1.deviceId} and Path: {arg1.path} ");
                EvaluateDevice(arg1);
                break;
            case InputDeviceChange.Removed:
                Debug.Log($" Removed {arg1.name} with DeviceId: {arg1.deviceId} and Path: {arg1.path} ");
                RemoveDevice(arg1);
                break;
            case InputDeviceChange.Disconnected:
                break;
            case InputDeviceChange.Reconnected:
                break;
            default:
                break;
        }
    }
    public static void RemoveDevice(InputDevice device)
    {
        foreach (var item in playerIdToDevices)
        {
            if (item.Value.Contains(device))
            {
                item.Value.Remove(device);
                if (playerIdUpdateActionMap.ContainsKey(item.Key))
                {
                    Debug.Log($"Update ActionMap for: {item.Key} ");
                    playerIdUpdateActionMap[item.Key]?.Invoke();
                }
            }
        }

    }
    public static void SetUpPlayerNumbers(int numberOfPlayers)
    {
        for (int i = 0; i <= numberOfPlayers; i++)
        {
            SetUpPlayerNumber(i);
        }
    }
    public static void SetUpPlayerNumber(int playerNumber)
    {
        if (!playerIdToDevices.ContainsKey(playerNumber))
            playerIdToDevices.Add(playerNumber, new List<InputDevice>());

    }

    /// <summary>
    /// Determines what number the device is by removing all text in device name but number
    /// Adds numbered device to numbered player
    /// </summary>
    /// <param name="device"></param>
    public static void EvaluateDevice(InputDevice device)
    {
        string deviceSplit = device.name.Substring(device.displayName.Count());

        int playerNumber = 1;
        if (!string.IsNullOrWhiteSpace(deviceSplit))
        {
            playerNumber += int.Parse(deviceSplit);
        }
        //Debug.Log($" {device.name} - {device.displayName} => {deviceSplit} => {playerNumber} ");

        if (!playerIdToDevices.ContainsKey(playerNumber))
        {
            SetUpPlayerNumber(numberOfPlayers);
        }

        if (device is Keyboard)
        {
            foreach (var item in playerIdToDevices)
            {
                item.Value.Add(device);
            }
        }

        if (!playerIdToDevices[playerNumber].Contains(device))
        {
            playerIdToDevices[playerNumber].Add(device);

            if (device is Gamepad gamepad)
            {
                RumbleManager.AddGamepadToPlayer(playerNumber, gamepad);
            }

            if (playerIdUpdateActionMap.ContainsKey(playerNumber))
            {
                //Debug.Log($"Update ActionMap for: {playerNumber} ");
                playerIdUpdateActionMap[playerNumber]?.Invoke();
            }
        }
    }
    public static void RevaluateAllDevices()
    {
        foreach (var item in InputSystem.devices)
        {
            EvaluateDevice(item);
        }
    }

    /// <summary>
    /// Get action map with correct device for player number
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public static InputSystemControls GetInputActions(int playerNumber)
    {
        if (!playerIdToDevices.ContainsKey(playerNumber))
        {
            SetUpPlayerNumbers(numberOfPlayers);
            RevaluateAllDevices();
            if (playerIdToDevices.ContainsKey(playerNumber))
            {
                return null;
            }
        }
        InputSystemControls actions = new InputSystemControls();

        actions.devices = playerIdToDevices[playerNumber].ToArray();
        return actions;
    }
    public static void SetPlayerNumberUpdateDevice(int playerNumber, UpdateActionMaps updateActionMaps)
    {
        if (playerIdUpdateActionMap.ContainsKey(playerNumber))
        {
            return;
        }
        else
        {
            playerIdUpdateActionMap.Add(playerNumber, updateActionMaps);
        }
    }

}

