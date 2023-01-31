using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAdd2 : MonoBehaviour
{
    [SerializeField]
    private NewEnemyBehavior enemyB;
    //[SerializeField]
    //private NewEnemyBehavior2 enemyB;
    [SerializeField]
    private NavMeshAgent agentA;
    [SerializeField]
    private NavMeshObstacle agentO;
    [SerializeField]
    private int health;
    private CapsuleCollider capsuleCollider;   
    private AnimatedMesh animator;
    private Animator animator2;
    [SerializeField]
    private ParticleSystem blood;
    public DamageNumber numberPrefab;    
    public MySolidSpawner Parent;
    private int currentHealth;
    public int SwordDamage=25;
  
    void Awake()
    {

        enemyB = FindObjectOfType<NewEnemyBehavior>();
        //enemyB = FindObjectOfType<NewEnemyBehavior2>();
        if (GetComponentInChildren<Animator>() != null)
        {
            animator2 = GetComponentInChildren<Animator>();
        }
        else if (GetComponentInChildren<Animator>() == null)
        {
            animator = GetComponentInChildren<AnimatedMesh>();
        }
        agentA = GetComponent<NavMeshAgent>();
        agentO = GetComponent<NavMeshObstacle>();
        Parent=FindObjectOfType<MySolidSpawner>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //agentA.updateRotation = false;
        NavMesh.avoidancePredictionTime = 0.1f;
    }
    private void OnEnable()
    {
       
        currentHealth = health;
        capsuleCollider.enabled = true;

    }
    private void OnTriggerEnter(Collider other)
    {
      
            if (other.CompareTag("Player"))
            {
                agentA.enabled = true;

            enemyB.agents.Add(agentA,agentO);
 
            if (agentA.isOnNavMesh == true)
                {
                    agentA.isStopped = false;

                }

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
            }
            enemyB.agents.Remove(agentA);
            //Parent.ReturnToPool(this.gameObject);
            Parent.spawnedPrefabs.Remove(this.gameObject);
            this.gameObject.SetActive(false);
        }

    }  
    private void OnCollisionEnter(Collision collision)
    {
       
            if (collision.gameObject.CompareTag("Sword"))
            {
                DealDamage(SwordDamage);
            }
            else if (collision.gameObject.CompareTag("Spell1"))
            {
                DealDamage(SwordDamage*4);
            }
        else if (collision.gameObject.CompareTag("Player"))
        {
            agentA.enabled = false;
        }
   
    }
    void DealDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            blood.Play();
            DamageN(1.5f, "hit");
        }

        if (currentHealth <= 0)
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
        if (animator2!=null)
        {
            animator2.SetBool("dead", true);
        }
        else if (animator2==null)
        {

            animator.Play("Death2");
        }      
        capsuleCollider.enabled = false;
        agentA.enabled = false;
        agentO.enabled = false;
        enemyB.agents.Remove(agentA);
       
        StartCoroutine(Dying());
        Parent.spawnedPrefabs.Remove(this.gameObject);
    }
    IEnumerator Dying()
    {
        yield return new WaitForSeconds(2.6f);
        //Parent.ReturnToPool(this.gameObject);
        this.gameObject.SetActive(false);
    }
    void DamageN(float value, string st)
    {
        DamageNumber damageNumber = numberPrefab.Spawn(transform.position + Vector3.up * value, st);
    }

}
