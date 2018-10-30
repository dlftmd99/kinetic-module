using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_video
{
    private static DB_video _instance = null;
    public List<VideoInfo> videoInfoList = new List<VideoInfo>();

    public static DB_video Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DB_video();
            }
            return _instance;
        }
    }

    public void removeAllList()
    {
        videoInfoList.RemoveRange(0, videoInfoList.Count);
    }

    public void showList()
    {
        for(int a=0; a< videoInfoList.Count; a++)
        {
            Debug.Log("[DB_video [" + a + "] 번째 리스트 정보]\n"
                     +"######################################################");
            Debug.Log("[videoName] : " + videoInfoList[a].videoName);
            Debug.Log("[videoUid] : " + videoInfoList[a].videoUid);
            Debug.Log("[videoExtension] : " + videoInfoList[a].videoExtension);
            Debug.Log("[videoUrl] : " + videoInfoList[a].videoUrl);
            Debug.Log("######################################################\n");
        }
    }

}

public class VideoInfo
{
    public string videoName = string.Empty;
    public string videoUid = string.Empty;
    public string videoUrl = string.Empty;
    public string videoExtension = string.Empty;
    //public MediaPlayer mediaPlayer = null;
    public GameObject obj_mediaPlayer;
}
