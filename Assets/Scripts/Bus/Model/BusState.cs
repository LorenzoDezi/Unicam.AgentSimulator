using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus.Model {

    /// <summary>
    /// An AgentState struct represents a set of properties of the agent
    /// </summary>
    public class BusState: AgentState
    {

        public BusState(Vector3 position, Vector3 direction) : base(position, direction){
        }

    }

}

