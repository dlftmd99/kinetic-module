using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    //Class
    public MainController mainController;

    //Other
    public List<GameObject> screenList = new List<GameObject>();
    public GameObject pf_ray;
    public Transform parent;



    private void Awake()
    {
        screenList = mainController.objList;
        addScreenHandler(screenList);
        addRayHandler(pf_ray);
    }

    private void Start()
    {
        //addScreenHandler(screenList);
        //addRayHandler(pf_ray);
    }

    private void addScreenHandler(List<GameObject> screenList)
    {
        for (int a = 0; a < screenList.Count; a++)
            screenList[a].AddComponent<ScreenHandler>().screenNumber = a;
    }

    private void addRayHandler(GameObject obj)
    {
        for (int a = 0; a < screenList.Count; a++)
            screenList[a].AddComponent<RayHandler>().createRayObject(pf_ray, parent);
    }
    

    private void test()
    {
        for(int a=0; a< gameObject.transform.GetChildCount(); a++)
        {
            for(int b=0; b< gameObject.transform.GetChild(a).GetChildCount(); b++)
            {
                screenList.Add(gameObject.transform.GetChild(a).GetChild(b).GetChild(1).gameObject); 
            }
        }
    }






}
