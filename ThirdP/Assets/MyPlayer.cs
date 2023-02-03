using Pathfinding;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject Forcefield;
    [SerializeField]
    private GameObject spell1 ;
    [SerializeField]
    private ParticleSystem spell2 ;
    [SerializeField]
    private ParticleSystem spell2Child ;
    [SerializeField]
    private GameObject spell3 ;
    [SerializeField]  
    private GameObject spell4 ; 
    [SerializeField]  
    private GameObject spell5 ; 
    [SerializeField]  
    private GameObject spello ;
    [SerializeField]
    private List<Material> materials = new List<Material>();
    [SerializeField]
    private float value;
    [SerializeField]
    private Animator ani;
    public bool invisible;
    [SerializeField]
    private ProgressBar healthBar;
    [SerializeField]
    private int MaxHealth; 
    [SerializeField]
    private int CurrentHealth;
    private CharacterController characterController;
    private ThirdPersonController thirdPersonController;
    public bool dead;
    [SerializeField]
    private int Hit1;
    [SerializeField]
    private int HitBoss;
    public MySolidSpawner MySolidSpawner;
    public GameObject myPet;
    public TargetMover targetMover;
    public ProceduralGridMover ProceduralGridMover;
    public Transform mybot;
    public GameObject boomer;
    private bool canBoomerang;
    public float weapon;
    public GameObject harita;
    private void Start()
    {
        targetMover=Camera.main.GetComponent<TargetMover>();
        ProceduralGridMover = Camera.main.GetComponent<ProceduralGridMover>();
        healthBar.BarValue=MaxHealth;
        CurrentHealth=MaxHealth;
        //ani = GetComponent<Animator>(); 
        characterController = GetComponent<CharacterController>(); 
        thirdPersonController = GetComponent<ThirdPersonController>();
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            materials.AddRange(renders[i].materials);
        }
        SetValue(0);
    }
    private void LateUpdate()
    {

        var forward=healthBar.transform.position-Camera.main.transform.position;
        forward.Normalize();
        var up=Vector3.Cross(forward,Camera.main.transform.right);
        healthBar.transform.rotation=Quaternion.LookRotation(forward,up);
    }
    void Update()
    {
        if (!dead)
        {
            if (Input.GetAxis("Mouse ScrollWheel")<0)
            {
                weapon = 0;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                weapon = 1;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (Forcefield.activeInHierarchy == false)
                {
                    Forcefield.SetActive(true);
                    StartCoroutine(Force());
                }

            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Spell1();

            }
            if (Input.GetMouseButtonDown(1))
            {
                Spell2();
            } 
            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                harita.SetActive(!harita.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                spell2.Play();
                spell2Child.Play();
                if (CurrentHealth<=75)
                {
                    CurrentHealth += 25;
                    healthBar.BarValue = CurrentHealth;
                }
                else
                    CurrentHealth = 100;
                    healthBar.BarValue = CurrentHealth;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

                if (value == 0)
                {
                    invisible = true;
                    //tag = "Finish";
                    StartCoroutine(LerpFunction(1, 1));
                    StartCoroutine(Inv());
                }

            }
            if (Input.GetMouseButtonDown(0))
            {
                if (weapon==0)
                {
                    Attack();
                }
                else if (weapon==1)
                {
                    if (spell4.activeInHierarchy == false && !dead && spell5.activeSelf == false && canBoomerang == false)
                    {
                        ani.SetTrigger("Attack");
                        StartCoroutine(AttackBoomerang());
                        canBoomerang = true;
                    }
                }

            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);

        } 
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (SceneManager.GetActiveScene().buildIndex==0)
            {
                SceneManager.LoadScene(1);
            }
            else 
            { 
                SceneManager.LoadScene(0);
            }
            

        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            targetMover.enabled = !targetMover.enabled;
            mybot.position =new Vector3(transform.position.x+2,transform.position.y+3,transform.position.z);
            myPet.SetActive(!myPet.activeInHierarchy);
            ProceduralGridMover.enabled=!ProceduralGridMover.enabled;
        }
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    if (spell4.activeInHierarchy == false && !dead && spell5.activeSelf == false&&canBoomerang==false)
        //    {
        //        ani.SetTrigger("Attack");
        //        StartCoroutine(AttackBoomerang());
        //        canBoomerang = true;
        //    }
           
        //}
    }
    public void Attack()
    {
        if (spell4.activeInHierarchy == false&&!dead&& spell5.activeSelf == false)
        {
            ani.SetTrigger("Attack");


            StartCoroutine(UnAttack());
        }
    }
    IEnumerator AttackBoomerang()
    {
        yield return new WaitForSeconds(.4f);
        GameObject clone;
        clone = Instantiate(boomer, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation) as GameObject;
        yield return new WaitForSeconds(1f);
        canBoomerang = false;
    }
    IEnumerator UnAttack()
    {
        yield return new WaitForSeconds(0.3f);
        spell4.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        spell4.SetActive(false);     
    }
    public void Spell1()
    {
        if (spello.activeSelf == false)
        {
            spello.SetActive(true);
            StartCoroutine(Back());
        }
    }
    public void Spell2()
    {
        if (spell5.activeSelf == false)
        {
            ani.SetTrigger("Fire");
            spell5.SetActive(true);
            StartCoroutine(Back2());
        }
    }

    IEnumerator Back()
    {
        yield return new WaitForSeconds(3) ;
        spello.SetActive(false);
      
    } 
    IEnumerator Back2()
    {
        yield return new WaitForSeconds(3) ;
        
        spell5.SetActive(false);
    }
    IEnumerator Inv()
    {
        yield return new WaitForSeconds(5);
        invisible = false;
        //tag = "Player";
        StartCoroutine(LerpFunction(0, 1)); 

    }
    IEnumerator Force()
    {
        yield return new WaitForSeconds(5);
        Forcefield.SetActive(false);
    }
    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = value;
        while (time < duration)
        {
            value = Mathf.Lerp(startValue, endValue, time / duration);          
            SetValue(value);
            time += Time.deltaTime;
            yield return null;
        }
        value = endValue;
        SetValue(value);
    }
    public void SetValue(float value)
    {

        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetFloat("_Dissolve", value);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
       
        //if (other.gameObject.CompareTag("Terrain"))
        //{
        //    MySolidSpawner.Spawn2();
        //}
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            DealDamage2(Hit1);
        }
        if (other.gameObject.CompareTag("BossWeapon"))
        {
            DealDamage2(HitBoss);
        }
    }
    void DealDamage2(int damage)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damage;
            //blood.Play();
            //DamageN(1.5f, "hit");
           
           healthBar.BarValue=CurrentHealth;
        }

        if (CurrentHealth <= 0)
        {
           
            Death();
     
        }

    }
    void Death()
    {
       
        ani.SetTrigger("death");
        //DamageN(.5f, "+execution");
        characterController.enabled = false; 
        thirdPersonController.enabled = false;
        dead=true;
        StartCoroutine(ReloadScene());
    }
    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
