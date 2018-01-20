using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Unicam.AgentSimulator.Model.Bus
{
    /// <summary>
    /// An Axle is a pair of wheels in the bus. This class contains all the informations
    /// needed. It's serializable, so it can be modified directly in the unity inspector.
    /// </summary>
    [System.Serializable]
    public class AxleBusInfo
    {
        public WheelCollider leftWheel;
        public GameObject leftWheelMesh;
        public WheelCollider rightWheel;
        public GameObject rightWheelMesh;

        //is the axle attached to the motor? 
        public bool motor;

        //Does this wheel apply a steer angle?
        public bool steering;

    }
}
