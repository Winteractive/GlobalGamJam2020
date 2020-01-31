﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerNumber playerNumber;
    InputSystemControls inputs;
    bool holdingCharge;
    // Start is called before the first frame update
    void Start()
    {
        playerNumber = GetComponent<PlayerNumber>();
        InputManager.SetPlayerNumberUpdateDevice(playerNumber.id, InputManager_OnChanges);
        InputManager_OnChanges();
        inputs.Player.Charge.started += Charge_started;
        inputs.Player.Charge.canceled += Charge_canceled; ;
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
        inputs = InputManager.GetInputActions(playerNumber.id);
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
            playerNumber = playerNumber.id,
            leftStick = inputs.Player.Movement.ReadValue<Vector2>(),
            charge = holdingCharge,
        });
    }
}
