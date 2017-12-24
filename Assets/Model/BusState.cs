using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Model {

    /// <summary>
    /// An AgentState struct represents a set of properties of the agent
    /// </summary>
    public class BusState: AgentState
    {

        //TODO: Varie proprietà che possono essere specificati ai singoli bus
        public Color color;

        public BusState(Vector3 position, Vector3 direction) : base(position, direction){
        }

        public BusState(Vector3 position, Vector3 direction, Color color) : base(position, direction)
        {
            this.color = color;
        }

        

    }

}

