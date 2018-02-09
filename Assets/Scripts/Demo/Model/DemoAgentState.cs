using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.dll.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Demo.Model {

     /// <summary>
    /// An AgentState class represents a set of properties of the agent.
    /// Override this class to add more properties.
    /// </summary>
    [System.Serializable]
    public class DemoAgentState : AgentState
    {

        public Color color;
        public Vector3 direction;

        public DemoAgentState(Vector3 position) : base(position)
        {

        }

        public DemoAgentState(Vector3 position, Vector3 direction) : base(position)
        {
            this.direction = direction;
        }

    }

}

