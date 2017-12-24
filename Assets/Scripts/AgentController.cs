using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class AgentController : MonoBehaviour
    {

        [SerializeField]
        public TimeController timeController;

        [SerializeField]
        protected float speed = 1f;
        [SerializeField]
        protected float timeoutTime = 3f;
        [SerializeField]
        protected float distanceTolerance = 0.1f;

        /// <summary>
        /// A set of states linked to a moment in time. The int index represents the exact moment,
        /// the AgentState object the set of properties of the agent in that exact moment.
        /// </summary>
        protected Dictionary<int, AgentState> states = new Dictionary<int, AgentState>();

        //This variable enables us to detect if there is a change of time or the simulation is paused.
        public bool transitionDone = true;
        protected float transitionTime;

        /// <summary>
        /// The agent rigidbody, where physics is applied.
        /// </summary>
        protected Rigidbody rigidBody;

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
                this.transform.forward = currentAgentState.direction;
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

        protected void Start()
        {
            rigidBody = this.GetComponent<Rigidbody>();
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

        protected void FixedUpdate()
        {
            if (states.TryGetValue(timeController.time, out currentAgentState) && !transitionDone)
            {
                //With this line of code, we don't use physics - it works if the collider is set to isTrigger
                //this.transform.position = currentAgentState.position;
                rigidBody.velocity = (currentAgentState.position - transform.position).normalized * speed;
                this.transform.forward = currentAgentState.direction;
                transitionTime -= Time.fixedDeltaTime;

            }

            if (Vector3.Distance(currentAgentState.position, transform.position) <= distanceTolerance)
            {
                rigidBody.velocity = Vector3.zero;
                transitionDone = true;
                transitionTime = timeoutTime;
            }
            else if (transitionTime <= 0f)
            {
                Debug.Log("Agent serial: " + this.gameObject.GetInstanceID() + " has failed updating its properties. Simulation broken.");
                rigidBody.velocity = Vector3.zero;
                transitionDone = true;
                transitionTime = timeoutTime;
            }

            rigidBody.isKinematic = transitionDone;
        }

    }

}


