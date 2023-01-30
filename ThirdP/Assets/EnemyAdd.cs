using AnimationInstancing;
using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyAdd : MonoBehaviour
{
    [SerializeField]
    private EnemyBehavior enemyB;
    [SerializeField]
    private NavMeshAgent agentA;
    [SerializeField]
    private int health;
    //private Vector3 velocity=Vector3.zero;
    private Animator animator;
    [SerializeField]
    private ParticleSystem blood;
    public DamageNumber numberPrefab;
 
    
    //public MyPool Parent;
    void Awake()
    {
        enemyB = FindObjectOfType<EnemyBehavior>();
        agentA=GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        //agentA.updatePosition = false;
        //agentA.updateRotation = false;
        //Parent = FindObjectOfType<MyPool>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agentA.enabled = true;
        
            enemyB.agents.Add(agentA);
            
            //if (agentA.isOnNavMesh == true)
            //{
            //    agentA.isStopped = false;
            //}
        }
      
       
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (agentA.isOnNavMesh == true)
            {
                agentA.isStopped = true;
                agentA.velocity = Vector3.zero;

                animator.SetFloat("Speed", 0);
            }
            enemyB.agents.Remove(agentA);
            agentA.enabled = false;

        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            DealDamage(25);
         
           
        }
        else if (collision.gameObject.CompareTag("Spell1"))
        {
            DealDamage(200);
          
        }

    }
    void DealDamage(int damage)
    {
        if (health>0)
        {
            health -= damage;
            blood.Play();
            DamageN(1.5f, "hit");
        }

        if (health <= 0)
        {
            DamageN(.5f, "+execution");
            Death();
           
        }

    }
    void Death()
    {
        if (agentA.isOnNavMesh == true)
        {
            agentA.isStopped = true;
           
        }
        agentA.GetComponent<Collider>().enabled = false;
        agentA.enabled = false; 
        enemyB.agents.Remove(agentA);
        //animator.SetBool("wonder", false);
        animator.SetBool("dead", true);
        StartCoroutine(Dying());
        
    }
    IEnumerator Dying()
    {
        
        yield return new WaitForSeconds(3f);
        
        this.gameObject.SetActive(false);
       
        //Parent.pooledObjects.Add(this.gameObject);
    }
    void DamageN(float value, string st)
    {

        DamageNumber damageNumber = numberPrefab.Spawn(transform.position + Vector3.up * value, st);

    }
}
