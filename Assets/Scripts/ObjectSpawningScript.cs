using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawningScript : MonoBehaviour
{
    [SerializeField] private GameObject spawnable;
    [SerializeField] private int intervalMin = 1;
    [SerializeField] private int intervalMax = 2;

    [SerializeField] private float speedMin = 1;
    [SerializeField] private float speedMax = 2;

    private float remainingTime;
    // Update is called once per frame
    void Update()
    {
        if (remainingTime <= 0)
        {
            
            
            remainingTime = Random.value * (intervalMax - intervalMin) + intervalMin;
        }
        else
        {
            remainingTime -= Time.deltaTime;
        }
    }
}
