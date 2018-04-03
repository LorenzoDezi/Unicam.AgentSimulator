using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unicam.AgentSimulator.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unicam.AgentSimulator.Scripts.Bus;

namespace Unicam.AgentSimulator.Scripts.Menu
{
    public class MenuManager : MonoBehaviour
    {

        [SerializeField]
        GameObject StartMenuContainer;

        [SerializeField]
        GameObject DemoMenuContainer;

        [SerializeField]
        GameObject AgentUIPrefab;
        [SerializeField]
        GameObject defaultAgentUI;

        [SerializeField]
        UserInputStorage storage;


        /// <summary>
        /// Switch to the demo menu
        /// </summary>
        public void SwitchToDemoMenu()
        {
            this.StartMenuContainer.SetActive(false);
            this.DemoMenuContainer.SetActive(true);
        }

        public void Start()
        {
            if(storage.InputPaths.Count > 0)
            {
                foreach(string path in storage.InputPaths)
                {
                    UpdateAgentUIAddPath(path);
                }
            }
        }

        /// <summary>
        /// Remove an agent from the selection
        /// </summary>
        /// <param name="agentName"> The name of the agent, aka the filename </param>
        /// <returns>true if the removal is successful</returns>
        public bool RemoveAgent(string agentName)
        {
            string pathToRemove = null;
            foreach(string path in storage.InputPaths)
            {
                if(Path.GetFileName(path) == agentName)
                {
                    pathToRemove = path;
                    break;
                }
            }
            if(pathToRemove != null)
            {
                storage.InputPaths.Remove(pathToRemove);
                if(storage.InputPaths.Count == 0)
                {
                    //"No agents selected" displayed
                    defaultAgentUI.SetActive(true);
                }
                return true;
            } else
            {
                return false;
            }
           
        }

        /// <summary>
        /// Add an agent to the selection.
        /// </summary>
        public void AddAgent()
        {
            string path = SFB.StandaloneFileBrowser.OpenFilePanel
                ("Select a txt file", "", "txt", false)[0];
            if(path.Length != 0)
            {
                storage.InputPaths.Add(
                path);
                UpdateAgentUIAddPath(path);
            }

        }

        /// <summary>
        /// Add an agent to the ui from its file path
        /// </summary>
        /// <param name="path">The path of the agent textfile</param>
        private void UpdateAgentUIAddPath(string path)
        {
            string fileName = Path.GetFileName(path);
            GameObject agentUI = Object.Instantiate(AgentUIPrefab, DemoMenuContainer.GetComponentInChildren<VerticalLayoutGroup>().transform);
            agentUI.GetComponent<AgentUIScript>().SetAgentText(fileName);
            //"No agents selected" not displayed anymore
            defaultAgentUI.SetActive(false);
        }

        /// <summary>
        /// Start bus simulation
        /// </summary>
        public void StartBusSimulation()
        {
            SceneManager.LoadScene(2);
        }

        /// <summary>
        /// Start Demo Simulation
        /// </summary>
        public void StartDemoSimulation()
        {
            SceneManager.LoadScene(1);
        }
    }
}


