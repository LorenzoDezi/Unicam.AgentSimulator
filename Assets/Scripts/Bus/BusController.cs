using System;
using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class BusController : AgentController
    {
        Animator animator;
        AudioSource audioSource;

        BusDrive busDrive;
        AgentState nextAgentState;

        protected override void Start()
        {
            base.Start();
            animator = this.GetComponent<Animator>();
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
            //Set the bus animation for the wheel motion
            animator.SetFloat("speed", this.speed);
            //The speed of the animation is directly linked to the speed of the bus
            animator.speed = this.speed / 2;
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


