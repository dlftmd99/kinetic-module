using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjItemHandler : MonoBehaviour
{
    //Delegate
    public delegate void SelectItemToggleEvent(string dataUid);
    public event SelectItemToggleEvent selectItemToggleEvent;
    public delegate void DeleteItemButtonEvent(string dataUid);
    public event DeleteItemButtonEvent deleteItemButtonEvent;
    public delegate void ChangeDataFps(string dataUid);
    public event ChangeDataFps changeDataFps;

    //Other
    public DataInfo dataInfo;
    public Text text_name;
    public Text text_dataNumberofFrame;
    public InputField inputField_fps;
    public Toggle toggle_select;


    //**Set
    public void setUpdatingData(string dataUid)
    {
        DataInfo dataInfo = DB_Data.Instance.getData_sceneDataByUid(dataUid);
        if(dataInfo != null)
        {
            this.dataInfo = dataInfo;
            text_name.text = dataInfo.dataName;
            text_dataNumberofFrame.text = dataInfo.dataNumberofFrame.ToString();
            inputField_fps.text = dataInfo.dataFps.ToString();
        }
    }

    public void setToggleIsOn(bool isOn)
    {
        Debug.Log(dataInfo.dataUid+" / ");
        toggle_select.isOn = isOn;
    }


    //**Event
    public void onToggleEvent()
    {
        if(selectItemToggleEvent != null && toggle_select.isOn)
            selectItemToggleEvent(dataInfo.dataUid);
    }

    public void onInputFieldEvent(InputField inputField)
    {
        int newFps;
        if (int.TryParse(inputField.text, out newFps))
        {
            DB_Data.Instance.getData_sceneDataByUid(dataInfo.dataUid).dataFps = newFps;
            if (changeDataFps != null)
                changeDataFps(dataInfo.dataUid);
        }
    }

    public void onDeleteItemButtonEvent()
    {
        if (deleteItemButtonEvent != null)
            deleteItemButtonEvent(dataInfo.dataUid);
    }

}
