using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerdied : MonoBehaviour
{
	public GameObject menuUi;
	public GameObject player;
	
    // Start is called before the first frame update
    void Start()
    {
	    
    }

    // Update is called once per frame
    void Update()
	{
		PlayerHealt dead=FindObjectOfType<PlayerHealt>();
		
		if(dead.Healt<=0){
			
			GameObject move=GameObject.Find("FPS Controller");
			if(move!=null)
			move.GetComponent<PlayerMovment>().enabled=false;
			GameObject moves=GameObject.Find("Main Camera");
			moves.GetComponent<MouseLook>().enabled=false;
			Animator anim=GetComponent<Animator>();
			anim.SetBool("dead",true);
			Invoke("deads",3f);
			
		}
    	
        
	}
    
	void deads(){
		
		
		
		
		menuUi.SetActive(true);
		Invoke("Reload",3f);
		
	}
    
	void Reload(){
		
		SceneManager.LoadScene("SampleScene");
	}
}
