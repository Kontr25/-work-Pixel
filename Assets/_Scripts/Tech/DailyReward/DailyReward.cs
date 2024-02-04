using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DailyReward
{
    [SerializeField] private DailyRewardType _type;
    [SerializeField] private int _value;

    public DailyRewardType Type => _type;
    public int Value => _value;
}
