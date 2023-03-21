using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public State currentState;
    public NavMeshAgent agent;
    public MovementScript target;
    public bool isActing = false;
    public List<AttackAction> possibleAttacks = new List<AttackAction>();
    Animator anim;
    [HideInInspector]
    public float distanceFromTarget;
    public AttackAction currentAttack;
    [Header("AI Settings")]
    public List<GameObject> projectiles = new List<GameObject>();
    public float currentRecoveryTime = 0;
    public Transform bulletSpawnPosition;
    public GameObject gun;
    public bool isMelee;
    public float shootAccAngle = 0;

    private void Start()
    {
        agent.speed = moveSpeed;
        target = FindObjectOfType<MovementScript>();
        anim = GetComponent<Animator>();
    }
    public void Update()
    {
        currentState = currentState.StateTick(this);
        distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
        HandleRecovery();
    }

    //if we wanna do fancy attacks a la enter the gungeon, this will have to be a constantly re-written function. 
    //to make things simple, more "unique" attack states will be their own states rather than this simple controller. 
    //this is only to be used when your projectile can be a simple prefab. 
    //this also only works if the enemy is facing towards the player.
    public void FireWeapon(int projectileType)
    {
        Vector3 shootTarget = target.transform.position;
        shootTarget.y += 0.5f;
        Vector3 shootVelocity = shootTarget - transform.position;
        if(shootAccAngle != 0)
        {
            float randAngle = Random.Range(-shootAccAngle, shootAccAngle);
            Vector3 angledVelocity = Quaternion.AngleAxis(randAngle, Vector3.forward) * shootVelocity;
            angledVelocity.y = 0;
            Instantiate(projectiles[projectileType], bulletSpawnPosition.position, Quaternion.LookRotation(angledVelocity));
        }else
        {
            Instantiate(projectiles[projectileType], bulletSpawnPosition.position, Quaternion.LookRotation(shootVelocity));

        }
    }
    public void FireRigidBullet(int projectileType)
    {
        Vector3 shootTarget = target.transform.position;
        shootTarget.y += 0.5f;
        Vector3 shootVelocity = shootTarget - transform.position;
        if (shootAccAngle != 0)
        {
            float randAngle = Random.Range(-shootAccAngle, shootAccAngle);
            Vector3 angledVelocity = Quaternion.AngleAxis(randAngle, Vector3.forward) * shootVelocity;
            angledVelocity.y = 0;
            Instantiate(projectiles[projectileType], bulletSpawnPosition.position, Quaternion.LookRotation(angledVelocity)).GetComponent<PlasmaBall>().SetVelocity(shootVelocity);
        }
        else
        {
            Instantiate(projectiles[projectileType], bulletSpawnPosition.position, Quaternion.LookRotation(shootVelocity)).GetComponent<PlasmaBall>().SetVelocity(shootVelocity);
            

        }
    }
    public void EndAction()
    {
        isActing = false;
    }
    void HandleRecovery()
    {
        if(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }
        if (currentRecoveryTime <= 0)
        {
            isActing = false;
        }
    }
    public void PlayAnimation(string animName, int animLayer)
    {
        anim.CrossFade(animName, 0.2f, animLayer);
    }
    public bool GetAttack()
    {
        Vector3 forwardsVec = gun.transform.forward;
        forwardsVec.y = 0;
        Vector3 lookVec = target.transform.position - transform.position;
        lookVec.y = 0;
        if (Vector3.Angle(forwardsVec, lookVec) < 4f)
        {

            int maxScore = 0;
            for (int i = 0; i < possibleAttacks.Count; i++)
            {
                AttackAction attackAction = possibleAttacks[i];
                if (distanceFromTarget <= attackAction.maxDistance && distanceFromTarget >= attackAction.minDistance)
                {
                    maxScore += attackAction.attackScore;
                }
            }
            if (maxScore == 0)
            {
                return false;
            }
            int randomValue = Random.Range(0, maxScore + 1);
            int tempScore = 0;
            for (int i = 0; i < possibleAttacks.Count; i++)
            {

                AttackAction attackAction = possibleAttacks[i];
                if (distanceFromTarget <= attackAction.maxDistance && distanceFromTarget >= attackAction.minDistance)
                {
                    if (currentAttack != null)
                    {
                        return true;
                    }
                    tempScore += attackAction.attackScore;
                    if (tempScore > randomValue)
                    {
                        currentAttack = attackAction;
                    }
                }
            }
            return true;
        }
        return false;
    }
    //Delete this crap later only here for prototype
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player Bullet"))
        {
            Debug.Log("Hello We Reached Here");
            Destroy(col.gameObject);
            //Take Damage
            Destroy(gameObject);
        }
    }

}
