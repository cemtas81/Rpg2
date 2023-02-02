using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silah : MonoBehaviour
{
    private SphereCollider coll;
    private bool canHit=true;
    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }
    public void HitDamage()
    {
        if (canHit)
        {
            //coll.enabled = false;
            StartCoroutine(Hitto());
            canHit = false;
           
        }
       
    }
    IEnumerator Hitto()
    {
      
        yield return new WaitForSeconds(2.3f);       
        coll.enabled = true;
        canHit = true;
        yield return new WaitForSeconds(0.1f);
        coll.enabled = false;
    }
    
}
