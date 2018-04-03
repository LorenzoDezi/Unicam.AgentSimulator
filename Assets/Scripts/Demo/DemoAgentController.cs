using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.dll;
using Unicam.AgentSimulator.dll.Model;
using Unicam.AgentSimulator.Scripts.Demo.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Unicam.AgentSimulator.Scripts
{
    /// <summary>
    /// The AgentController implementation for the Demo Scene.
    /// </summary>
    public class DemoAgentController : AgentController
    {
        [SerializeField]
        [Tooltip("Current time taken for a transition to happen")]
        protected float transitionTime;
        [SerializeField]
        [Tooltip("Maximum time needed for an agent to fails a transition")]
        protected float timeoutTime = 3f;

        [SerializeField]
        [Tooltip("Text used to display last property update fail")]
        Text ErrorText;

        protected override void Start()
        {
            transitionTime = timeoutTime;
            if(ErrorText == null)
            {
                GameObject errorTextGameObject = GameObject.FindGameObjectWithTag("ErrorText");
                if(errorTextGameObject != null)
                    ErrorText = errorTextGameObject.GetComponent<Text>();
            }
            base.Start();
        }

        protected override void UpdateProperties()
        {
            DemoAgentState currentAgentState = (DemoAgentState) this.currentAgentState;
            //Updating direction
            if (currentAgentState.direction != Vector3.zero && !TransitionDone)
            {
                transform.forward = currentAgentState.direction;
            }
            else if (!TransitionDone)
            {
                transform.forward = (currentAgentState.position - transform.position).normalized;
            }
            //Updating color
            this.GetComponent<MeshRenderer>().material.color = currentAgentState.color;
            Renderer[] renderInChildren = this.GetComponentsInChildren<MeshRenderer>();
            foreach(Renderer renderer in renderInChildren)
            {
                renderer.material.color = currentAgentState.color;
            }
            return;
        }

        
        protected override void UpdatePosition()
        {
            if (!TransitionDone)
                this.GetComponent<Rigidbody>().velocity = (currentAgentState.position - transform.position).normalized * speed;
            else
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().isKinematic = TransitionDone;
        }
        
        protected override void FixedUpdate()
        {
            if (states.TryGetValue(TimeController.Time, out currentAgentState) && !TransitionDone)
            {

                transitionTime -= Time.fixedDeltaTime;
            }

            if (Vector3.Distance(currentAgentState.position, transform.position) <= distanceTolerance)
            {
                TransitionDone = true;
                transitionTime = timeoutTime;
            }
            else if (transitionTime <= 0f)
            {
                ErrorText.text = "Agent serial: " 
                    + this.gameObject.GetInstanceID() 
                    + " has failed updating its properties." +
                    " Simulation broken.";
                TransitionDone = true;
                transitionTime = timeoutTime;
            }
            UpdatePosition();
            UpdateProperties();
        }

    }

}


