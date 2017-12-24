using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts {

    public class TimeController : MonoBehaviour
    {

        public int time;

        public bool isReversing = false;

        AgentController agent;

        private void Start()
        {
            agent = this.GetComponent<AgentController>();
        }

        private void FixedUpdate()
        {

            if (agent.transitionDone)
            {
                if (isReversing && time > 0)
                {
                    time--;
                    agent.transitionDone = false;
                }
                else if (!isReversing && time < agent.GetNumberOfStates() - 1)
                {
                    time++;
                    agent.transitionDone = false;
                }

            }
        }

    }

}


