using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building : MonoBehaviour
{
    public float health, damage, built, rCost, bCost, gCost;
    public bool flipped;
    public Transform self;

    public void InitBuilding()
    {
        self = this.transform;
    }
}
