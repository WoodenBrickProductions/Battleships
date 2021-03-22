using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    private Dictionary<Collider, PositionTrigger> obstructions;
    private PositionTrigger _currentPosition;
    
    private void Start()
    {
        obstructions = new Dictionary<Collider, PositionTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Position")
        {
            PositionTrigger position = other.GetComponentInChildren<PositionTrigger>();
            if (position.IsOccupied())
            {
                obstructions.Add(other, position);
            }
            else
            {
                _currentPosition = position;
                position.SetOccupied(this.gameObject);
            }
            print("SOMETHING INSIDE Me");
        }
    }

    public int GetObstructionCount()
    {
        return obstructions.Count;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Position")
        {
            if(obstructions.ContainsKey(other))
            {
                obstructions.Remove(other);
            }
        }
    }

    public bool IsObstructed()
    {
        return obstructions.Count != 0;
    }
}
