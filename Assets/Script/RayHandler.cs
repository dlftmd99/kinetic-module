using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHandler : MonoBehaviour
{
    public void createRayObject(GameObject pf_ray, Transform parent)
    {
        GameObject obj = Instantiate(pf_ray);
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localPosition = new Vector3(0, 0, 2);
        obj.transform.SetParent(parent);
    }
}
