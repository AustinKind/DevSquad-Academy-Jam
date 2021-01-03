using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public GameObject deathSprite;
    public GameObject characterSprite;

    public void Die()
    {
        characterSprite.SetActive(false);
        deathSprite.SetActive(true);
    }
}
