using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapSkeleton : GoapAgent
{
    new void Start()
    {
        base.Start();
        goals.Add(new SubGoal("playerProtected", 1, false), 1);
    }
}
