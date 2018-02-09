using System;
using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.dll;
using Unicam.AgentSimulator.dll.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    public class BusController : AgentController
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

        protected override void UpdateRotation()
        {
            //The rotation is applied in the BusDrive script.
        }

        protected override void UpdatePosition()
        {
                busDrive.ApplyMovement(transform.InverseTransformPoint(currentAgentState.position), 
                transform.InverseTransformPoint(nextAgentState.position));
        }

        protected override void FixedUpdate()
        {
            //We recover the next agent state, to predict a strict curve
            states.TryGetValue(timeController.time + 1, out nextAgentState);
            //Play the sound effect
            if (speed != 0 && !audioSource.isPlaying)
            {
                audioSource.Play();
            } else if(speed == 0)
            {
                audioSource.Stop();
            }
            base.FixedUpdate();

        }

    }

}


