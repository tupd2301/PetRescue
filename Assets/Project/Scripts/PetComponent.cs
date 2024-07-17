using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetComponent : MonoBehaviour
{
    public PetData petData;

    public void SetData(PetData petData)
    {
        this.petData = petData;
    }

    public void Run()
    {
        
    }
}
