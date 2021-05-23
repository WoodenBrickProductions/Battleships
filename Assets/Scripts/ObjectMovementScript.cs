using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovementScript : MonoBehaviour
{
    
    [SerializeField] private float amplitude = 0.0001f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private bool bob = false;
    [SerializeField] private bool rotate = false;
    
    private Vector3 _direction;
    private Vector3 _rotationDirection;
    private bool _setDirection = false;
    private float randomOffset;
    // Start is called before the first frame update
    void Start()
    {
        randomOffset = Random.value;
    }

    public void SetMovementDirection(Vector3 direction)
    {
        _direction = direction;
        _setDirection = true;
    }

    public void SetRotationDirection(Vector3 direction)
    {
        _rotationDirection = direction;
        rotate = true;
    }

    private void Move()
    {
        transform.position += _direction * Time.deltaTime;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_setDirection)
        {
            Move();
        }

        if (bob)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.y += Mathf.Sin ((Time.fixedTime + randomOffset) * Mathf.PI * frequency) * amplitude;
            transform.position = currentPosition;
        }

        if (rotate)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation += _rotationDirection * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
