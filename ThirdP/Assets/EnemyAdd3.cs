using FSG.MeshAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;
using AnimationInstancing;
public class EnemyAdd3 : MonoBehaviour
{
    [SerializeField]
    private InstEnemy enemyB;
    [SerializeField]
    private NavMeshAgent agentA;
    [SerializeField]
    private int health;
    private AnimationManager animator;
    [SerializeField]
    private ParticleSystem blood;
    public DamageNumber numberPrefab;
    //public MyPool Parent;
    void Awake()
    {
        enemyB = FindObjectOfType<InstEnemy>();
        agentA = GetComponent<NavMeshAgent>();
        //animator = GetComponentInChildren<AnimationInstancingMgr>();
        animator =FindObjectOfType<AnimationManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agentA.enabled = true;

     
            if (agentA.isOnNavMesh == true)
            {
                agentA.isStopped = false;
                enemyB.agents.Add(agentA);
                
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
                //animator.Play("Idle");
                
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
        if (health > 0)
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
        //animator.Play("death");
        agentA.GetComponent<Collider>().enabled = false;
        agentA.enabled = false;
        enemyB.agents.Remove(agentA);
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
