using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unicam.AgentSimulator.dll;
using Unicam.AgentSimulator.dll.Model;
using Unicam.AgentSimulator.Scripts.Demo.Model;
using Unicam.AgentSimulator.Scripts.Menu;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class DemoInputController : InputController
    {
        [Header("Agent parameters")]
        //If true, text files are specified by paths - userInputObject
        //else they are project assets - textAssets property
        [SerializeField]
        bool inputFromUser = false;
       

        protected override void Start()
        {
            GameObject input = GameObject.FindGameObjectWithTag("Storage");
            if(inputFromUser && input != null)
            {
                foreach(string path in input.GetComponent<UserInputStorage>().inputPaths)
                {
                    string content = File.ReadAllText(path);
                    
                    CreateAgent(content);
                }

            } else
            {
                base.Start();
            }
            
        }

        
        protected override AgentState CreateState(string currentTimePropertySet)
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

            DemoAgentState newState = new DemoAgentState(position, direction);
            return newState;
        }



    }

}


