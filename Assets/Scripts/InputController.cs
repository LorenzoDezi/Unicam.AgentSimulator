using System;
using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class InputController : MonoBehaviour
    {

        [SerializeField]
        protected UnityEngine.TextAsset[] texts;
        [SerializeField]
        protected GameObject agentPrefab;

        [SerializeField]
        protected char TabulationDelimiter = '\t';

        [SerializeField]
        protected char valueDelimiter = '-';

        [SerializeField]
        protected char[] timeDelimiter = Environment.NewLine.ToCharArray();



        private void Awake()
        {
            foreach (TextAsset text in texts)
            {

                //For each time value, there is a set of properties. The timeDelimiter chosen in the text 
                //separates different time values.
                string[] propertySets = text.text.Split(timeDelimiter,
                    StringSplitOptions.RemoveEmptyEntries);

                if (propertySets.Length != 0)
                {

                    Dictionary<int, AgentState> states = new Dictionary<int, AgentState>(propertySets.Length);
                    Vector3 startPosition = new Vector3();
                    Vector3 startDirection = new Vector3();
                    int index = 0;
                    foreach (string currentTimePropertySet in propertySets)
                    {
                        AgentState state = this.CreateState(currentTimePropertySet);
                        if (index == 0)
                        {
                            startPosition = state.position;
                            startDirection = state.direction;
                        }
                        states.Add(index, state);
                        index++;

                    }
                    GameObject agent = GameObject.Instantiate(agentPrefab, startPosition, Quaternion.identity);
                    //agent.transform.forward = startDirection;
                    this.SetStates(agent, states);
                    this.GetComponent<TimeManager>().AddAgent(agent.GetComponent<AgentController>());

                }
            }
        }

        /// <summary>
        /// If the input position values must be
        /// modified, this is the method where the job get done.
        /// </summary>
        /// <param name="positionValues"> The raw input position values</param>
        /// <returns></returns>
        protected virtual string[] SanitizePositionValues(string[] positionValues)
        {
            return positionValues;
        }

        /// <summary>
        /// This method set an agent states. This can be overriden to perform different behaviours.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="states"></param>
        protected virtual void SetStates(GameObject agent, Dictionary<int, AgentState> states)
        {
            agent.GetComponent<AgentController>().SetStates(states);

        }

        /// <summary>
        /// This method creates a class of type AgentState. Different controller could use different subclasses
        /// of AgentState
        /// </summary>
        /// <param name="currentTimePropertySet"></param>
        /// <returns></returns>
        protected virtual AgentState CreateState(string currentTimePropertySet)
        {
            string[] propertyStrings = currentTimePropertySet.Split(TabulationDelimiter);

            Vector3 position = new Vector3();
            Vector3 direction = new Vector3();

            if (propertyStrings.Length > 1)
            {
                string[] directionValues = propertyStrings[1].Split(valueDelimiter);
                if (directionValues.Length != 3)
                    Debug.Log("Problem parsing direction" + directionValues.Length);
                direction = new Vector3(float.Parse(directionValues[0]), 
                    float.Parse(directionValues[1]), 
                    float.Parse(directionValues[2]));
                direction.Normalize();
            }
            if (propertyStrings.Length > 0)
            {
                string[] positionValues = propertyStrings[0].Split(valueDelimiter);
                if (positionValues.Length != 3)
                    throw new System.FormatException("Problem parsing position" + positionValues.Length);
                positionValues = this.SanitizePositionValues(positionValues);
                position = new Vector3(float.Parse(positionValues[0]), float.Parse(positionValues[1]), float.Parse(positionValues[2]));
            }

            return new AgentState(position, direction);
        }



    }

}


