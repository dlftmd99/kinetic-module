using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovingHandler : MonoBehaviour
{
    public Camera camera;
    float speed = 3f;


	void Update ()
    {
        //뷰 회전
        if (Input.GetAxis("Mouse Y") > 0)
            camera.transform.localEulerAngles += new Vector3(Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed * -10, 0.0f, 0);
        else if (Input.GetAxis("Mouse Y") < 0)
            camera.transform.localEulerAngles += new Vector3(Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed * -10, 0.0f, 0);
        if (Input.GetAxis("Mouse X") > 0)
            transform.transform.localEulerAngles += new Vector3(0, Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed * 10, 0);
        else if (Input.GetAxis("Mouse X") < 0)
            transform.transform.localEulerAngles += new Vector3(0, Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed * 10, 0);
        
        //이동
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector3.forward * -speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left * -speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector3.right * -speed * Time.deltaTime);
    }





}

