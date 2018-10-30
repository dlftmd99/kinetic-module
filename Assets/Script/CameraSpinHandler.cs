using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpinHandler : MonoBehaviour
{
    public GameObject target;
    private Transform tf;
    private float angle;


    private void Awake()
    {
        tf = target.transform;
    }

    void Update ()
    {
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            angle += 2;
            if (angle > 360)
                angle = 0;
            tf.localEulerAngles = new Vector3(0, angle, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            angle -= 2;
            if (angle < 0)
                angle = 360;
            tf.localEulerAngles = new Vector3(0, angle, 0);
        }
    }

    private void OnEnable()
    {
        angle = 0;
        tf.localEulerAngles = new Vector3(0, angle, 0);
    }







}
