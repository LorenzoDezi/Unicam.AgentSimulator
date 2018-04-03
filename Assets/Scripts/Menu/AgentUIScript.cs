using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unicam.AgentSimulator.Scripts.Menu
{
    /// <summary>
    ///  Script attached to the single agent element in the demo menu interface
    /// </summary>
    public class AgentUIScript : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The button in charge to remove the selected agent")]
        Button RemoveButton;

        [SerializeField]
        [Tooltip("The filename of the agent")]
        Text AgentText;

        MenuManager menuManager;
        private string defaultText = "No Agent Selected!";

        void Start()
        {
            if (AgentText.text == defaultText)
            {
                RemoveButton.gameObject.SetActive(false);
            }
            menuManager = GameObject.FindGameObjectWithTag("MainMenu").
                GetComponent<MenuManager>();
        }

        /// <summary>
        /// Sets the agent text
        /// </summary>
        /// <param name="text"></param>
        public void SetAgentText(string text)
        {
            this.AgentText.text = text;
        }

        /// <summary>
        /// Remove the agent from the user interface
        /// </summary>
        public void RemoveAgent()
        {
            string nameAgentToRemove = AgentText.text;
            if(menuManager.RemoveAgent(nameAgentToRemove))
            {
                Destroy(this.gameObject);
            } else
            {
                Debug.LogError("Fatal Error - Name not corresponding: " + nameAgentToRemove);
                Application.Quit();
            }
        }

        


    }
}


