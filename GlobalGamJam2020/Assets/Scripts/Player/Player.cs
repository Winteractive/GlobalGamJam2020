using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
    public int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        //Setup all player parts attached on this Gameobject
        List<PlayerPart> playerParts = new List<PlayerPart>();
        playerParts.AddRange(GetComponents<PlayerPart>());
        playerParts.AddRange(GetComponentsInChildren<PlayerPart>());
        foreach (var playerPart in playerParts)
        {
            playerPart.Initialize(playerNumber);
        }
    }
}
