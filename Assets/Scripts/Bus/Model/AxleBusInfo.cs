using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus.Model
{
    /// <summary>
    /// An Axle is a pair of wheels in the bus. This class contains all the informations
    /// needed. It's serializable, so it can be modified directly in the unity inspector.
    /// </summary>
    [System.Serializable]
    public class AxleBusInfo
    {
        [Tooltip("Bus left wheel")]
        public WheelCollider leftWheel;
        [Tooltip("Bus left wheel mesh")]
        public GameObject leftWheelMesh;
        [Tooltip("Bus right wheel")]
        public WheelCollider rightWheel;
        [Tooltip("Bus right wheel mesh")]
        public GameObject rightWheelMesh;

        [Tooltip("Is the axle attached to the motor?")]
        public bool motor;

        [Tooltip("Does this wheel apply a steer angle?")]
        public bool steering;

    }
}
