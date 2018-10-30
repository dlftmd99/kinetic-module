using RenderHeads.Media.AVProVideo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallControlHandler : MonoBehaviour
{
    //Delegate
    public delegate void CurrentPlayValueEvent(int currentValue);
    public event CurrentPlayValueEvent currentPlayValueEvent;

    //Class
    public MainController mainController;

    //Other
    public List<GameObject> objList = new List<GameObject>();
    public List<Coroutine> coroutineList = new List<Coroutine>();
    public string currentUid = string.Empty;
    public float startTime = 0;
    public float currentTime = 0;
    public int currentValue = 0;


    /*
    public GameObject gameObject;
    private void Start()
    {
        test();
    }
    private void test()
    {
        for (int a = 0; a < gameObject.transform.GetChildCount(); a++)
        {
            for (int b = 0; b < gameObject.transform.GetChild(a).GetChildCount(); b++)
            {
                objList.Add(gameObject.transform.GetChild(a).GetChild(b).GetChild(1).gameObject);
            }
        }
    }
    */


    private void Awake()
    {
        objList = mainController.objList;
    }

    private void Update()
    {
        currentTime = Time.time;
    }

    private void resetStartTime()
    {
        startTime = Time.time;
    }

    public void sliderPlay(string dataUid, int value)
    {
        currentValue = value;
        DataInfo dataInfo = DB_Data.Instance.getData_sceneDataByUid(dataUid);
        for (int a = 0; a < objList.Count; a++)
        {
            try
            {
                Vector3 pos = objList[a].transform.position;
                objList[a].transform.position = new Vector3(pos.x, pos.y, dataInfo.heightList[currentValue].floatLIst[a]);
                settingOffset(dataUid, a);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message + " / " + a);
            }

        }
    }

    private void settingOffset(string dataUid, int value)
    {
        return;
        DataInfo dataInfo = DB_Data.Instance.getData_sceneDataByUid(dataUid);
        Vector2 offset = objList[value].transform.GetChild(0).GetComponent<ApplyToMaterial>()._offset;
        //float ratioValue = (objList[value].transform.position.y - 0.285f) / (0.8839998f - 0.285f);
        //float offsetValue = -0.46f * ratioValue;
        float ratioValue = (objList[value].transform.position.y - 0.334f) / (0.934f - 0.334f);
        float offsetValue = (-0.46f * ratioValue);
        objList[value].transform.GetChild(0).GetComponent<ApplyToMaterial>()._offset = new Vector2(offset.x, offsetValue);
    }

    public void play(string dataUid)
    {
        if (currentUid == dataUid)
            return;
        currentUid = dataUid;
        coroutineList.Add(StartCoroutine(play_coroutine(dataUid)));
    }

    IEnumerator play_coroutine(string dataUid)
    {
        DataInfo dataInfo = DB_Data.Instance.getData_sceneDataByUid(dataUid);
        resetStartTime();
        int currentNumber = currentValue;
        while (currentValue < dataInfo.heightList.Count - 1)
        {
            currentValue = currentNumber + ((int)((currentTime - startTime) / (1f / (float)dataInfo.dataFps)));
            sliderPlay(dataUid, currentValue);
            if (currentPlayValueEvent != null)
                currentPlayValueEvent(currentValue);
            yield return null;
        }
        stop(dataUid);
    }

    public void pause(string dataUid)
    {
        currentUid = string.Empty;
        if (coroutineList.Count > 0)
        {
            for (int a = 0; a < coroutineList.Count; a++)
                StopCoroutine(coroutineList[a]);
            coroutineList.RemoveRange(0, coroutineList.Count);
        }
    }

    public void stop(string dataUid)
    {
        reset();
        pause(dataUid);
        DataInfo dataInfo = DB_Data.Instance.getData_sceneDataByUid(dataUid);
        Debug.Log(dataInfo.heightList[0].floatLIst.Count);
        for (int a = 0; a < dataInfo.heightList[0].floatLIst.Count; a++)
        {
            Vector3 pos = objList[a].transform.position;
            objList[a].transform.position = new Vector3(pos.x, pos.y, dataInfo.heightList[0].floatLIst[a]);
        }
        if (currentPlayValueEvent != null)
            currentPlayValueEvent(currentValue);
    }

    private void reset()
    {
        currentValue = 0;
        currentUid = string.Empty;
    }




}
