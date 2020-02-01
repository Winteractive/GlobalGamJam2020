using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitAnimationTest : MonoBehaviour
{
    public UnitAnimator unitAnimator;

    private void Start()
    {
        unitAnimator.Initialize(1);
    }

    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            unitAnimator.TryStartAnimation("Idle");
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            unitAnimator.TryStartAnimation("Idle");
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            unitAnimator.TryStartAnimation("NotMade");
        }
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            unitAnimator.TryStartAnimation("ChargeHold");
        }
    }
}
