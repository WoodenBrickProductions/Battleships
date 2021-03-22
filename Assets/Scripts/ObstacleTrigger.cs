using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    [SerializeField] private bool obstructed;
    
    private void OnTriggerStay(Collider other)
    {
        obstructed = true;
        print("SOMETHING INSIDE Me");
    }

    private void OnTriggerExit(Collider other)
    {
        obstructed = false;
    }

    public bool IsObstructed()
    {
        return obstructed;
    }
}
