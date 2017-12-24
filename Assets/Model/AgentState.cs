using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Model {

        /// <summary>
    /// An AgentState class represents a set of properties of the agent
    /// </summary>
    public class AgentState
    {

        public Vector3 position;
        public Vector3 direction;

        public AgentState(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
        }

    }

}

