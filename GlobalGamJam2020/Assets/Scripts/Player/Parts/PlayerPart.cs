using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPart : MonoBehaviour
{
    protected int playerNumber;
    public void Initialize(int playerNumber)
    {
        this.playerNumber = playerNumber;
        CustomStart();
    }
    public virtual void CustomStart()
    {

    }

}
