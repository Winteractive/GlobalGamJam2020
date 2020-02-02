using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : PlayerPart
{
    InputSystemControls inputs;
    bool holdingCharge;
    bool allowedToMove;

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        allowedToMove = false;
        InputManager.SetPlayerNumberUpdateDevice(playerNumber, InputManager_OnChanges);
        InputManager_OnChanges();
        inputs.Player.Charge.started += Charge_started;
        inputs.Player.Charge.canceled += Charge_canceled;
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
                    allowedToMove = true;
                    break;
                case GameEvents.PLAYER_REPAIR_TRIGGER_BOX:
                    break;
                case GameEvents.PLAYER_GOT_DISMOUNTED:
                    break;
                case GameEvents.LEVEL_WON:
                    allowedToMove = false;
                    break;
                case GameEvents.LEVEL_START:
                    allowedToMove = true;
                    break;
            }
        }
    }

    private void Charge_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        holdingCharge = false;
    }

    private void Charge_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        holdingCharge = true;
    }

    private void InputManager_OnChanges()
    {
        inputs?.Disable();
        inputs = InputManager.GetInputActions(playerNumber);
        inputs?.Enable();
    }

    private void OnEnable()
    {
        inputs?.Enable();
    }
    private void OnDisable()
    {
        inputs?.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowedToMove) return;
        GlobalMediator.SendMessage(GameEvents.PLAYER_INPUT, new PlayerInputData
        {
            id = playerNumber,
            axis = inputs.Player.Movement.ReadValue<Vector2>(),
            key_charge = holdingCharge,
        });
    }
}
