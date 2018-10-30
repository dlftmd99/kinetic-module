using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Data 
{

    private static DB_Data _instance = null;
    public List<DataInfo> dataInfoList = new List<DataInfo>();

    public static DB_Data Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DB_Data();
            }
            return _instance;
        }
    }

    //API
    public DataInfo getData_sceneDataByUid(string dataUid)
    {
        //Debug.Log("[DB_Data:getData_sceneDataByUid]");
        for (int a = 0; a < dataInfoList.Count; a++)
        {
            //Debug.Log(dataUid+" / "+ dataInfoList[a].dataUid);
            if (dataUid == dataInfoList[a].dataUid)
                return dataInfoList[a];
        }
        return null;
    }

    public void removeData(string dataUid)
    {
        for (int a = 0; a < dataInfoList.Count; a++)
        {
            if (dataUid == dataInfoList[a].dataUid)
                dataInfoList.RemoveAt(a);
        }
    }

    public void showList()
    {
        for (int a = 0; a < dataInfoList.Count; a++)
        {
            Debug.Log("[DB_Data [" + a + "] 번째 리스트 정보]\n"
                     + "######################################################");
            Debug.Log("[dataUid] : " + dataInfoList[a].dataUid);
            Debug.Log("[dataName] : " + dataInfoList[a].dataName);
            Debug.Log("[dataType] : " + dataInfoList[a].dataType);
            Debug.Log("[dataFps] : " + dataInfoList[a].dataFps);
            Debug.Log("[dataLed] : " + dataInfoList[a].dataLed);
            Debug.Log("[dataUrl] : " + dataInfoList[a].dataUrl);
            for(int b = 0; b< dataInfoList[a].heightList.Count; b++)
            {
                string str = string.Empty;
                for (int c = 0; c < dataInfoList[a].heightList[b].floatLIst.Count; c++)
                    str += dataInfoList[a].heightList[b].floatLIst[c] + ",";
                Debug.Log("[heightList][" + b + "]" + str);
            }
            Debug.Log("######################################################\n");
        }
    }


}




public class DataInfo
{
    public GameObject dataObj;
    public string dataUid; //auto set 
    public string dataName;
    public string dataType;  //   load/external  삭제예정
    public string dataUrl;
    public int dataNumberofFrame;
    public int dataFps = 30;
    public string dataLed; //삭제예정
    public List<Height> heightList = new List<Height>();
    //public List<Mesh> meshList;
}

public class Height
{
    public List<float> floatLIst = new List<float>();
}