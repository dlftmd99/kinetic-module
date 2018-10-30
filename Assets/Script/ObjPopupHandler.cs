using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjPopupHandler : MonoBehaviour
{
    //Delegate
    public delegate void WindowBrewserEvent(string url);
    public event WindowBrewserEvent windowBrewserEvent;
    public delegate void SelectToggle(string dataUid);
    public event SelectToggle selectToggle;
    public delegate void ObjPlayerHandlerSliderEvent(string dataUid, int value);
    public event ObjPlayerHandlerSliderEvent objPlayerHandlerSliderEvent;
    public delegate void ObjPlayerHandlerPlayEvent(string dataUid);
    public event ObjPlayerHandlerPlayEvent objPlayerHandlerPlayEvent;
    public delegate void ObjPlayerHandlerPauseEvent(string dataUid);
    public event ObjPlayerHandlerPauseEvent objPlayerHandlerPauseEvent;
    public delegate void ObjPlayerHandlerStopEvent(string dataUid);
    public event ObjPlayerHandlerStopEvent objPlayerHandlerStopEvent;
    public delegate void DeleteItemEvent(string txtUrl);
    public event DeleteItemEvent deleteItemEvent;
    public delegate void ChangeDataFpsEvetn(string dataUid);
    public event ChangeDataFpsEvetn changeDataFpsEvetn;

    //Handler
    public ObjPlayerHandler objPlayerHandler;
    public List<GameObject> objItemHandlerList = new List<GameObject>();
    //Other
    public ToggleGroup toggleGroup;
    public GameObject objListBox;
    public GameObject pf_item;

    private void Start()
    {
        setDelegateCallback();
    }

    private void Update()
    {

    }

    public void createObjList(string dataUid)
    {
        GameObject item = Instantiate(pf_item);
        RectTransform rt_item = item.GetComponent<RectTransform>();
        ObjItemHandler objItemHandler = item.GetComponent<ObjItemHandler>();
        rt_item.SetParent(objListBox.GetComponent<RectTransform>());
        rt_item.localScale = new Vector3(1, 1, 1);
        rt_item.localPosition = new Vector3(3, ((-3 + (-80 * (objItemHandlerList.Count + 1))) + 80), 0);
        objItemHandler.setUpdatingData(dataUid);
        objItemHandler.toggle_select.group = toggleGroup;
        objItemHandler.selectItemToggleEvent += selectItemToggleEvent_callback;
        objItemHandler.deleteItemButtonEvent += deleteItemButtonEvent_callback;
        objItemHandler.changeDataFps += changeDataFps_callback;
        objItemHandlerList.Add(item);
        setObjListBoxSize(objItemHandlerList.Count);
        objItemHandlerList[objItemHandlerList.Count - 1].GetComponent<ObjItemHandler>().setToggleIsOn(true);
    }

    private void setObjListBoxSize(float count)
    {
        RectTransform rt = objListBox.GetComponent<RectTransform>();
        count = (count * 80) + 6; //리스트 아이템 하나당 사이즈 80 + 위아래 여분 3씩(6)
        if (count < 80)
            count = 80;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, count);
    }

    private void removeItem(string dataUid)
    {
        for(int a=0; a< objItemHandlerList.Count; a++)
        {
            string itemUid = objItemHandlerList[a].GetComponent<ObjItemHandler>().dataInfo.dataUid;
            if (dataUid == itemUid)
            {
                DestroyImmediate(objItemHandlerList[a]);
                objItemHandlerList.RemoveAt(a);
            } 
        }
    }

    private void rePositionItems()
    {
        for(int a=0; a< objItemHandlerList.Count; a++)
        {
            RectTransform rt_item = objItemHandlerList[a].GetComponent<RectTransform>();
            rt_item.localPosition = new Vector3(3, ((-3 + (-80 * (a + 1))) + 80), 0);
        }
    }


    //**Callback
    private void windowBrewserEvent_callback(string url)
    {
        if (windowBrewserEvent != null)
            windowBrewserEvent(url);
    }

    private void selectItemToggleEvent_callback(string dataUid)
    {
        objPlayerHandler.setSelectDataUid(dataUid);
        objPlayerHandler.setSliderMaxValue(dataUid);
        if (selectToggle != null)
            selectToggle(dataUid);
    }

    private void deleteItemButtonEvent_callback(string dataUid)
    {
        string txtUrl = DB_Data.Instance.getData_sceneDataByUid(dataUid).dataUrl;
        objPlayerHandler.onStopButtonEvent();
        removeItem(dataUid);
        rePositionItems();
        DB_Data.Instance.removeData(dataUid);
        UidCreater.Instance.removeUid(dataUid);
        objPlayerHandler.removeItem_Callback(dataUid);
        Debug.Log(objPlayerHandler.selectDataUid +" / "+ dataUid);
        if (objItemHandlerList.Count > 0 && objPlayerHandler.selectDataUid == string.Empty)
            objItemHandlerList[0].GetComponent<ObjItemHandler>().setToggleIsOn(true);
        if (deleteItemEvent != null)
            deleteItemEvent(txtUrl);
    }

    private void changeDataFps_callback(string dataUid)
    {
        if (changeDataFpsEvetn != null)
            changeDataFpsEvetn(dataUid);
    }

    private void sliderEvent_callback(string dataUid, int value)
    {
        if (objPlayerHandlerSliderEvent != null)
            objPlayerHandlerSliderEvent(dataUid, value);
    }

    private void playEvent_callback(string dataUid)
    {
        if (objPlayerHandlerPlayEvent != null)
            objPlayerHandlerPlayEvent(dataUid);
    }

    private void pauseEvent_callback(string dataUid)
    {
        if (objPlayerHandlerPauseEvent != null)
            objPlayerHandlerPauseEvent(dataUid);
    }

    private void stopEvent_callback(string dataUid)
    {
        if (objPlayerHandlerStopEvent != null)
            objPlayerHandlerStopEvent(dataUid);            
    }

    //**Event
    public void onDeleteButtonEvent()
    {
        this.gameObject.SetActive(false);
    }


    //*Setting
    private void setDelegateCallback()
    {
        objPlayerHandler.windowBrewserEvent += windowBrewserEvent_callback;
        objPlayerHandler.sliderEvent += sliderEvent_callback;
        objPlayerHandler.playEvent += playEvent_callback;
        objPlayerHandler.pauseEvent += pauseEvent_callback;
        objPlayerHandler.stopEvent += stopEvent_callback;
    }


}
