using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unicam.AgentSimulator.dll;
using Unicam.AgentSimulator.dll.Model;
using Unicam.AgentSimulator.Scripts.Demo.Model;
using Unicam.AgentSimulator.Scripts.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Unicam.AgentSimulator.Scripts
{
    /// <summary>
    /// The Input Controller implementation of the Demo Scene
    /// </summary>
    public class DemoInputController : InputController
    {
        [Header("Agent parameters")]
        //If true, text files are specified by paths - UserInputStorage -
        //else they are project assets - textAssets property
        [SerializeField]
        [Tooltip("Are text files specified by paths on a UserInputStorage script?")]
        bool inputFromUser = false;

        [SerializeField]
        [Tooltip("Interface text used to display format error in parsing agents")]
        Text ErrorText;

        protected override void Start()
        {
            GameObject input = GameObject.FindGameObjectWithTag("Storage");
            if (inputFromUser && input != null)
            {
                foreach (string path in input.GetComponent<UserInputStorage>().InputPaths)
                {
                    string content = File.ReadAllText(path);
                    try
                    {
                        CreateAgent(content);
                    }
                    catch (FormatException ex)
                    {
                        ErrorText.text = Path.GetFileName(path) + " - " + ex.Message;
                    }
                }
            }
            else
            {
                base.Start();
            }

        }

        Color GetColorFromInput(string[] colorValues)
        {
            Color color = new Color();
            //colorvalues follow RGBA, red green blue and alpha (transparency)
            if (colorValues.Length != 4)
            {
                throw new FormatException("Problem parsing color at value position: " + colorValues.Length);
            }
            else
            {
                color = new Color(float.Parse(colorValues[0]),
                    float.Parse(colorValues[1]),
                    float.Parse(colorValues[2]));
            }
            return color;
        }

        Vector3 GetDirectionFromInput(string[] directionValues)
        {
            Vector3 direction = new Vector3();
            if (directionValues.Length != 3)
                throw new FormatException("Problem parsing direction at value position: " + directionValues.Length);
            else
            {
                direction = new Vector3(float.Parse(directionValues[0]),
                float.Parse(directionValues[1]),
                float.Parse(directionValues[2]));
                direction.Normalize();
            }
            return direction;
        }

        Vector3 GetPositionFromInput(string[] positionValues)
        {
            Vector3 position = new Vector3();
            if (positionValues.Length != 3)
                throw new FormatException("Problem parsing position at value position: " + positionValues.Length);
            positionValues = this.SanitizePositionValues(positionValues);
            position = new Vector3(float.Parse(positionValues[0]), float.Parse(positionValues[1]), float.Parse(positionValues[2]));
            return position;
        }

        protected override AgentState CreateState(string currentTimePropertySet)
        {
            string[] propertyStrings = currentTimePropertySet.Split(tabulationDelimiter);

            Vector3 position = new Vector3();
            Vector3 direction = new Vector3();
            Color color = new Color();

            if (propertyStrings.Length > 2)
                color = this.GetColorFromInput(propertyStrings[2].Split(valueDelimiter));

            if (propertyStrings.Length > 1)
                direction = this.GetDirectionFromInput(propertyStrings[1].Split(valueDelimiter));

            if (propertyStrings.Length > 0)
                position = this.GetPositionFromInput(propertyStrings[0].Split(valueDelimiter));

            DemoAgentState newState = new DemoAgentState(position, direction, color);
            return newState;
        }



    }

}


