using System;
using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.dll;
using Unicam.AgentSimulator.dll.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    /// <summary>
    /// The AgentController implementation made for the Edinburgo bus scene
    /// It needs a BusDrive Script and an AudioSource attached to the same object.
    /// </summary>
    public class BusAgentController : AgentController
    {

        AudioSource audioSource;
        BusDrive busDrive;
        AgentState nextAgentState;

        protected override void Start()
        {
            base.Start();
            audioSource = this.GetComponent<AudioSource>();
            transform.LookAt(currentAgentState.position);
            busDrive = this.GetComponent<BusDrive>();
        }
       
        protected override void UpdatePosition()
        {
            busDrive.ApplyMovement(transform.InverseTransformPoint(currentAgentState.position), 
                transform.InverseTransformPoint(nextAgentState.position));
        }

        protected override void FixedUpdate()
        {
            //We recover the next agent state, to predict a strict curve
            states.TryGetValue(TimeController.Time + 1, out nextAgentState);
            //Play the sound effect
            if (busDrive.currentSpeed != 0 && !audioSource.isPlaying)
                audioSource.Play();
            else if(busDrive.currentSpeed == 0)
                audioSource.Stop();
            base.FixedUpdate();
        }

    }

}


