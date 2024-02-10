using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIContext
{
    const int SIZE = 16;
    const float ANGLE_MULTIPLIER = (2.0f * Mathf.PI) / SIZE;

    public List<float> weights = new List<float>();


    public float factor = 1.0f;
    public float angle = 0f;

    public int size
    {
        get
        {
            return SIZE;
        }
    }

    public AIContext()
    {
        for (int i = 0; i < size; i++)
        {
            weights.Add(0);
        }
    }

    public void SetWeight(int index, float value)
    {
        if (index >= 0 && index < weights.Count)
        {
            weights[index] = value;
        }
    }

    public float GetWeight(int index)
    {
        if (index >= 0 && index < weights.Count)
        {
            return weights[index] * factor;
        }
        else
        {
            return Mathf.NegativeInfinity;
        }
    }

    public Vector2 GetDesiredDirection()
    {
        return GetNormal(GetMaxIndex());
    }

    public Vector2 GetNormal(int index)
    {
        if (index >= 0 && index < size)
        {
            return Polar2Cartesian(1.0f, GetAngle(index));
        }
        else
        {
            return new Vector2(0.0f, 0f);
        }
    }

    public void Combine(List<AIContext> contexts)
    {
        for(int i = 0; i < size; i++)
        {
            weights[i] = 0;
            foreach(var c in contexts)
            {
                weights[i] += c.GetWeight(i);
            }
        }
    }


    public static Vector2 Polar2Cartesian(float r, float theta)
    {
        Vector2 res = Vector2.zero;
        res.x = r * Mathf.Cos(theta);
        res.y = r * Mathf.Sin(theta);

        return res;

    }

    public static float MapValue(float value, float min_value, float max_value)
    {
        var map = (value - min_value) / (max_value - min_value);
        return Mathf.Clamp01(map);
    }

    public int GetMaxIndex(float min_weight = 0f)
    {
        int idx = -1;
        float max_weight = min_weight;

        for (int i = 0; i < size; i++)
        {
            float weight = GetWeight(i);
            if (weight > max_weight)
            {
                max_weight = weight;
                idx = i;
            }
        }
        return idx;
    }

    public void Clear()
    {
        for(int i = 0; i < size; i++)
        {
            weights[i] = 0;
        }
    }
    public float GetAngle(int index)
    {
        var ret = ANGLE_MULTIPLIER * index;
        angle = Mathf.Rad2Deg * ret;
        return ret;
    }

    public string toString()
    {
        return weights.ToString();
    }
}
