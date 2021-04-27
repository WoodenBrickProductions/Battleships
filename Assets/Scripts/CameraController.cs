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
    [SerializeField] private float maxSpeed = 6f;
    
    private float _distanceToLocation;
    private Vector3 _targetLocation;
    private float _factor;
    private bool _moving = false;
    private bool changeCamera = false;
    private void Start()
    {
        _targetLocation = playerBoardPosition;
    }

    void Update()
    {
        if (changeCamera && !_moving)
        {
            if (transform.position == playerBoardPosition)
            {
                _targetLocation = enemyBoardPosition;
                _distanceToLocation = (_targetLocation - transform.position).magnitude;
                _moving = true;
                changeCamera = false;
            }
            else
            {
                _targetLocation = playerBoardPosition;
                _distanceToLocation = (_targetLocation - transform.position).magnitude;
                _moving = true;
                changeCamera = false;
            }
        }

        if (_targetLocation != transform.position)
        {
             _factor = (_distanceToLocation + _distanceToLocation * smoothing) / // Travel speed goes from 1 to 2 to 1
                           Math.Abs(_distanceToLocation/2 - (_targetLocation - transform.position).magnitude + _distanceToLocation * smoothing);
             _factor = Mathf.Clamp((float)Math.Sqrt(_factor), 0, maxSpeed);
             
            transform.position = Vector3.MoveTowards(transform.position, _targetLocation, cameraSpeed * _factor * Time.deltaTime);
        }
        else
        {
            _moving = false;
        }

    }

    public void ChangeCameraPosition()
    {
        changeCamera = true;
    }
}
