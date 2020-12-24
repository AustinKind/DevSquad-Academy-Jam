using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderHelper : MonoBehaviour
{
    private PlayerLadderMovement playerInside = null;
    public bool HasPlayerInside => (playerInside != null);
    bool added = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLadderMovement playerLM = null;
        if ((playerLM = collision.GetComponentInChildren<PlayerLadderMovement>()))
        {
            if (!added) added = playerLM.AddLadder(this);
            playerInside = playerLM;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<PlayerLadderMovement>() == playerInside)
            playerInside = null;
    }
}
