using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class MainCharacterScript : MonoBehaviour
{
    // constant attributes
    const int MAX_HP = 3;
    const int MOVE_SPEED_STOP = 0;
    const int MOVE_SPEED_WALK = 3;
    const int MOVE_SPEED_RUN = 5;
    const int ENEMY_DISTANCE_FEAR = 50;
    NavMeshAgent navMeshAgent;

    // dynamic attributes
    private int currentHP = MAX_HP;
    private int currentMovementSpeed = 0;
    bool isDistracted = false;
    public UIManager UIM;

    // Amount of pickusps in inventory
    int NrKeys = 0;

    // setters and incrementers
    public void setCurrentHP(int hp) { currentHP = hp; }

    public void incrementCurrentHP(int delta) { currentHP += delta; }

    public void setCurrentMovementSpeed(int speed)
    {
        if (speed != MOVE_SPEED_RUN && speed != MOVE_SPEED_WALK && speed != MOVE_SPEED_STOP)
        {
            Debug.Log("Movement speed input unrecognized, please input stop, walk, or run");
        }
        currentMovementSpeed = speed;
        navMeshAgent.speed = currentMovementSpeed;
    }

    public void hit()
    {
        incrementCurrentHP(-1);
        UIM.setHearts(currentHP);
        if (currentHP <= 0)
        {
            // then perish
        }

    }

    // only walking or running, so no incrementation of speed
    //public void incrementCurrentMovementSpeed(int delta) {   currentMovementSpeed += delta;  }

    // Function to that rolls to see if character becomes distracted by environment/object
    //   then runs the distraction anim and sets bool
    void rollDistract()
    {
        if (isDistracted) return;

        int roll = (int)Random.Range(0, 10000);
        if (roll < 1)
        {
            isDistracted = true;
            Debug.Log("Character has become distracted!");
            setCurrentMovementSpeed(MOVE_SPEED_STOP);
            // distraction animation start to finish then reset bool

            isDistracted = false;
        }
    }

    // Function to check for nearby enemies and return how many
    public int enemyScan()
    {
        int numEnemies = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = transform.position - enemy.transform.position;
            float distanceToEnemy = diff.sqrMagnitude;
            if (distanceToEnemy < ENEMY_DISTANCE_FEAR)
            {
                numEnemies++;
            }
        }
        return numEnemies;
    }

    public void Pickup(PickupTypes type)
    {
        if (type == PickupTypes.key)
        {
            NrKeys += 1;
            UIM.incrementKeys(1);
        }
    }

    public bool CheckForKey()
    {
        if (NrKeys > 0)
        {
            NrKeys--;
            UIM.incrementKeys(-1);
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        setCurrentMovementSpeed(MOVE_SPEED_WALK);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMovementSpeed == MOVE_SPEED_WALK && enemyScan() >= 3)
        {
            setCurrentMovementSpeed(MOVE_SPEED_RUN);
        }
        else if (currentMovementSpeed == MOVE_SPEED_RUN && enemyScan() < 3)
        {
            setCurrentMovementSpeed(MOVE_SPEED_WALK);
        }
        /*if (enemyScan() == 0 && !isDistracted)
        {
            rollDistract();
        }*/
    }
}