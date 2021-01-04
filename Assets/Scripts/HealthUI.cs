using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Image image;
    private PlayerStatusManager psm;
    public Sprite health0;
    public Sprite health1;
    public Sprite health2;
    public Sprite health3;
    public Sprite health4;
    public Sprite health5;
    private int currentHP = 5;

    void Start()
    {
        image = GetComponent<Image>();
        psm = PlayerStatusManager.Instance;
    }

    void Update()
    {
        if (psm.HitPoints != currentHP)
        {
            if (psm.HitPoints == 5)
            {
                image.sprite = health5;
                currentHP = 5;
            } else if (psm.HitPoints == 4)
            {
                image.sprite = health4;
                currentHP = 4;
            } else if (psm.HitPoints == 3)
            {
                image.sprite = health3;
                currentHP = 3;
            } else if (psm.HitPoints == 2)
            {
                image.sprite = health2;
                currentHP = 2;
            } else if (psm.HitPoints == 1)
            {
                image.sprite = health1;
                currentHP = 1;
            } else
            {
                image.sprite = health0;
                currentHP = 0;
            }
                
        }
        
    }
}
