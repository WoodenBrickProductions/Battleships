using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class BoatController : MonoBehaviour
{
    // Start is called before the first frame update


    private Vector3 _gridPosition;
    private Vector3 _startingPosition;
    private bool _movable;
    private List<ObstacleTrigger> _obstacles;
    [SerializeField] private int size = 1;
    
    private void OnTriggerEnter(Collider other)
    {
        //TODO Set state in Boat Controller for PositionTrigger
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
        for (int i = 0; i < size - 1; i++)
        {
            _obstacles.Add(transform.GetChild(i).GetComponentInChildren<ObstacleTrigger>());
        }
        _startingPosition = transform.position;
        _gridPosition = _startingPosition;
        _movable = true;
        _obstacles = new List<ObstacleTrigger>();
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
            foreach (var obstacle in _obstacles)
            {
                if (obstacle.IsObstructed())
                {
                    _gridPosition = _startingPosition;
                    transform.position = _gridPosition;
                    return;
                }
            }
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
