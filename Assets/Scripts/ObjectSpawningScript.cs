using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawningScript : MonoBehaviour
{
    [SerializeField] private GameObject spawnable;
    [SerializeField] private int intervalMin = 10;
    [SerializeField] private int intervalMax = 20;
    [SerializeField] private Vector3 startingDirection;
    [SerializeField] private Vector3 startingRotation;
    
    [SerializeField] private float speedMin = 1;
    [SerializeField] private float speedMax = 2;
    [SerializeField] private float lifetime = 20;

    [SerializeField] private BoxCollider boxCollider;
    
    
    private ObjectMovementScript _movementScript;

    private void Start()
    {
        boxCollider = GetComponentInChildren<BoxCollider>();
        print(boxCollider.bounds.center);
    }

    [SerializeField] private float remainingTime;
    // Update is called once per frame
    void Update()
    {
        if (remainingTime <= 0)
        {
            GameObject instance = Instantiate(spawnable);
            _movementScript = instance.GetComponentInChildren<ObjectMovementScript>();
            _movementScript.SetMovementDirection(startingDirection * Random.Range(speedMin, speedMax));
            _movementScript.SetRotationDirection(startingRotation * Random.Range(speedMin, speedMax));
            remainingTime = Random.Range(intervalMin, intervalMax);
            print(RandomPointInBounds(boxCollider.bounds));
            instance.transform.position = RandomPointInBounds(boxCollider.bounds);
            Destroy(instance, lifetime);
        }
        else
        {
            remainingTime -= Time.deltaTime;
        }
    }
    
    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

}
