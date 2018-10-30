using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBarHandler : MonoBehaviour
{
    public void onTaskMenuButtonEvent(GameObject menu)
    {
        if (menu.activeSelf)
            menu.SetActive(false);
        else
            menu.SetActive(true);
        setMenuPosition(menu);
    }

    private void setMenuPosition(GameObject menu)
    {
        menu.GetComponent<RectTransform>().anchoredPosition = new Vector2(8f, -43f);
    }

}
