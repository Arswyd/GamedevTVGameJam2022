// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform pair")]
    [SerializeField] MovingPlatform platformPair;

    [Header("Move settings")]
    [SerializeField] Vector2 offset;
    [SerializeField] float speed;

    Vector2 startPosition;
    Vector2 minPosition;
    Vector2 maxPosition;
    Vector2 targetPosition;

    void Awake()
    {
        startPosition = transform.position;
        minPosition = startPosition - offset;
        maxPosition = startPosition + offset;
        targetPosition = startPosition;
    }

    void Update()
    {
        MoveToTargetPosition(targetPosition);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            SetTargetPosition(-1);
            if (platformPair != null)
            {
                platformPair.SetTargetPosition(1);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            SetTargetPosition(0);
            if (platformPair != null)
            {
                platformPair.SetTargetPosition(0);
            }
        }
    }

    void MoveToTargetPosition(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime); 
    }

    public void SetTargetPosition(int value)
    {
        if(value == 0)
        {
            targetPosition = startPosition;
        }
        else if(value == 1)
        {
            targetPosition = maxPosition;
        }
        else if(value == -1)
        {
            targetPosition = minPosition;
        }
    }
}
