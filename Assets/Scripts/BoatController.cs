using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    // Start is called before the first frame update


    private Vector3 _gridPosition;
    private Vector3 _startingPosition;
    private bool _movable;
    private List<PositionTrigger> _obstacles;
    
    private void OnTriggerEnter(Collider other)
    {
        print("Boat OnTriggerEnter was called by " + other.gameObject.name);
        if (other.gameObject.tag == "Position")
        {
            PositionTrigger position = other.GetComponentInChildren<PositionTrigger>();
            if (!position.IsOccupied())
            {
                _gridPosition = position.gameObject.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.position == _gridPosition)
        {
            _gridPosition = _startingPosition;
        }
}

    void Start()
    {
        _startingPosition = transform.position;
        _gridPosition = _startingPosition;
        _movable = true;
        _obstacles = new List<PositionTrigger>();
    }

    public bool IsMovable()
    {
        return _movable;
    }
    
    public bool ChangedPosition()
    {
        return _gridPosition != _startingPosition;
    }

    public void SnapToGridPosition()
    {
        if (ChangedPosition())
        {
            _movable = false;
        }
        transform.position = _gridPosition;
    }

    public void ResetPosition()
    {
        transform.position = _startingPosition;
        _movable = true;
    }
}
