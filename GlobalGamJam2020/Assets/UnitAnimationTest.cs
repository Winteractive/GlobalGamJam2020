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
            unitAnimator.TryStartAnimation(UnitAnimator.Character.Blue, "Idle");
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            unitAnimator.TryStartAnimation(UnitAnimator.Character.Pink, "Idle");
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            unitAnimator.TryStartAnimation(UnitAnimator.Character.Blue, "ChargeUp");
        }
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            unitAnimator.TryStartAnimation(UnitAnimator.Character.Blue, "ChargeHold");
        }
    }
}
