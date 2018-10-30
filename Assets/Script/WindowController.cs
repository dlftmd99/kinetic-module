using UnityEngine;
using System.Collections;

public class WindowController : MonoBehaviour
{

    /***************************************************************************
    * Title Bar based Window Drag
    ****************************************************************************/
    private Vector3 PositionCha;
    private Vector3 DragPos1;
    private Vector3 DragPos2;
    public bool isDraging = false;

    public void OnMouseDownOnTitle(GameObject target)
    {
        this.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void start_MouseDrag(GameObject target)
    {
        DragPos1 = Input.mousePosition;
        DragPos2 = Input.mousePosition;
        isDraging = true;
    }

    public void move_MouseDrag(GameObject target)
    {
        DragPos1 = Input.mousePosition;
        if (DragPos1 != DragPos2 && DragPos2 != null)
        {
            PositionCha = DragPos1 - DragPos2;
            DragPos2 = DragPos1;
        }
        else
        {
            DragPos2 = Input.mousePosition;
        }

        Vector3 p = this.gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        this.gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(p.x + PositionCha.x, p.y + PositionCha.y, p.z);
    }

    public void onDragEnd(GameObject target)
    {
        isDraging = false;
    }
}
