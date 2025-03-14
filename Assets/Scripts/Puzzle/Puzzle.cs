using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleCheckable
{
    public bool IsCorrect();
}

[Serializable]
public class PuzzleReward
{
    public GameObject RewardHint;
    public int Score;
}

public class Puzzle : MonoBehaviour
{
    public PuzzleReward Reward;

    public virtual void GetReward()
    {
        if (Reward.RewardHint != null) 
            Reward.RewardHint.SetActive(true);

        // TODO : 그외 보상 지급
    }
}
