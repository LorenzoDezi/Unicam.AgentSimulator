using UnityEngine;

namespace Unicam.AgentSimulator.Scripts.Demo
{
    /// <summary>
    /// The camera controller controls camera movement.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Parameters")]
        [SerializeField]
        [Tooltip("The mouse movement speed")]
        private float panSpeed = 20f;

        [SerializeField]
        [Tooltip("The limit position at which the mouse" +
            " will make the camera update the camera position")]
        private float panBorderThickness = 10f;

        [SerializeField]
        [Tooltip("Limit to the x/z of camera, setting a limit" +
            " to avoid the user moving the camera away from the simulation")]
        private Vector2 panLimit;

        [SerializeField]
        [Tooltip("The velocity of the zoom in-out of the camera")]
        private float scrollSpeed = 2f;

        [SerializeField]
        [Tooltip("Minimum zoom capability of the camera")]
        private float minY = 20f;
        [SerializeField]
        [Tooltip("Maximum zoom capability of the camera")]
        private float maxY = 120f;

        /// <summary>
        /// Returns the position <paramref name="pos"/> updated with the 
        /// int the last frame
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        Vector3 CameraControl(Vector3 pos)
        {
            //Checking for movement input 
            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
                pos.z += panSpeed * Time.deltaTime;
            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
                pos.x += panSpeed * Time.deltaTime;

            if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
                pos.x -= panSpeed * Time.deltaTime;

            if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
                pos.z -= panSpeed * Time.deltaTime;
            //Checking for zoom input
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll *= scrollSpeed * 100f * Time.deltaTime;
            //Clamping the position at limits
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            return pos;
        }

        void LateUpdate()
        {
            transform.position = CameraControl(transform.position);
        }
    }
}