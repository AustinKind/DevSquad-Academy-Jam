using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatRange
{
    public float min;
    public float max;

    public FloatRange()
    {
        min = 0; max = 1;
    }

    public FloatRange(float mn, float mx)
    {
        min = mn; max = mx;
    }

    public void ClampToRange(ref float value)
    {
        value = Mathf.Clamp(value, min, max);
    }

    public float EvaluatePercent(float percent)
    {
        return (Difference * percent) + min;
    }

    public float Difference => (max - min);
}
