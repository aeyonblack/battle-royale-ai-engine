using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I don't know what the purpose of these monsters is but I just have them
/// </summary>
public class GoapMonster : GoapAgent
{
    new void Start()
    {
        base.Start();

        goals.Add(new SubGoal("threatsAverted", 1, false), 1);  
    }

}
