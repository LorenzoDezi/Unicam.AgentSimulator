using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.dll;
using Unicam.AgentSimulator.dll.Model;
using Unicam.AgentSimulator.Scripts.Demo.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class DemoAgentController : AgentController
    {
        [SerializeField]
        protected float transitionTime;
        [SerializeField]
        protected float timeoutTime = 3f;

        protected override void Start()
        {
            transitionTime = timeoutTime;
            base.Start();
        }

        protected override void UpdateProperties()
        {
            return;
        }

        
        void UpdateRotation()
        {
            DemoAgentState currentAgentState = (DemoAgentState) this.currentAgentState; 
            if (currentAgentState.direction != Vector3.zero && !transitionDone)
            {
                transform.forward = currentAgentState.direction;
            }
            else if (!transitionDone)
            {
                transform.forward = (currentAgentState.position - transform.position).normalized;
            }
        }

        
        protected override void UpdatePosition()
        {
            if (!transitionDone)
                this.GetComponent<Rigidbody>().velocity = (currentAgentState.position - transform.position).normalized * speed;
            else
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().isKinematic = transitionDone;
        }



        protected override void FixedUpdate()
        {
            if (states.TryGetValue(timeController.time, out currentAgentState) && !transitionDone)
            {

                transitionTime -= Time.fixedDeltaTime;
            }

            if (Vector3.Distance(currentAgentState.position, transform.position) <= distanceTolerance)
            {
                transitionDone = true;
                transitionTime = timeoutTime;
            }
            else if (transitionTime <= 0f)
            {
                Debug.Log("Agent serial: " + this.gameObject.GetInstanceID() + " has failed updating its properties. Simulation broken.");
                transitionDone = true;
                transitionTime = timeoutTime;
            }
            UpdateRotation();
            UpdatePosition();

        }

    }

}


