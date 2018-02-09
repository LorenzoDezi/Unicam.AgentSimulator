using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicam.AgentSimulator.Scripts.Bus.Model;
using System;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    public class BusDrive : MonoBehaviour
    {
        [Header("Axles")]
        /// <summary>
        /// List of wheel pairs.
        /// </summary>
        [SerializeField]
        List<AxleBusInfo> axleInfos;
        /// <summary>
        /// A ref to the motor Axle.
        /// </summary>
        public AxleBusInfo motorAxleInfo;

        [Header("Motor parameters")]
        /// <summary>
        /// Maximum torque the motor can apply to the wheels.
        /// </summary>
        [SerializeField]
        public float maxMotorTorque = 80f;
        [SerializeField]
        public float maxSpeed = 60f;
        /// <summary>
        /// The maximum angle the wheels can reach
        /// </summary>
        [SerializeField]
        public float maxSteeringAngle = 45f;
        /// <summary>
        /// The center of mass of the vehicle. 
        /// It will be slightly below the bottom center of the bus.
        /// </summary>
        [SerializeField]
        public Vector3 centerOfMass = new Vector3(0, -0.1f, 0);
        [SerializeField]
        float currentSpeed;

        [Header("Curve parameters")]
        [SerializeField]
        float curveApproachDistance = 50f;
        [SerializeField]
        float turnSpeed = 2f;




        private void Start()
        {
            //Initializing axles
            foreach(AxleBusInfo axleInfo in axleInfos)
            {
                if(axleInfo.motor)
                {
                    motorAxleInfo = axleInfo;
                }
            }
            this.GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        }

        void ApplyLocalPositionToVisuals(AxleBusInfo axle)
        {
            Vector3 position;
            Quaternion rotation;

            //Applying the new position and rotation to the left wheel
            axle.leftWheel.GetWorldPose(out position, out rotation);
            axle.leftWheelMesh.transform.position = position;
            axle.leftWheelMesh.transform.rotation = rotation;

            //... and to the right one
            axle.rightWheel.GetWorldPose(out position, out rotation);
            axle.rightWheelMesh.transform.position = position;
            axle.rightWheelMesh.transform.rotation = rotation;
            
        }

        public void ApplyMovement(Vector3 directionWayPoint, Vector3 directionNextWayPoint)
        {
            float actualSteering = (directionWayPoint.x / directionWayPoint.magnitude) * maxSteeringAngle;
            if(directionWayPoint.magnitude < curveApproachDistance)
            {
                //nextWayPointDirection is used to detect if there is a dangerous curve approaching
                //its range goes from -1 to 1
                float nextWayPointDirection = (directionNextWayPoint.x / directionNextWayPoint.magnitude);
                if(nextWayPointDirection > 0.5f || nextWayPointDirection < -0.5f)
                {
                    actualSteering = nextWayPointDirection * maxSteeringAngle;
                    //At this point the transition is at the next waypoint
                    this.GetComponent<BusController>().transitionDone = true;
                }
            }
            


            foreach(AxleBusInfo axleInfo in axleInfos)
            {
                //Check the steering
                if(axleInfo.steering)
                {
                    //Applying the steering to the axle. The lerping grants a smooth transition
                    axleInfo.leftWheel.steerAngle = Mathf.Lerp(axleInfo.leftWheel.steerAngle, actualSteering, Time.fixedDeltaTime * turnSpeed);
                    axleInfo.rightWheel.steerAngle = Mathf.Lerp(axleInfo.rightWheel.steerAngle, actualSteering, Time.fixedDeltaTime * turnSpeed);
                }

                //Check the motor
                if(axleInfo.motor)
                {
                    currentSpeed = 2 * Mathf.PI * axleInfo.leftWheel.radius * axleInfo.leftWheel.rpm * 60 / 1000;
                    if (currentSpeed < maxSpeed)
                    {
                        //Applying the motor to the axle
                        axleInfo.leftWheel.motorTorque = maxMotorTorque;
                        axleInfo.rightWheel.motorTorque = maxMotorTorque;
                    }
                    else
                    {
                        axleInfo.leftWheel.motorTorque = 0;
                        axleInfo.rightWheel.motorTorque = 0;
                    }
                    
                }

                ApplyLocalPositionToVisuals(axleInfo);
            }
        }
    }
}




