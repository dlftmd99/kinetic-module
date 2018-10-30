using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLocationHandler : MonoBehaviour
{
    public delegate void CameraLocationEvent(int number);
    public event CameraLocationEvent cameraLocationEvent;

    public void onCameraLocationButtonEvent(int number)
    {
        if (cameraLocationEvent != null)
            cameraLocationEvent(number);
    }
	
}
