using System.Collections;
using System.Collections.Generic;
using Unicam.AgentSimulator.Model;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class StopState : MonoBehaviour
    {
        //The name of the bus stop
        public string Name;

        private void Start()
        {
            //Some position and rotation adjustements need to be hard-coded, to improve the scene look.
            if(this.Name.Contains("Airport"))
            {
                this.transform.position = new Vector3(-1247.04f, 0.03f, -21462.72f);
                this.transform.rotation = Quaternion.AngleAxis(390.549f, Vector3.up);
            } else if (this.Name.Contains("Waverley"))
            {
                this.transform.position = new Vector3(-150.61f, 0.03f, -2698f);
                this.transform.rotation = Quaternion.AngleAxis(-150.493f, Vector3.up);
            } else if (this.Name.Contains("Drum") || this.Name.Contains("Place"))
            {
                this.transform.rotation = Quaternion.AngleAxis(92f, Vector3.up);
            } 
        }

    }

}


