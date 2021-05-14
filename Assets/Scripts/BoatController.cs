﻿using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class BoatController : MonoBehaviour, IHittable
{
    // Start is called before the first frame update


    [SerializeField] private Vector3 _gridPosition;
    private Vector3 _startingPosition;
    private Quaternion _startingRotation;
    private bool _movable;
    private List<ObstacleTrigger> _obstacles;
    [SerializeField] private int size = 1;
    [SerializeField] private bool hit = false;

    [SerializeField] private TileTrigger occupiedTile;

    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;

    private GameObject boat, destroyedBoat;
    private float randomOffset;
    
    public void Update()
    {
        Vector3 currentPosition = boat.transform.position;
        currentPosition.y += Mathf.Sin ((Time.fixedTime + randomOffset) * Mathf.PI * frequency) * amplitude;
        boat.transform.position = currentPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO Set state in Boat Controller for PositionTrigger
        print("Boat OnTriggerEnter was called by " + other.gameObject.name);
        if (other.gameObject.tag == "Position")
        {
            PositionTrigger position = other.GetComponentInChildren<PositionTrigger>();
            if (!position.IsOccupied())
            {
                position.SetOccupied(this.gameObject);
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
        randomOffset = Random.value;
        boat = transform.GetChild(transform.childCount - 2).gameObject;
        destroyedBoat = transform.GetChild(transform.childCount - 1).gameObject;

        _obstacles = new List<ObstacleTrigger>();
        for (int i = 0; i < size - 1; i++)
        {
            _obstacles.Add(transform.GetChild(i).GetComponentInChildren<ObstacleTrigger>());
        }
        _startingPosition = transform.position;
        _startingRotation = transform.rotation;
        _gridPosition = _startingPosition;
        _movable = true;
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
        print("Changed position" + ChangedPosition());
        if (ChangedPosition())
        {
            foreach (var obstacle in _obstacles)
            {
                print(obstacle.GetObstructionCount());
                if (obstacle.IsObstructed())
                {
                    _gridPosition = _startingPosition;
                    print("Exiting");
                    _movable = true;
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
        transform.rotation = _startingRotation;
        _movable = true;
    }

    public bool IsDestroyed()
    {
        if (hit)
        {
            foreach (var obstacle in _obstacles)
            {
                if (!obstacle.IsHit())
                {
                    return false;
                }
            }

            boat.SetActive(false);
            destroyedBoat.SetActive(true);
            return true;
        }

        return false;
    }
    
    public bool IsHit()
    {
        return hit;
    }

    public void Hit()
    {
        hit = true;
    }
}
