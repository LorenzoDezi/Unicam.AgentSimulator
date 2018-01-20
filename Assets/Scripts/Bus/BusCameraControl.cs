using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    public class BusCameraControl : MonoBehaviour
    {
        AudioSource cameraSwitchAudioSource;

        GameObject[] busCameraObjects = new GameObject[3];
        int currentCameraIndex;
        bool isBusMainView;

        private void Start()
        {
            //We retrieve every camera object the bus have in its children objects.
            Camera[] cameras = this.GetComponentsInChildren<Camera>();
            for (int i = 0; i < 3; i++)
            {
                cameras[i].gameObject.SetActive(false);
                busCameraObjects[i] = cameras[i].gameObject;
            }
            //Is this bus selected for main view?
            isBusMainView = false;
            cameraSwitchAudioSource = this.GetComponents<AudioSource>()[1];
            //The index of the camera selected
            currentCameraIndex = 0;
        }

        /// <summary>
        /// Enable this bus to be the main view of the scene
        /// </summary>
        public void EnableCamera()
        {
            busCameraObjects[currentCameraIndex].SetActive(true);
            isBusMainView = true;
        }

        /// <summary>
        /// This bus is not the main view of the scene anymore.
        /// </summary>
        public void DisableCamera()
        {
            busCameraObjects[currentCameraIndex].SetActive(false);
            isBusMainView = false;
        }

        public void Update()
        {
            if(isBusMainView)
            {
                if(Input.GetButtonDown("FirstPersonView"))
                {
                    cameraSwitchAudioSource.Play();
                    this.SetActiveCamera(0);

                } else if(Input.GetButtonDown("ThirdPersonView"))
                {
                    cameraSwitchAudioSource.Play();
                    this.SetActiveCamera(1);

                } else if(Input.GetButtonDown("PerspectiveView"))
                {
                    cameraSwitchAudioSource.Play();
                    this.SetActiveCamera(2);
                }
            }
        }

        /// <summary>
        /// Set the camera at the index specified as the active one.
        /// </summary>
        /// <param name="index"></param>
        public void SetActiveCamera(int index)
        {
            busCameraObjects[currentCameraIndex].SetActive(false);
            busCameraObjects[index].SetActive(true);
            currentCameraIndex = index;
        }
    }
}


