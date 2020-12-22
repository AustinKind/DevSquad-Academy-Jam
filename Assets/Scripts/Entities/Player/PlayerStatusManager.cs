using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    static PlayerStatusManager instance = null;

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
}
