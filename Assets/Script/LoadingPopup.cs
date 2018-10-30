using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPopup : MonoBehaviour
{
    public Text text_url;
    public Image image_barBg;
    public Image image_bar;
	
	public void setUrlText(string url)
    {
        text_url.text = url;
    }

    public void setBarSize(float value)
    {
        float minWidth = 0;
        float maxWidth = image_barBg.rectTransform.sizeDelta.x;
        float newWidth = maxWidth * value;
        if (newWidth > maxWidth)
            newWidth = maxWidth;
        else if (newWidth < 0)
            newWidth = 0;
        Vector2 barSize = image_bar.rectTransform.sizeDelta;
        image_bar.rectTransform.sizeDelta = new Vector2(newWidth, barSize.y);
    }


}
