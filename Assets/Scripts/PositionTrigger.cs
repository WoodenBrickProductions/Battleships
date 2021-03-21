using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject _selectionCircle;
    [SerializeField] private GameObject _occupiedObject;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_occupiedObject == null)
        {
            _selectionCircle.SetActive(true);
            _occupiedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(_occupiedObject))
        {
            _selectionCircle.SetActive(false);
            _occupiedObject = null;
        }
    }

    public bool IsOccupied()
    {
        return _occupiedObject != null;
    }
    
    void Start()
    {
        _selectionCircle = transform.GetChild(0).gameObject;
    }
}
