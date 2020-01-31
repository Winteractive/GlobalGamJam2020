using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerNumber playerNumber;
    InputSystemControls inputs;

    // Start is called before the first frame update
    void Start()
    {
        playerNumber = GetComponent<PlayerNumber>();
        InputManager.SetPlayerNumberUpdateDevice(playerNumber.id, InputManager_OnChanges);
    }

    private void InputManager_OnChanges()
    {
        inputs?.Disable();
        inputs = InputManager.GetInputActions(playerNumber.id);
        inputs.Enable();
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
        GlobalMediator.SendMessage(GameEvents.PLAYER_INPUT, new InputMessage
        {
            playerNumber = playerNumber.id,
            leftStick = inputs.Player.Movement.ReadValue<Vector2>(),
            charge = false,
        });
    }
}
