using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AxeDamage : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Animator anim;
    public  Animator animaxe;
    public BoxCollider BX;
    public float damage = 5f;
    public float AttackzombieDamage = 50f;
    bool attack;
    //public GameObject Blood;
   
    void Start()
    {
        
        animaxe = GetComponent<Animator>();
        //BX = GetComponent<BoxCollider>();
        BX.enabled = false;
        attack= false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Invoke("attact_bx",0.7f);
            anim.SetBool("axeAttack", true);
            animaxe.SetBool("attack", true);
            Invoke("BACK", 0.7f);
            attack = true;
            Invoke("Resets", 0.1f);
            
        }
       
        
    }

    public void attact_bx()
    {

        BX.enabled = true;

    }
    private void BACK()
    {
        anim.SetBool("axeAttack", false);
        animaxe.SetBool("attack", false);
        
    }

    private void Resets()
    {
            BX.enabled = false;
            
        
    }

    public void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag == "taxta")
        {
            
            if (attack==true)
            {
                attack = false;

                MakeBarrekada damageS = other.gameObject.GetComponent<MakeBarrekada>();
                damageS.axeDamage(damage);

                if (damageS != null) { 
                }
            }

        }
        if (other.gameObject.tag == "Enemy")
        {
            if (attack == true)
            {
               // Instantiate(Blood,other.gameObject.transform.position, other.transform.rotation);
                attack = false;
                Debug.Log("bolta hujumi");
                Target target = other.transform.GetComponent<Target>();
                target.TakeDamage(AttackzombieDamage);
            }

        }
    }
    


}
