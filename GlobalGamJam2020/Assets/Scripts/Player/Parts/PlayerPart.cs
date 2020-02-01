using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPart : MonoBehaviour
{
    public int playerNumber;

    //Runs from Player. And gets the playerNumber accosiaded with it
    public virtual void Initialize(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }

}
