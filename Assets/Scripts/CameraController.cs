using UnityEngine;

namespace Unicam.AgentSimulator.Scripts
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField]
        private float panSpeed = 20f;

        //panBorderThickness is the limit at which the mouse position will make the camera 
        //updates the position
        [SerializeField]
        private float panBorderThickness = 10f;

        //scrollSpeed is the velocity of the zoom-in/zoom-out camera movement
        [SerializeField]
        private float scrollSpeed = 2f;

        //minY and maxY represents the minimum and maximum zoom capabilities of the camera
        [SerializeField]
        private float minY = 20f;
        [SerializeField]
        private float maxY = 120f;

        //panLimit.x is for the x position of the camera, panLimit.y is for the z position of the camera
        //it limits the possibilities for the user to move the camera around the simulation world.
        [SerializeField]
        private Vector2 panLimit;

        void LateUpdate()
        {

            Vector3 pos = transform.position;

            if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.z += panSpeed * Time.deltaTime;
            }

            if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
            }

            if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }

            if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            {
                pos.z -= panSpeed * Time.deltaTime;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll *= scrollSpeed * 100f * Time.deltaTime;

            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);


            transform.position = pos;
        }
    }
}