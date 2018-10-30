using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenHandler : MonoBehaviour
{
    public int screenNumber;
    //public Vector2 offset;
    private Vector2 scale = new Vector2(.1f, .05f);
    private Material mt;
    private ApplyToMaterial atm;


    private void Start()
    {
        //Debug.Log(screenNumber);
        createMaterial();
        pushMeshRendererInMaterial(mt);
        addComponentApplyToMaterial();
        pushApplyToMaterialMaterial(mt);
        setApplyToMaterialOffset(atm);
        setApplyToMaterialScale(atm);
        setApplyToMaterialMedia(null);
    }

    private void createMaterial()
    {
        mt = new Material(Shader.Find("AVProVideo/Lit/Diffuse (texture+color+fog+stereo support)"));
    }

    private void pushMeshRendererInMaterial(Material mt)
    {
        gameObject.GetComponent<MeshRenderer>().material = mt;
    }

    private void addComponentApplyToMaterial()
    {
        atm = gameObject.AddComponent<ApplyToMaterial>();
    }

    private void pushApplyToMaterialMaterial(Material mt)
    {
        atm._material = mt;
    }

    private void setApplyToMaterialOffset(ApplyToMaterial atm)
    {
        float lineNumber = Mathf.Floor(screenNumber / 10);
        float rowNumber = screenNumber - (lineNumber * 10);
        atm._offset = new Vector2((0.1f* rowNumber), (-0.05f* lineNumber));
    }

    private void setApplyToMaterialScale(ApplyToMaterial atm)
    {
        atm._scale = scale;
    }

    private void setApplyToMaterialMedia(MediaPlayer mp)
    {
        if (mp != null)
            atm._media = mp;
        else
            StartCoroutine(findMediaPlayer());
    }

    private IEnumerator findMediaPlayer()
    {
        bool isState = false;
        if (!isState)
        {
            yield return new WaitForSeconds(0.3f);
            atm._media = GameObject.Find("pf_mediaPlayer").gameObject.GetComponent<MediaPlayer>();
            if (atm._media != null)
                isState = true;
        }
    }
}
