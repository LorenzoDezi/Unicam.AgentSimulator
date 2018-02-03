using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unicam.AgentSimulator.Scripts
{
    public class TimeManager : MonoBehaviour
    {

        List<AgentController> agents = new List<AgentController>();

        public void AddAgent(AgentController a)
        {
            agents.Add(a);
        }

        /// <summary>
        /// The time starts
        /// </summary>
        public void Play()
        {
            Time.timeScale = 1f;
            foreach (AgentController agent in agents)
            {
                agent.GetComponent<TimeController>().isReversing = false;
            }

        }

        /// <summary>
        /// The time stops
        /// </summary>
        public void Stop()
        {
            Time.timeScale = 0f;
        }

        /// <summary>
        /// The time goes backwards
        /// </summary>
        public void Backward()
        {
            Time.timeScale = 1f;
            foreach (AgentController agent in agents)
            {
                agent.GetComponent<TimeController>().isReversing = true;
            }

        }

        /// <summary>
        /// Restart the simulation
        /// </summary>
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Escape from the simulation, back to the main menu
        /// </summary>
        public void Close()
        {
            SceneManager.LoadScene(0);
        }

    }

}

