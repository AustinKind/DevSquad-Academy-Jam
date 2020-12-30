using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Text text;
    private PlayerStatusManager psm;

    void Start()
    {
        text = GetComponent<Text>();
        psm = PlayerStatusManager.Instance;
    }

    void Update()
    {
        text.text = psm.HitPoints + "";
    }
}
