using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class BusController : AgentController
    {
        Animator animator;

        protected override void Start()
        {
            base.Start();
            animator = this.GetComponent<Animator>();
        }

        protected override void FixedUpdate()
        {
            //Set the bus animation for the wheel motion
            animator.SetFloat("speed", this.speed);
            //The speed of the animation is directly linked to the speed of the bus
            animator.speed = this.speed / 2;
            base.FixedUpdate();
        }

    }

}


