using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPopupHandler : MonoBehaviour
{
    //Delegate
    public delegate void VideoPlayerHandlerPlayEvent();
    public event VideoPlayerHandlerPlayEvent videoPlayerHandlerPlayEvent;
    public delegate void VideoPlayerHandlerPauseEvent();
    public event VideoPlayerHandlerPauseEvent videoPlayerHandlerPauseEvent;
    public delegate void VideoPlayerHandlerStopEvent();
    public event VideoPlayerHandlerStopEvent videoPlayerHandlerStopEvent;
    public delegate void VideoPlayerHandlerReloadEvent();
    public event VideoPlayerHandlerReloadEvent videoPlayerHandlerReloadEvent;

    //Class
    public VideoPlayerHandler videoPlayerHandler;

    private void Start()
    {
        settingDelegate();
    }

    //**Callback
    private void playEvent_callback()
    {
        if (videoPlayerHandlerPlayEvent != null)
            videoPlayerHandlerPlayEvent();
    }

    private void pauseEvent_callback()
    {
        if (videoPlayerHandlerPauseEvent != null)
            videoPlayerHandlerPauseEvent();
    }

    private void stopEvent_callback()
    {
        if (videoPlayerHandlerStopEvent != null)
            videoPlayerHandlerStopEvent();
    }

    private void reloadEvent_callback()
    {
        if (videoPlayerHandlerReloadEvent != null)
            videoPlayerHandlerReloadEvent();
    }

    //**Setting Delegate
    private void settingDelegate()
    {
        videoPlayerHandler.playEvent += playEvent_callback;
        videoPlayerHandler.pauseEvent += pauseEvent_callback;
        videoPlayerHandler.stopEvent += stopEvent_callback;
        videoPlayerHandler.reloadEvent += reloadEvent_callback;
    }

    //**Event
    public void onDeleteButtonEvent()
    {
        this.gameObject.SetActive(false);
    }
}
