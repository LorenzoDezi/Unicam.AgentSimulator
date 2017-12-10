using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public int time;

    bool isForward = false;
    bool isBackward = false;
    bool allTransitionDone = false;

    List<AgentController> agents = new List<AgentController>();

    private void FixedUpdate()
    {

        foreach(AgentController agent in agents)
        {
            allTransitionDone = agent.transitionDone;
            if (!allTransitionDone)
                break;
        }

        if (isForward)
        {
            OneStepForward();                
        }

        else if (isBackward)
        {
            OneStepBack();   
        }    
    }

    public void AddAgent(AgentController agent)
    {
        this.agents.Add(agent);
    }

    /// <summary>
    /// The time starts
    /// </summary>
    public void Play()
    {
        this.isForward = true;
        this.isBackward = false;
        
    }

    /// <summary>
    /// The time stops
    /// </summary>
    public void Stop()
    {
        this.isForward = false;
        this.isBackward = false;
    }

    /// <summary>
    /// The time goes backwards
    /// </summary>
    public void Backward()
    {
        this.isForward = false;
        this.isBackward = true;
        
    }

    /// <summary>
    /// The time goes one step forward
    /// </summary>
    public void OneStepForward()
    {
        if(time < int.MaxValue && allTransitionDone)
        {
            time++;
            foreach (AgentController agent in agents)
            {
                agent.transitionDone = false;
            }
        }
        
    }

    /// <summary>
    /// The time goes one step back
    /// </summary>
    public void OneStepBack()
    {
        if(time > 0 && allTransitionDone)
        {
            time--;
            foreach (AgentController agent in agents)
            {
                agent.transitionDone = false;
            }
        }
        
    }

}
