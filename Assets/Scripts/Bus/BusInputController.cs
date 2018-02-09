using System;
using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.dll.Model;
using Unicam.AgentSimulator.dll;
using Unicam.AgentSimulator.Utility;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    public class BusInputController : InputController
    {
        [Header("Bus Agent Parameters")]
        /// <summary>
        /// This value indicates how many waypoint for each state.
        /// This is needed to give the bus IA a better trajectory.
        /// </summary>
        [SerializeField]
        int wayPointsForState = 10;

        protected override void SetStates(GameObject agent, Dictionary<int, AgentState> states)
        {
            //We add more waypoints between each state position.
            //newStates will represent my new set of states. J in the index used to cycle it.
            Dictionary<int, AgentState> newStates = new Dictionary<int, AgentState>();
            int j = 0;
            
            //i will be used to cycle input states.
            for(int i=0; i < states.Keys.Count; i++)
            {
                AgentState previousState;
                AgentState currentState;
                if(states.TryGetValue(i, out previousState) && states.TryGetValue(i+1, out currentState))
                {
                    //The directionVector will be divided by the number of waypoints.
                    Vector3 directionVector = currentState.position - previousState.position;
                    Vector3 currentPosition = previousState.position;
                    if(i == 0)
                        newStates.Add(j++, previousState);
                    int currentStateIndex = j + wayPointsForState;
                    for(; j < currentStateIndex; j++)
                    {

                        currentPosition += directionVector / wayPointsForState;
                        newStates.Add(j, new AgentState(currentPosition));
                        
                    }

                } else
                {
                    //Number of states are odd. According to the for loop, the previousState
                    //will always give a result as input of the TryGetValue method. 
                    newStates.Add(j, previousState);
                }
            }
            base.SetStates(agent, newStates);
        }


        protected override string[] SanitizePositionValues(string[] positionValues)
        {
            //We parse Longitude and latitued coordinates to UTM values
            return UTMUtility.ParseLongLatToUTM(positionValues, StreetMaker.EdinburghUTMOrigin);
        }
       
    }

}
