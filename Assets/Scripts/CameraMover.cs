using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public void MoveCam(float val)
    {
        Camera.main.transform.position = new Vector3(0, -390 * val, -10);//uses the scroll bar at the side of the card viewer scene to move the camera
    }
}
