using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPopupHandler : MonoBehaviour
{
    //Class
    public ObjLocationHandler objLocationHandler;
    //Other
    public GameObject[] cameraLocation = new GameObject[7];
    //public GameObject obj_camera;
    public GameObject character;
    public GameObject spingObj;

    public Vector3 saveCharacter_pos;
    public Vector3 saveCharacter_angle;
    public Vector3 saveCam_pos;
    public Vector3 saveCam_angle;

    void Start ()
    {
        settingDelegate();
        saveCharacterDate();
    }
	

    //**Event
    public void deleteButtonEvent()
    {
        this.gameObject.SetActive(false);
    }


    //**Callback
    private void cameraLocationEvent_callback(int number)
    {
        if(cameraLocation[number] != null)
        {
            for (int a = 0; a < cameraLocation.Length; a++)
                cameraLocation[a].SetActive(false);
            character.SetActive(false);
            spingObj.SetActive(false);

            if (number == 6)
            {
                character.transform.localPosition = saveCharacter_pos;
                character.transform.localEulerAngles = saveCharacter_angle;
                cameraLocation[6].transform.localPosition = saveCam_pos;
                cameraLocation[6].transform.localEulerAngles = saveCam_angle;
                cameraLocation[6].SetActive(true);
                character.SetActive(true);
            }
            else if(number == 5)
            {
                cameraLocation[5].SetActive(true);
                spingObj.SetActive(true);
            }
            else
            {
                cameraLocation[number].SetActive(true);
            }
        }
    }


    //**Setting
    private void settingDelegate()
    {
        objLocationHandler.cameraLocationEvent += cameraLocationEvent_callback;
    }

    private void saveCharacterDate()
    {
        saveCharacter_pos = character.transform.localPosition;
        saveCharacter_angle = character.transform.localEulerAngles;
        saveCam_pos = cameraLocation[6].transform.localPosition;
        saveCam_angle = cameraLocation[6].transform.localEulerAngles;
    }
	
}
