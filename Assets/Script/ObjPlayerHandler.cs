using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjPlayerHandler : MonoBehaviour
{
    //Delegate
    public delegate void PlayEvent(string dataUid);
    public event PlayEvent playEvent;
    public delegate void PauseEvent(string dataUid);
    public event PauseEvent pauseEvent;
    public delegate void StopEvent(string dataUid);
    public event StopEvent stopEvent;
    public delegate void SliderEvent(string dataUid, int value);
    public event SliderEvent sliderEvent;
    public delegate void WindowBrewserEvent(string url);
    public event WindowBrewserEvent windowBrewserEvent;
    //Other
    public string selectDataUid = string.Empty;
    public Slider slider;


    public void removeItem_Callback(string dataUid)
    {
        if (selectDataUid == dataUid)
            selectDataUid = string.Empty;
    }

    //**Set
    public void setSelectDataUid(string dataUid)
    {
        selectDataUid = dataUid;
    }

    public void setSliderMaxValue(string dataUid)
    {
        slider.maxValue = DB_Data.Instance.getData_sceneDataByUid(dataUid).dataNumberofFrame - 1;
    }

    public void setSliderValue(int value)
    {
        slider.value = value;
    }

    //**Event
    public void onPlayButtonEvent()
    {
        if (playEvent != null && selectDataUid != string.Empty)
            playEvent(selectDataUid);
    }

    public void onPauseButtonEvent()
    {
        if (pauseEvent != null && selectDataUid != string.Empty)
            pauseEvent(selectDataUid);
    }

    public void onStopButtonEvent()
    {
        if (stopEvent != null && selectDataUid != string.Empty)
            stopEvent(selectDataUid);            
    }

    public void onSliderEvent()
    {
        if (sliderEvent != null && selectDataUid != string.Empty)
            sliderEvent(selectDataUid, (int)slider.value);
    }

    public void onLoadButtonEvent()
    {
        WindowBrowser windowBrewser = new WindowBrowser();
        string url_objFile = windowBrewser.ShowFileDlg();
        if (windowBrewserEvent != null)
            windowBrewserEvent(url_objFile);
    }



}
