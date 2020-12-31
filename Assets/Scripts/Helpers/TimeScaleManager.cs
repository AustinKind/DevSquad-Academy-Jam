using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    static TimeScaleManager instance = null;
    public static TimeScaleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (TimeScaleManager)FindObjectOfType(typeof(TimeScaleManager));
                if (instance == null)
                    instance = (new GameObject("_TimeScaleManager")).AddComponent<TimeScaleManager>();
            }
            return instance;
        }
    }

    [Range(0f, 1f)]
    private float currentTimeScale = 1f;

    float defaultFixedDeltaTime;

    List<TimeAdjuster> adjustments = new List<TimeAdjuster>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        //Initialize time and gravity to initial values
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public int AdjustTimeScale(TimeAdjuster adjustTime)
    {
        int index = adjustments.Count;
        adjustments.Add(adjustTime);
        return index;
    }

    private void Update()
    {
        List<TimeAdjuster> currentAdjusters = new List<TimeAdjuster>();
        foreach(TimeAdjuster adjuster in adjustments)
        {
            if (adjuster.AdjustCheck.Invoke())
                currentAdjusters.Add(adjuster);
        }

        float min = 1f;
        for(int i = 0; i < currentAdjusters.Count; i++)
        {
            TimeAdjuster adjuster = currentAdjusters[i];

            float val = adjuster.value;
            if (val < min)
                min = val;
        }

        currentTimeScale = min;
        Time.timeScale = currentTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    public void AdjustScaler(int index, float val)
    {
        adjustments[index].value = Mathf.Clamp(val, 0f, 1f);
    }
}

public class TimeAdjuster
{
    public float value;
    public BoolAction AdjustCheck;

    public TimeAdjuster(float val, BoolAction AdjustAction)
    {
        value = Mathf.Clamp(val, 0f, 1f);
        AdjustCheck = AdjustAction;
    }
}