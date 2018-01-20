using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class AgentController : MonoBehaviour
    {



        [Header("Agent Time Properties")]
        [SerializeField]
        public TimeController timeController;
        [SerializeField]
        protected float speed = 1f;
        [SerializeField]
        protected float timeoutTime = 3f;
        [SerializeField]
        protected float distanceTolerance = 0.1f;
        protected float transitionTime;
        public bool transitionDone = false;


        [Header("Agent States Properties")]
        /// <summary>
        /// A set of states linked to a moment in time. The int index represents the exact moment,
        /// the AgentState object the set of properties of the agent in that exact moment.
        /// </summary>
        protected Dictionary<int, AgentState> states = new Dictionary<int, AgentState>();
        /// <summary>
        /// The current set of properties of the agent
        /// </summary>
        protected AgentState currentAgentState;








        /// <summary>
        /// Set the map from time to agentState, each state is mapped by an integer that represents its moment
        /// in time. 
        /// </summary>
        /// <param name="states"> the input set of states, retrieved by the InputController </param>
        public void SetStates(Dictionary<int, AgentState> states)
        {
            this.states = states;
            if (states.TryGetValue(0, out currentAgentState))
            {
                this.transform.position = currentAgentState.position;
                //this.transform.forward = currentAgentState.direction;
                if(states.TryGetValue(1, out currentAgentState))
                {
                    transitionDone = true;
                }
            }
        }

        /// <summary>
        /// Returns the number of states from the input text file
        /// </summary>
        /// <returns> The number of states </returns>
        public int GetNumberOfStates()
        {
            return states.Keys.Count;
        }

        protected virtual void Start()
        {
            timeController = this.GetComponent<TimeController>();
            transitionTime = timeoutTime;
        }

        /// <summary>
        /// This method updates each single property, different for each subclass
        /// </summary>
        protected virtual void UpdateProperties()
        {
            return;
        }

        /// <summary>
        /// This method update the direction of the agent.
        /// </summary>
        protected virtual void UpdateRotation()
        {
            if (currentAgentState.direction != Vector3.zero && !transitionDone)
            {
                transform.forward = currentAgentState.direction;
            }
            else if (!transitionDone)
            {
                transform.forward = (currentAgentState.position - transform.position).normalized;
            }
        }

        /// <summary>
        /// This method updates the position of the agent.
        /// </summary>
        protected virtual void UpdatePosition()
        {
            //With this line of code, we don't use physics - it works if the collider is set to isTrigger
            //this.transform.position = currentAgentState.position;
            if (!transitionDone)
                this.GetComponent<Rigidbody>().velocity = (currentAgentState.position - transform.position).normalized * speed;
            else
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().isKinematic = transitionDone;
        }



        protected virtual void FixedUpdate()
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


