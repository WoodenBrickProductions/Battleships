using System;
using System.Collections;
using System.Collections.Generic;using DefaultNamespace;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour, IHittable
{
    private Dictionary<Collider, PositionTrigger> obstructions;
    [SerializeField] private PositionTrigger _currentPosition;
    [SerializeField] private int count;
    [SerializeField] private bool hit = false;
    
    private void Start()
    {
        obstructions = new Dictionary<Collider, PositionTrigger>();
        count = obstructions.Count;
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
            count = obstructions.Count;

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
            if (other.GetComponentInChildren<PositionTrigger>().Equals(_currentPosition))
            {
                _currentPosition = null;
            }
            if(obstructions.ContainsKey(other))
            {
                obstructions.Remove(other);
            }
            count = obstructions.Count;
        }
    }

    public void Hit()
    {
        hit = true;
    }
    
    public bool IsHit()
    {
        return hit;
    }
    
    public bool IsObstructed()
    {
        return obstructions.Count != 0;
    }

    public bool InPosition()
    {
        return _currentPosition != null;
    }
}
