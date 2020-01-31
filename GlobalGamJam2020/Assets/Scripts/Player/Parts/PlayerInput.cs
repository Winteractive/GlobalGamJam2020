using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : PlayerPart
{
    InputSystemControls inputs;
    bool holdingCharge;

    public override void CustomStart()
    {
        InputManager.SetPlayerNumberUpdateDevice(playerNumber, InputManager_OnChanges);
        InputManager_OnChanges();
        inputs.Player.Charge.started += Charge_started;
        inputs.Player.Charge.canceled += Charge_canceled;
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

    public struct InputMessage
    {
        public int playerNumber;
        public Vector2 leftStick;
        public bool charge;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(holdingCharge);
        
        GlobalMediator.SendMessage(GameEvents.PLAYER_INPUT, new InputMessage
        {
            playerNumber = playerNumber,
            leftStick = inputs.Player.Movement.ReadValue<Vector2>(),
            charge = holdingCharge,
        });
    }
}
