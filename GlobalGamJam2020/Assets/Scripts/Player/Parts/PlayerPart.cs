using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPart : MonoBehaviour
{
    protected PlayerNumber playerNumber;
    public virtual void Initialize(PlayerNumber playerNumber)
    {
        this.playerNumber = playerNumber;
    }

}
