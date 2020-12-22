using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    static PlayerStatusManager instance = null;
    private int hitpoints;

    public static PlayerStatusManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (PlayerStatusManager)FindObjectOfType(typeof(PlayerStatusManager));
                if (instance == null)
                    instance = (new GameObject("_PlayerStatusManager")).AddComponent<PlayerStatusManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        maxHitpoints = 5;
        hitpoints = 5;
        isAlive = true;
        levelNumber = 1;
    }

    public void modifyHealth(int change)
    {
        hitpoints += change;
        if (hitpoints > maxHitpoints)
        {
            hitpoints = maxHitpoints;
        }
        else if (hitpoints < 0)
        {
            hitpoints = 0;
            isAlive = false;
        }
    }

    public int maxHitpoints { get; set; }

    public bool isAlive { get; set; }

    public int levelNumber { get; set; }
}
