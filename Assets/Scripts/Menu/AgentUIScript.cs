using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unicam.AgentSimulator.Scripts.Menu
{
    public class AgentUIScript : MonoBehaviour
    {
        [SerializeField]
        Button RemoveButton;

        [SerializeField]
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

        public void SetAgentText(string text)
        {
            this.AgentText.text = text;
        }

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


