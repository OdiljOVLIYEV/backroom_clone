using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{   
	public GameObject prefab;
	
	public int Xpos;
	public int Zpos;

	public int enemycount;
    // Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(Enemydrop());
    }

    // Update is called once per frame
	IEnumerator Enemydrop(){
		
		while(enemycount<20){
			
			Xpos=Random.Range(6,15);
			Zpos=Random.Range(10,24);
			Instantiate(prefab,new Vector3(Xpos,0,Zpos),Quaternion.identity);
			yield return new WaitForSeconds(0.1f);
			enemycount+=1;
		}
		
		
	}
}
