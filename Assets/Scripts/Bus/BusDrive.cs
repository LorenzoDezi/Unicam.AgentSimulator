using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicam.AgentSimulator.Scripts.Bus.Model;
using System;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    /// <summary>
    /// It implements the bus drive implementation, using waypoint between each known position 
    /// to have a better trajectory.
    /// </summary>
    public class BusDrive : MonoBehaviour
    {
        [Header("Axles")]
        /// <summary>
        /// List of wheel pairs.
        /// </summary>
        [SerializeField]
        [Tooltip("A list of the bus wheel pairs")]
        List<AxleBusInfo> axleInfos;

        /// <summary>
        /// A ref to the motor Axle.
        /// </summary>
        public AxleBusInfo MotorAxleInfos;

        [Header("Motor parameters")]
        /// <summary>
        /// Maximum torque the motor can apply to the wheels.
        /// </summary>
        [SerializeField]
        [Tooltip("Torque applied")]
        public float MotorTorque = 80f;
        [SerializeField]
        [Tooltip("Maximum speed of the bus, in km/h")]
        public float MaxSpeed = 60f;
        /// <summary>
        /// The maximum angle the wheels can reach
        /// </summary>
        [SerializeField]
        [Tooltip("Maximum angle the bus wheels can reach")]
        public float MaxSteeringAngle = 45f;
        /// <summary>
        /// The center of mass of the vehicle. 
        /// It will be slightly below the bottom center of the bus.
        /// </summary>
        [SerializeField]
        [Tooltip("The center of mass, slightly below the bottom center of the bus")]
        public Vector3 CenterOfMass = new Vector3(0, -0.1f, 0);

        [HideInInspector]
        public float currentSpeed;

        [Header("Curve parameters")]
        [SerializeField]
        float curveApproachDistance = 50f;
        [SerializeField]
        float turnSpeed = 2f;




        private void Start()
        {
            //Initializing axles
            if(axleInfos != null)
                foreach(AxleBusInfo axleInfo in axleInfos)
                {
                    if(axleInfo.motor)
                    {
                        MotorAxleInfos = axleInfo;
                    }
                }
                this.GetComponent<Rigidbody>().centerOfMass = CenterOfMass;
        }

        /// <summary>
        /// Update the wheel mesh position/rotation with the
        /// physics value of the axle.
        /// </summary>
        /// <param name="axle"></param>
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

        /// <summary>
        /// Update the bus axle <paramref name="axleInfo"/> 
        /// with the proper steering defined in <paramref name="actualSteering"/>
        /// and with the torque needed until the bus reached its max speed.
        /// <param name="axleInfo"></param>
        /// <param name="actualSteering"></param>
        void UpdateAxle(AxleBusInfo axleInfo, float actualSteering)
        {
            //Check the steering
            if (axleInfo.steering)
            {
                //Applying the steering to the axle. The lerping grants a smooth transition
                axleInfo.leftWheel.steerAngle = Mathf.Lerp(axleInfo.leftWheel.steerAngle,
                    actualSteering, Time.fixedDeltaTime * turnSpeed);
                axleInfo.rightWheel.steerAngle = Mathf.Lerp(axleInfo.rightWheel.steerAngle,
                    actualSteering, Time.fixedDeltaTime * turnSpeed);
            }
            //Check the motor
            if (axleInfo.motor)
            {
                currentSpeed = 2 * Mathf.PI * axleInfo.leftWheel.radius
                    * axleInfo.leftWheel.rpm * 60 / 1000;

                if (currentSpeed < MaxSpeed)
                {
                    //Applying the motor to the axle
                    axleInfo.leftWheel.motorTorque = MotorTorque;
                    axleInfo.rightWheel.motorTorque = MotorTorque;
                }
                else
                {
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.rightWheel.motorTorque = 0;
                }
            }
        }

        /// <summary>
        /// Calculate steering needed to travel to the <paramref name="directionWayPoint"/>.
        /// <paramref name="directionNextWayPoint"/> is needed to check if there is a dangerous curve
        /// approaching.
        /// </summary>
        /// <param name="directionWayPoint"></param>
        /// <param name="directionNextWayPoint"></param>
        /// <returns></returns>
        float CalculateSteering(Vector3 directionWayPoint, Vector3 directionNextWayPoint)
        {
            float actualSteering = (directionWayPoint.x / directionWayPoint.magnitude) * MaxSteeringAngle;
            if (directionWayPoint.magnitude < curveApproachDistance)
            {
                //nextWayPointDirection is used to detect if there is a dangerous curve approaching
                //its range goes from -1 to 1
                float nextWayPointDirection = (directionNextWayPoint.x / directionNextWayPoint.magnitude);
                if (nextWayPointDirection > 0.5f || nextWayPointDirection < -0.5f)
                {
                    actualSteering = nextWayPointDirection * MaxSteeringAngle;
                    //At this point the transition is at the next waypoint
                    this.GetComponent<BusAgentController>().TransitionDone = true;
                }
            }
            return actualSteering;
        }
        /// <summary>
        /// It applies movement to the bus, according to the <paramref name="directionWayPoint"/>.
        /// <paramref name="directionNextWayPoint"/> is needed to calculate strict curves approaching.
        /// </summary>
        /// <param name="directionWayPoint"></param>
        /// <param name="directionNextWayPoint"></param>
        public void ApplyMovement(Vector3 directionWayPoint, Vector3 directionNextWayPoint)
        {
            float actualSteering = this.CalculateSteering(directionWayPoint, directionNextWayPoint);
            if(axleInfos != null)
                foreach(AxleBusInfo axleInfo in axleInfos)
                {
                    this.UpdateAxle(axleInfo, actualSteering);
                    this.ApplyLocalPositionToVisuals(axleInfo);
                }
        }
    }
}




