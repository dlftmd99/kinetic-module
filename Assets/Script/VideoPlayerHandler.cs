using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerHandler : MonoBehaviour
{
    public delegate void PlayEvent();
    public event PlayEvent playEvent;
    public delegate void PauseEvent();
    public event PauseEvent pauseEvent;
    public delegate void StopEvent();
    public event StopEvent stopEvent;
    public delegate void ReloadEvent();
    public event ReloadEvent reloadEvent;

    public void onPlayButtonEvent()
    {
        if (playEvent != null)
            playEvent();
    }

    public void onPauseButtonEvent()
    {
        if (pauseEvent != null)
            pauseEvent();
    }

    public void onStopButtonEvent()
    {
        if (stopEvent != null)
            stopEvent();
    }

    public void onReloadButtonEvent()
    {
        if (reloadEvent != null)
            reloadEvent();
    }

}
