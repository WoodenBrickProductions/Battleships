using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject _selectionCircle;
    [SerializeField] private GameObject _occupiedObject;
    private TileTrigger _tileTrigger;
    
    private void OnTriggerEnter(Collider other)
    {
        print(gameObject + " PositionTrigger is called, entered: " + other.gameObject + " " + IsOccupied());
        if (!other.gameObject.CompareTag("EnemyBoat"))
        {
            _selectionCircle.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(_occupiedObject))
        {
            SetOccupied(null);
            _selectionCircle.SetActive(false);
        }
    }

    public void SetOccupied(GameObject gameObject)
    {
        _occupiedObject = gameObject;
        _tileTrigger.SetOccupied(gameObject);
    }
    
    public bool IsOccupied()
    {
        return _occupiedObject != null;
    }

    
    void Start()
    {
        _selectionCircle = transform.GetChild(0).gameObject;
        _tileTrigger = GetComponentInParent<TileTrigger>();
    }
}
