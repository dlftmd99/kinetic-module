using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayManager : MonoBehaviour
{
    public GameObject rayObjectRoot;
    public List<Transform> rayTransformList = new List<Transform>();
    //public List<Transform> movingPositionList = new List<Transform>();
    public bool isRayPlay = false;
    private RaycastHit hit;
    private float[] heightValue;
    private float maxRayDepth = 100;
    private float limitMinValue = 0.334f;
    private float limitMaxValue = 0.934f;
    //
    //public List<GameObject> objList = new List<GameObject>();





    private void Start()
    {
        for (int a = 0; a < rayObjectRoot.transform.childCount; a++)
            rayTransformList.Add(rayObjectRoot.transform.GetChild(a).transform);
        heightValue = new float[rayTransformList.Count];
        //StartCoroutine(update_coroutine());
    }

    public void startRay()
    {
        isRayPlay = true;
    }

    public void stopRay()
    {
        isRayPlay = false;
    }

    public float[] getHeightValue()
    {
        return heightValue;
    }

    IEnumerator update_coroutine()
    {
        float time = 0f;
        float delay = 0f;
        while (true)
        {
            if (isRayPlay)
            {
                time += Time.deltaTime;
                if (time > delay)
                {
                    delay = 0.0333f;
                    for (int a = 0; a < rayTransformList.Count; a++)
                        ray(rayTransformList[a], a);
                    time = 0;
                }
            }
            yield return null;
            yield return null;
        }
    }

    private void Update()
    {
        float time = 0f;
        float delay = 0f;
        if (isRayPlay)
        {
            time += Time.deltaTime;
            if (time > delay)
            {
                delay = 0.0333f;
                for (int a = 0; a < rayTransformList.Count; a++)
                    ray(rayTransformList[a], a);
                time = 0;
            }
        }
    }

    private void ray(Transform tramsform, int count)
    {
        Physics.Raycast(tramsform.position, Vector3.back, out hit, maxRayDepth);
        //Debug.Log(hit.point.z);
        heightValue[count] = hit.point.z;// - 0.706f;// - 0.6327831f; //+ 0.1179831f;// - 0.5148001f;

        //heightValue[count] = heightValue[count].ToString("N2");

        heightValue[count] = float.Parse(heightValue[count].ToString("N5"));

        //최저값 제한
        /*
        if (heightValue[count] < limitMinValue)
            heightValue[count] = limitMinValue;
        //최고값
        if (heightValue[count] > limitMaxValue)
            heightValue[count] = limitMaxValue;
        */

        Debug.DrawLine(tramsform.position, hit.point, Color.green);
    }


}
