using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendingMailPuzzle : Puzzle
{
    public ComputerScreenUI computer;
    public bool IsCompleted;
    private void Awake()
    {
        computer.puzzle = this;
    }

    public override void GetReward()
    {
        base.GetReward();
        IsCompleted = true;

        // TODO : Puzzle3 완료 후 할 일
    }
}
