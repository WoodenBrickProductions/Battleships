using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    [SerializeField] private GameObject occupiedObject;
    private GameObject marker;
    private bool marked = false;
    
    // Start is called before the first frame update
    void Start()
    {
        marker = transform.GetChild(2).gameObject;
        marker.SetActive(false);
        occupiedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetOccupiedObject()
    {
        return occupiedObject;
    }
    
    public void SetOccupied(GameObject gameObject)
    {
        occupiedObject = gameObject;
    }

    public void SetMarked()
    {
        marker.SetActive(true);
        marked = true;
    }

    public bool IsMarked()
    {
        return marked;
    }
}
