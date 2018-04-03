using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Bus
{
    /// <summary>
    /// Camera control in the Edinburgo Bus scene
    /// </summary>
    public class BusCameraControl : MonoBehaviour
    {
        AudioSource cameraSwitchAudioSource;
        GameObject[] busCameraObjects;
        int currentCameraIndex;

        private void Awake()
        {
            //Retrieving every camera object the bus has in its children objects.
            Camera[] cameras = this.GetComponentsInChildren<Camera>();
            busCameraObjects = new GameObject[cameras.Length];
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].gameObject.SetActive(false);
                busCameraObjects[i] = cameras[i].gameObject;
            }
            cameraSwitchAudioSource = this.GetComponents<AudioSource>()[1];
            //The index of the camera selected
            currentCameraIndex = 0;
        }

        public void Update()
        {
            if (Input.GetButtonDown("FirstPersonView"))
            {
                cameraSwitchAudioSource.Play();
                this.SetActiveCamera(0);

            }
            else if (Input.GetButtonDown("ThirdPersonView"))
            {
                cameraSwitchAudioSource.Play();
                this.SetActiveCamera(1);

            }
            else if (Input.GetButtonDown("PerspectiveView"))
            {
                cameraSwitchAudioSource.Play();
                this.SetActiveCamera(2);
            }
        }

        /// <summary>
        /// Set the camera at the index specified as the active one.
        /// </summary>
        /// <param name="index"></param>
        public void SetActiveCamera(int index)
        {
            if (index < 0 || index >= busCameraObjects.Length)
                return;
            busCameraObjects[currentCameraIndex].SetActive(false);
            busCameraObjects[index].SetActive(true);
            currentCameraIndex = index;

        }
    }
}


