using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableGridUnit : GridUnit
{
    protected override void HandleDeath()
    {
        //Don't relay anything to the gamemanager.
    }
}
