using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    public class BusCameraFollow : MonoBehaviour
    {

        [SerializeField]
        Transform BusTransform;
        [SerializeField]
        float movementSmoothness = 0.6f;
        Vector3 BusCameraDistance;

        void Start()
        {
            BusCameraDistance = transform.position - BusTransform.position;
        }

        void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position,
            BusTransform.position + BusCameraDistance, movementSmoothness);
        }
    }
}


