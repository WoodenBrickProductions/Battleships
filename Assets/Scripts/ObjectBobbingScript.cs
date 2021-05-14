using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBobbingScript : MonoBehaviour
{
    
    [SerializeField] private float amplitude = 0.0001f;
    [SerializeField] private float frequency = 1f;

    private float randomOffset;
    // Start is called before the first frame update
    void Start()
    {
        randomOffset = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y += Mathf.Sin ((Time.fixedTime + randomOffset) * Mathf.PI * frequency) * amplitude;
        transform.position = currentPosition;
    }
}
