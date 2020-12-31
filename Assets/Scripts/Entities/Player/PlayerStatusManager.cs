using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    static PlayerStatusManager instance = null;
    private PlayerController player;
    public PlayerController Player => player; 

    private int hitpoints;
    public int HitPoints => hitpoints;

    private int maxHitpoints;
    public int MaxHitpoints => maxHitpoints;

    private bool isAlive;
    public bool IsAlive => isAlive;

    private int levelNumber;
    public int LevelNumber => levelNumber;

    PlayerKnockbackMovement knockbackMovement;

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
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    private void Start() 
    {
        maxHitpoints = 5;
        hitpoints = 5;
        isAlive = true;
        levelNumber = 1;
        player = FindObjectOfType<PlayerController>();
        knockbackMovement = player.GetComponent<PlayerKnockbackMovement>();
    }

    public void ModifyHealth(int change)
    {
        hitpoints = Mathf.Clamp(hitpoints + change, 0, maxHitpoints);

        if (hitpoints <= 0)
        {
            hitpoints = 0;
            isAlive = false;
        }
    }

    public void ApplyKnockback(Vector2 knocked, float time)
    {
        if(knockbackMovement != null)
            knockbackMovement.KnockPlayer(knocked, time);
    }
}
