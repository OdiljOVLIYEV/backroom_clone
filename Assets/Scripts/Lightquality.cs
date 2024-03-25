using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightquality : MonoBehaviour
{
	public int light; 
    // Update is called once per frame
    void Update()
    {
	    QualitySettings.pixelLightCount = light;
    }
}
