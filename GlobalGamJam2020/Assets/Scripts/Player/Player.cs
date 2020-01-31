using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerNumber))]
public class Player : MonoBehaviour
{
    PlayerNumber playerNumber;
    // Start is called before the first frame update
    void Start()
    {
        playerNumber = GetComponent<PlayerNumber>();
        List<PlayerPart> playerParts = new List<PlayerPart>();
        playerParts.AddRange(GetComponents<PlayerPart>());
        playerParts.AddRange(GetComponentsInChildren<PlayerPart>());
        foreach (var playerPart in playerParts)
        {
            playerPart.Initialize(playerNumber);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
