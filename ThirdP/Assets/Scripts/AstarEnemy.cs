using DamageNumbersPro;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AstarEnemy : MonoBehaviour
{
    [SerializeField]
    private NewEnemyBehavior enemyB;
    //[SerializeField]
    //private Flocking enemyB;
   
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
    public int SwordDamage = 25;
    //private MeshRenderer mesh; 
    void Awake()
    {

        
       
            animator = GetComponentInChildren<AnimatedMesh>();
        
       
        
        Parent = FindObjectOfType<MySolidSpawner>();
      
        capsuleCollider = GetComponent<CapsuleCollider>();
       
    }
    private void OnEnable()
    {

        currentHealth = health;
        capsuleCollider.enabled = true;

    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
           
            this.gameObject.SetActive(false);
            Parent.spawnedPrefabs.Remove(this.gameObject);
            //Parent.ReturnObjectToPool(this.gameObject);
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
            DealDamage(SwordDamage * 4);
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
       
       
            //animator.Play("death");
            //animator.Play("Dying");
            animator.Play("Death2");

      
        capsuleCollider.enabled = false;
       
        
        StartCoroutine(Dying());

    }
    IEnumerator Dying()
    {
        yield return new WaitForSeconds(2.6f);
        this.gameObject.SetActive(false);
        Parent.spawnedPrefabs.Remove(this.gameObject);
        //Parent.ReturnObjectToPool(this.gameObject);
    }
    void DamageN(float value, string st)
    {
        DamageNumber damageNumber = numberPrefab.Spawn(transform.position + Vector3.up * value, st);
    }
}

