using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 playerBoardPosition;
    [SerializeField] private Vector3 enemyBoardPosition;
    [SerializeField] private float cameraSpeed = 3f;
    [SerializeField] private float smoothing = 0.2f;
    
    private float _distanceToLocation;
    private Vector3 _targetLocation;
    private float _factor;
    private bool _moving = false;
    private void Start()
    {
        _targetLocation = playerBoardPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_moving)
        {
            if (transform.position == playerBoardPosition)
            {
                _targetLocation = enemyBoardPosition;
                _distanceToLocation = (_targetLocation - transform.position).magnitude;
                _moving = true;
            }
            else
            {
                _targetLocation = playerBoardPosition;
                _distanceToLocation = (_targetLocation - transform.position).magnitude;
                _moving = true;
            }
        }

        if (_targetLocation != transform.position)
        {
             _factor = (_distanceToLocation + _distanceToLocation * smoothing) / // Travel speed goes from 1 to 2 to 1
                           Math.Abs(_distanceToLocation/2 - (_targetLocation - transform.position).magnitude + _distanceToLocation * smoothing);
             _factor = (float) Math.Sqrt(_factor);
            transform.position = Vector3.MoveTowards(transform.position, _targetLocation, cameraSpeed * _factor * Time.deltaTime);
        }
        else
        {
            _moving = false;
        }

    }
}
