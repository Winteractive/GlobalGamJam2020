using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MomentumTransfer : PlayerPart
{
    [SerializeField]float secretMultiplier = 2;
    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
    } 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Vector3 positionFromMe = transform.position - collision.transform.position;
        positionFromMe.Normalize();
        if (Vector2.Dot(Vector2.up,positionFromMe) >= 0)
        {
            collision.otherRigidbody.velocity = collision.rigidbody.velocity * secretMultiplier;
            collision.rigidbody.velocity = Vector2.zero;
        }
    }
}
