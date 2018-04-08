using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus.Model
{
    /// <summary>
    /// It represents the state of the bus stop.
    /// </summary>
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
            } else if (this.Name.Contains("Drum"))
            {
                this.transform.position = new Vector3(-1128.68f,
                    this.transform.position.y, this.transform.position.z);
                this.transform.rotation = Quaternion.AngleAxis(96.6f, Vector3.up);
            } else if (this.Name.Contains("Corner"))
            {
                this.transform.rotation = Quaternion.AngleAxis(94.8f, Vector3.up);
            } else if (this.Name.Contains("Shandwick"))
            {
                this.transform.rotation = Quaternion.AngleAxis(-65.68f, Vector3.up);
            } else if (this.Name.Contains("Coates"))
            {
                this.transform.position = new Vector3(-840.42f,
                    this.transform.position.y, this.transform.position.z);
                this.transform.rotation = Quaternion.AngleAxis(93f, Vector3.up);
            } 
        }

    }

}


