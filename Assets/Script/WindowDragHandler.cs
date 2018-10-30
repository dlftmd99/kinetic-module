using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject targetController;
    private WindowController windowController;

    void Start()
    {
        if (targetController != null)
        {
            windowController = targetController.GetComponent<WindowController>();
        }
        else
        {
            Debug.LogError("There is no gameobject having windowController.");
        }
    }

    /***************************************************************************
	* Event handlers
	****************************************************************************/
    public void OnDrag(PointerEventData eventData)
    {
        windowController.move_MouseDrag(this.gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        windowController.start_MouseDrag(this.gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        windowController.onDragEnd(this.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        windowController.OnMouseDownOnTitle(this.gameObject);
    }
}
