using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    private List<GameObject> popupList = new List<GameObject>();
    public GameObject popup_loading;
    public GameObject popup_err;
    public GameObject popup_obj;
    public GameObject popup_video;
    public GameObject popup_camera;

    private void Start()
    {
        popupList.Add(popup_loading);
        popupList.Add(popup_err);
        popupList.Add(popup_obj);
        popupList.Add(popup_video);
        popupList.Add(popup_camera);
        //activeOffAllPopup();
    }

    public void activePopup(POPUP_TYPE type)
    {
        switch (type)
        {
            case POPUP_TYPE.loading:
                popup_loading.SetActive(true);
                break;
            case POPUP_TYPE.err:
                popup_err.SetActive(true);
                break;
            case POPUP_TYPE.obj:
                popup_obj.SetActive(true);
                break;
            case POPUP_TYPE.video:
                popup_video.SetActive(true);
                break;
            case POPUP_TYPE.camera:
                popup_camera.SetActive(true);
                break;
        }
    }

    public void activeoffPopup(POPUP_TYPE type)
    {
        switch (type)
        {
            case POPUP_TYPE.loading:
                popup_loading.SetActive(false);
                break;
            case POPUP_TYPE.err:
                popup_err.SetActive(false);
                break;
            case POPUP_TYPE.obj:
                popup_obj.SetActive(false);
                break;
            case POPUP_TYPE.video:
                popup_video.SetActive(false);
                break;
            case POPUP_TYPE.camera:
                popup_camera.SetActive(false);
                break;
        }
    }

    public void activeOffAllPopup()
    {
        for (int a = 0; a < popupList.Count; a++)
            popupList[a].SetActive(false);
    }

    public GameObject getPopup(POPUP_TYPE type)
    {
        if (type == POPUP_TYPE.loading)
            return popup_loading;
        else if (type == POPUP_TYPE.err)
            return popup_err;
        else if (type == POPUP_TYPE.obj)
            return popup_obj;
        else if (type == POPUP_TYPE.video)
            return popup_video;
        else if (type == POPUP_TYPE.camera)
            return popup_camera;
        return null;
    }

}


