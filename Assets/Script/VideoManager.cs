using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : MonoBehaviour
{
    //Delegate
    public delegate void LoadVideoComplete();
    public event LoadVideoComplete loadVideoComplete;
    public delegate void LoadVideoFail();
    public event LoadVideoFail loadVideoFail;
    public delegate void UnloadComplete();
    public event UnloadComplete unloadComplete;
    //Other
    public GameObject pf_mediaPlayerObject;
    //new screen
    public Material material_screen;


    public void loadVideo(List<string> urls)
    {
        for(int a=0; a<urls.Count; a++)
            StartCoroutine(load(urls[a]));
        if (loadVideoComplete != null)
            loadVideoComplete();
    }

    private IEnumerator load(string url)
    {
        GameObject obj_mediaPlayer = Instantiate(this.pf_mediaPlayerObject);
        obj_mediaPlayer.name = "pf_mediaPlayer";
        MediaPlayer mediaPlayer = obj_mediaPlayer.GetComponent<MediaPlayer>();
        ApplyToMaterial applyToMaterial = obj_mediaPlayer.GetComponent<ApplyToMaterial>();
        mediaPlayer.m_VideoLocation = MediaPlayer.FileLocation.AbsolutePathOrURL;
        mediaPlayer.m_AutoStart = false;
        mediaPlayer.m_Loop = false;
        mediaPlayer.m_VideoPath = url;
        mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
        mediaPlayer.OpenVideoFromFile(mediaPlayer.m_VideoLocation, mediaPlayer.m_VideoPath, mediaPlayer.m_AutoStart);
        
        //DB저장
        for (int a = 0; a < DB_video.Instance.videoInfoList.Count; a++)
        {
            if (DB_video.Instance.videoInfoList[a].videoUrl == url)
            {
                DB_video.Instance.videoInfoList[a].obj_mediaPlayer = obj_mediaPlayer;
                //applyToMaterial._material = materialList[a];
                applyToMaterial._material = material_screen;
            }
        }

        yield return null;
    }

    public void unLoadVideo(MediaPlayer mediaPlayer)
    {
        mediaPlayer.CloseVideo();
    }
    
    public void playVideo(MediaPlayer mediaPlayer)
    {
        mediaPlayer.Play();
    }

    public void pauseVideo(MediaPlayer mediaPlayer)
    {
        mediaPlayer.Pause();
    }

    public void stopVideo(MediaPlayer mediaPlayer)
    {
        mediaPlayer.Rewind(true);
    }

    public void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.MetaDataReady:
                // Android platform doesn't display its first frame until poked
                //Debug.Log("[VideoManager:MetaDataReady]");
                break;
            case MediaPlayerEvent.EventType.Closing:
                //Debug.Log("[VideoManager:Closing]");
                break;
            case MediaPlayerEvent.EventType.Error:
                //Debug.Log("[VideoManager:Error]");
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                //Debug.Log("[VideoManager:FinishedPlaying]");
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                //Debug.Log("[VideoManager:FirstFrameReady]");
                break;
            case MediaPlayerEvent.EventType.ReadyToPlay:
                //Debug.Log("[VideoManager:ReadyToPlay]");
                break;
            case MediaPlayerEvent.EventType.Stalled:
                //Debug.Log("[VideoManager:Stalled]");
                break;
            case MediaPlayerEvent.EventType.Started:
                //Debug.Log("[VideoManager:Started]");
                break;
            case MediaPlayerEvent.EventType.SubtitleChange:
                //Debug.Log("[VideoManager:SubtitleChange]");
                break;
            case MediaPlayerEvent.EventType.Unstalled:
                //Debug.Log("[VideoManager:Unstalled]");
                break;
        }
    }


}
