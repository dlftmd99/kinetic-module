using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainController : MonoBehaviour
{
    //Class
    public VideoManager videoManager;
    public PopupManager popupManager;
    public ObjLoadManager objLoadManager;
    public FileControlManager fileControlManager;
    public WallControlHandler wallControlHandler;
    public TextManager textManager;
    
    //Handler
    public ObjPopupHandler objPopupHandler;
    public VideoPopupHandler videoPopupHandler;
    
    //OBJ
    public List<GameObject> objList = new List<GameObject>();

    //Other
    private GameObject obj_popup;
    private string dataFolderUrl = string.Empty;
    private string videoFolderUrl = string.Empty;
    public bool isReady = false;

    private void Start()
    {
        Debug.Log("[MainController:Start]---------------------------------------");
        Screen.SetResolution(1920, 1080, false);
        //셋팅
        settingUrls();
        settingDelegateCallback();
        //비디오 폴더 유무 체크
        checkVideoFolder();
    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
            DB_Data.Instance.showList();

        if (Input.GetKeyDown("2"))
            DB_video.Instance.showList();
    }


    //**VIDEO
    //비디오 폴더 유무 체크
    private void checkVideoFolder()
    {
        if (fileControlManager.checkExists(videoFolderUrl, FILEFOLDER_TYPE.folder))
            checkVideoFolder_true();
        else
            checkVideoFolder_fail();
    }

    //비디오 폴더 없을시
    private void checkVideoFolder_fail()
    {
        Debug.Log("[MainController:checkVideoFolder_fail]");
        fileControlManager.createFolderComplete += createVideoFolderComplete;
        fileControlManager.createFolder(videoFolderUrl);
    }

    //비디오폴더 없을시 만들어주고 데이터 폴더 단계로 넘어가기
    private void createVideoFolderComplete(bool result, string errMsg)
    {
        fileControlManager.createFolderComplete -= createVideoFolderComplete;
        if (!result)
        {
            popupManager.activePopup(POPUP_TYPE.err);
            obj_popup = popupManager.getPopup(POPUP_TYPE.err);
            obj_popup.GetComponent<ErrPopup>().setErrText(errMsg);
        }
        if (!isReady)
            checkDataFolder();
    }

    //비디오 폴더 있을시
    private void checkVideoFolder_true()
    {
        Debug.Log("[MainController:checkVideoFolder_true]");
        DirectoryInfo directoryInfo = fileControlManager.getDirectoryInfo(videoFolderUrl);
        if (directoryInfo != null)
        {
            FileInfo[] files = directoryInfo.GetFiles();
            List<string> urlList = new List<string>();

            //폴더 안에 있는 비디오중 mp4확장자의 비디오Url을 리스트에 넣음 
            for (int a = 0; a < files.Length; a++)
            {
                string videUrl = files[a].FullName;
                string extension = fileControlManager.getFileExtension(videUrl);
                if (extension == "mp4")
                    urlList.Add(videUrl);
            }
            urlList = fileControlManager.SortNumericallyFileUrl(urlList);
            if (urlList.Count > 15)
                urlList.RemoveRange(15, urlList.Count - 15);
            for (int b = 0; b < urlList.Count; b++)
            {
                VideoInfo videoInfo = new VideoInfo();
                videoInfo.videoName = fileControlManager.getFileName(urlList[b]);
                videoInfo.videoUid = UidCreater.Instance.createUid();
                videoInfo.videoExtension = "mp4";
                videoInfo.videoUrl = urlList[b];
                //DB저장
                DB_video.Instance.videoInfoList.Add(videoInfo);
            }

            //비디오 로드
            loadVideo();
        }
    }

    //비디오 로드
    private void loadVideo()
    {
        List<string> urlList = new List<string>();
        if (DB_video.Instance.videoInfoList.Count <= 0)
            return;
        for (int a = 0; a < 1; a++)
            urlList.Add(DB_video.Instance.videoInfoList[a].videoUrl);
        videoManager.loadVideo(urlList);
    }

    //**DATA
    //데이터 폴더 유무 체크
    private void checkDataFolder()
    {
        //Debug.Log("[MainController:checkDataFolder] : " + fileControlManager.checkExists(dataFolderUrl, FILEFOLDER_TYPE.folder));
       
        if (fileControlManager.checkExists(dataFolderUrl, FILEFOLDER_TYPE.folder))
            checkDataFolder_true();
        else
            checkDataFolder_fail();
    }

    //데이터 폴더 없을시
    private void checkDataFolder_fail()
    {
        Debug.Log("[MainController:checkDataFolder_fail]");
        fileControlManager.createFolderComplete += createDataFolderComplete;
        fileControlManager.createFolder(dataFolderUrl);
    }

    //데이터 폴더 없을시 만들어주고 준비완료
    private void createDataFolderComplete(bool result, string errMsg)
    {
        fileControlManager.createFolderComplete -= createDataFolderComplete;
        if (!result)
        {
            popupManager.activePopup(POPUP_TYPE.err);
            obj_popup = popupManager.getPopup(POPUP_TYPE.err);
            obj_popup.GetComponent<ErrPopup>().setErrText(errMsg);
        }
        setIsReady(true);
    }

    //데이터 폴더 있을시
    private void checkDataFolder_true()
    {
        //text로드해서 가공후 DB에 저장
        string textUrl = Application.streamingAssetsPath + "/Data";
        DirectoryInfo directoryInfo = new DirectoryInfo(textUrl);
        FileInfo[] fileInfos = directoryInfo.GetFiles();

        for(int a=0; a<fileInfos.Length; a++)
        {
            if(fileControlManager.getFileExtension(fileInfos[a].FullName) != "meta")
            {
                textManager.LoadText(fileInfos[a].FullName);
                objPopupHandler.createObjList(DB_Data.Instance.dataInfoList[DB_Data.Instance.dataInfoList.Count - 1].dataUid);
            }
        }
        popupManager.activeOffAllPopup();
        setIsReady(true);
    }



   



























    //**Callback
    private void windowBrewserEvent_callback(string url)
    {
        objLoadManager.loadObjectFiles(url);
    }

    private void loadVideoComplete_callback()
    {
        //비디오 로드 완료 후 데이터 폴더 단계로 넘어가기
        if (!isReady)
            checkDataFolder();
    }

    private void objPlayerHandlerselectToggleEvent_callback(string dataUid)
    {
        wallControlHandler.stop(dataUid);
    }

    private void objPlayerHandlerSliderEvent_callback(string dataUid, int value)
    {
        wallControlHandler.sliderPlay(dataUid, value);
    }

    private void objPlayerHandlerPlayEvent_callback(string dataUid)
    {
        wallControlHandler.play(dataUid);
        videoPlayerHandlerPlayEvent_callback();
    }

    private void objPlayerHandlerPauseEvent_callback(string dataUid)
    {
        wallControlHandler.pause(dataUid);
        videoPlayerHandlerPauseEvent_callback();
    }

    private void objPlayerHandlerStopEvent_callback(string dataUid)
    {
        wallControlHandler.stop(dataUid);
        videoPlayerHandlerStopEvent_callback();
    }

    private void wallControlHandlerCurrentPlayValueEven_callback(int value)
    {
        objPopupHandler.objPlayerHandler.setSliderValue(value);
    }

    private void deleteItemEvent_callback(string txtUrl)
    {
        fileControlManager.deleteFile(txtUrl);
    }

    private void changeDataFpsEvetn_callback(string dataUid)
    {
        textManager.saveHeightListData(dataUid);
    }

    private void videoPlayerHandlerPlayEvent_callback()
    {
        for (int a = 0; a < DB_video.Instance.videoInfoList.Count; a++)
            videoManager.playVideo(DB_video.Instance.videoInfoList[a].obj_mediaPlayer.GetComponent<MediaPlayer>());
    }

    private void videoPlayerHandlerPauseEvent_callback()
    {
        for (int a = 0; a < DB_video.Instance.videoInfoList.Count; a++)
            videoManager.pauseVideo(DB_video.Instance.videoInfoList[a].obj_mediaPlayer.GetComponent<MediaPlayer>());
    }

    private void videoPlayerHandlerStopEvent_callback()
    {
        for (int a = 0; a < DB_video.Instance.videoInfoList.Count; a++)
            videoManager.stopVideo(DB_video.Instance.videoInfoList[a].obj_mediaPlayer.GetComponent<MediaPlayer>());
    }

    private void videoPlayerHandlerReloadEvent_callback()
    {
        //비디오 삭제
        for (int a = 0; a < DB_video.Instance.videoInfoList.Count; a++)
            StartCoroutine(unloadVideo(DB_video.Instance.videoInfoList[0].obj_mediaPlayer.GetComponent<MediaPlayer>()));
        for (int a = 0; a < DB_video.Instance.videoInfoList.Count; a++)
            DestroyImmediate(DB_video.Instance.videoInfoList[a].obj_mediaPlayer);
        DB_video.Instance.removeAllList();
        //다시 로드
        checkVideoFolder();
    }

    IEnumerator unloadVideo(MediaPlayer mediaPlayer)
    {
        videoManager.unLoadVideo(mediaPlayer);
        yield return null;
    }

    //**Settings
    private void settingUrls()
    {
        videoFolderUrl = Application.streamingAssetsPath + "/Video";
        dataFolderUrl = Application.streamingAssetsPath + "/Data";

        Debug.Log("[settingUrls] : "+ videoFolderUrl + " / "+ videoFolderUrl);
    }

    private void settingDelegateCallback()
    {
        //ObjLoadManager
        objLoadManager.loadStart += loadStart;
        objLoadManager.progressDialogNumber += progressDialogNumber;
        objLoadManager.progressDialogText += progressDialogText;
        objLoadManager.loadFail += loadFail;
        objLoadManager.loadComplete += loadComplete;
        //ObjPopupHandler
        objPopupHandler.windowBrewserEvent += windowBrewserEvent_callback;
        objPopupHandler.objPlayerHandlerSliderEvent += objPlayerHandlerSliderEvent_callback;
        objPopupHandler.objPlayerHandlerPlayEvent += objPlayerHandlerPlayEvent_callback;
        objPopupHandler.objPlayerHandlerPauseEvent += objPlayerHandlerPauseEvent_callback;
        objPopupHandler.objPlayerHandlerStopEvent += objPlayerHandlerStopEvent_callback;
        objPopupHandler.selectToggle += objPlayerHandlerselectToggleEvent_callback;
        objPopupHandler.deleteItemEvent += deleteItemEvent_callback;
        objPopupHandler.changeDataFpsEvetn += changeDataFpsEvetn_callback;
        //VideoPopupHandler
        videoPopupHandler.videoPlayerHandlerPlayEvent += videoPlayerHandlerPlayEvent_callback;
        videoPopupHandler.videoPlayerHandlerPauseEvent += videoPlayerHandlerPauseEvent_callback;
        videoPopupHandler.videoPlayerHandlerStopEvent += videoPlayerHandlerStopEvent_callback;
        videoPopupHandler.videoPlayerHandlerReloadEvent += videoPlayerHandlerReloadEvent_callback;
        //WallControlHandler
        wallControlHandler.currentPlayValueEvent += wallControlHandlerCurrentPlayValueEven_callback;
        //VideoManager
        videoManager.loadVideoComplete += loadVideoComplete_callback;
    }

    //**Load Popup
    private void loadStart()
    {
        popupManager.activePopup(POPUP_TYPE.loading);
        obj_popup = popupManager.getPopup(POPUP_TYPE.loading);
    }

    private void progressDialogNumber(float value)
    {
        obj_popup = popupManager.getPopup(POPUP_TYPE.loading);
        obj_popup.GetComponent<LoadingPopup>().setBarSize(value);
    }

    private void progressDialogText(string text)
    {
        obj_popup = popupManager.getPopup(POPUP_TYPE.loading);
        obj_popup.GetComponent<LoadingPopup>().setUrlText(text);
    }

    private void loadFail(string errMsg)
    {
        popupManager.activePopup(POPUP_TYPE.err);
        obj_popup = popupManager.getPopup(POPUP_TYPE.err);
        obj_popup.GetComponent<ErrPopup>().setErrText(errMsg);
    }

    private void loadComplete(string dataUid)
    {
        textManager.saveHeightListData(dataUid);
        popupManager.activeoffPopup(POPUP_TYPE.loading);
        objPopupHandler.createObjList(dataUid);
        objPlayerHandlerStopEvent_callback(dataUid);
    }

    //GET & SET
    private void setIsReady(bool result)
    {
        isReady = result;
    }

    public bool getIsReady()
    {
        return isReady;
    }




    private void reset()
    {

    }


}
