using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHandler : MonoBehaviour
{
    private void Awake()
    {
        setMaterial();
    }
	
    private void setMaterial()
    {
        ApplyToMaterial atm = this.gameObject.GetComponent<ApplyToMaterial>();
        if (atm != null)
            atm._material = this.GetComponent<Renderer>().material;
    }
}
