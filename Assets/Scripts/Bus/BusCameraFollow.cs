using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusCameraFollow : MonoBehaviour {

    [SerializeField]
    Transform BusTransform;

    [SerializeField]
    float smoothness = 0.6f;

    Vector3 BusCameraDistance;


    void Start()
    {
        BusCameraDistance = transform.position - BusTransform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,
        BusTransform.position + BusCameraDistance, smoothness);
    }
}
