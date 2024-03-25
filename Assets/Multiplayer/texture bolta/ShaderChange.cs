using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderChange : MonoBehaviour
{
    // Start is called before the first frame update
    public MeshRenderer mr;

    void Start()
    {
        mr.material.SetColor("_EmissionColor", Color.black);


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        WeaponPick pick=FindObjectOfType<WeaponPick>();
        if (pick!=null)

        if (other.gameObject.tag == "Player"&&pick.Raycast==false)
        {

            red();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            black();

        }
    }

   public  void white(){
        Debug.Log("oq rang");
        mr.material.SetColor("_EmissionColor", Color.white);
    }
    public void red()
    {
        mr.material.SetColor("_EmissionColor", Color.red);


    }

    public void black()
    {
        mr.material.SetColor("_EmissionColor", Color.black);

    }
}
