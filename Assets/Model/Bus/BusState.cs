using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Model.Bus {

    /// <summary>
    /// An AgentState struct represents a set of properties of the agent
    /// </summary>
    public class BusState: AgentState
    {

        public BusState(Vector3 position, Vector3 direction) : base(position, direction){
        }

    }

}

