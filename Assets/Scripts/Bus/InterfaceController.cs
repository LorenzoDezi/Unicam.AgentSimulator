using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    public class InterfaceController : MonoBehaviour
    {

        List<BusCameraControl> busCameraControls;
        int currentBus;


        void Start()
        {
            currentBus = 0;
            busCameraControls = new List<BusCameraControl>();
            GameObject[] bus = GameObject.FindGameObjectsWithTag("Bus");
            foreach(GameObject singleBus in bus)
            {
                busCameraControls.Add(singleBus.GetComponent<BusCameraControl>());
            }
            if(busCameraControls.Count > 0)
            {
                busCameraControls[currentBus].EnableCamera();
            }

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                busCameraControls[currentBus].DisableCamera();
                currentBus = (currentBus + 1) % busCameraControls.Count;
                busCameraControls[currentBus].EnableCamera();
                

            } else if(Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                busCameraControls[currentBus].DisableCamera();
                currentBus = (currentBus - 1) % busCameraControls.Count;
                busCameraControls[currentBus].EnableCamera();
            }

        }
    }
}


