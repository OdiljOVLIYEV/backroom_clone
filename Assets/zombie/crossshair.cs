using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossshair : MonoBehaviour
{
	public float dinamic=800f;
	
	public RectTransform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if(Input.GetKey(KeyCode.Mouse0)){
	    	
	    	target.sizeDelta=new Vector2(dinamic,dinamic);
	    	
	    	
	    }
	    /*if(Input.GetKey(KeyCode.Mouse0)){
	    	
	    	target.sizeDelta=new Vector2(354.9984F,340.8728F);
	    	
	    	
	    }*/
	    if(Input.GetKeyUp(KeyCode.Mouse0)){
	    	
	    	target.sizeDelta=new Vector2(703.7F,681.5F);
	    	
	    }
	    	
	    	
	    
    }
}
