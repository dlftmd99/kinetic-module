using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrPopup : MonoBehaviour
{
    public Text text_err;

    public void setErrText(string text)
    {
        text_err.text = text;
        Invoke("activeOff_invoke", 3f);
    }

    private void activeOff_invoke()
    {
        text_err.text = string.Empty;
        this.gameObject.SetActive(false);
    }
}
