using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPart : MonoBehaviour
{
    protected int playerNumber;

    //Runs from Player. And gets the playerNumber accosiaded with it
    public void Initialize(int playerNumber)
    {
        this.playerNumber = playerNumber;
        CustomStart();
    }
    /// <summary>
    /// Runs on Player's Start function. Use this instead of Unitys Start. 
    /// </summary>
    public virtual void CustomStart()
    {

    }

}
