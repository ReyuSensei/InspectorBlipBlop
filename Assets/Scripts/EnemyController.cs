using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class EnemyController : MonoBehaviour
{
    public enum E_ENEMYSTATE { WAIT, PATROL, SEEK, ATTACK, FLEE, DAMAGE, DEAD }
    public enum E_ENEMYATTACK { NONE, MELEE, RANGED }

    [SerializeField]
    private E_ENEMYSTATE state;
    [SerializeField]
    private E_ENEMYATTACK attack;

    //===============COMPONENTS=========
    private NavMeshAgent agent;
    private Animator anim;

    //===============WAIT===============
    [SerializeField]
    private float waitTime;
    private float timeHasBeenWaiting;
    private bool isWaiting;

    //===============PATROL=============
    [SerializeField]
    private List<Transform> patrolWaypoints;
    private int currentPatrolWpIndex;

    //===============SEEK===============
    [SerializeField]
    private float detectionRadius;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private float raycastOffset;

    //===============ATTACK=============
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float attackCooldownTime;
    private float timeHasBeenInAttackCooldown;
    //MELEE
    [SerializeField]
    private Transform meleeWeaponSocket;
    [SerializeField]
    private Collider meleeWeaponCollider;
    //RANGED
    [SerializeField]
    private GameObject rangedAttackPrefab;
    [SerializeField]
    private Transform rangedAttackSpawnPoint;
    private bool isAttacking;

    //===============FLEE===============
    [SerializeField]
    private float fleeHealthThreshold;
    [SerializeField]
    private float fleeMaxDistance;

    //===============DAMAGE=============
    [SerializeField]
    private float maxHealth;
    private float currentHealth;
    private bool isInvincible;
    [SerializeField]
    private float invincibleCooldownTime;
    private float timeHasBeenInvincible;

    //===============MOVEMENT===========
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float rotationSpeed;

    private GameObject player;
    private float playerDistance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        if (meleeWeaponSocket != null)
        {
            meleeWeaponSocket.gameObject.SetActive(false);
        }
        if (meleeWeaponCollider != null)
        {
            meleeWeaponCollider.enabled = false;
        }

        agent.speed = 0;
        agent.isStopped = true;
        timeHasBeenWaiting = waitTime;

        player = GameManager.instance.playerController.gameObject;
    }

    void Update()
    {
        if (timeHasBeenInAttackCooldown > 0f)
        {
            timeHasBeenInAttackCooldown -= Time.deltaTime;
        }

        if (timeHasBeenWaiting > 0f)
        {
            timeHasBeenWaiting -= Time.deltaTime;
        }

        if (timeHasBeenInvincible > 0f)
        {
            timeHasBeenInvincible -= Time.deltaTime;
        }

        playerDistance = Vector3.Distance(transform.position, player.transform.position);

        HandleWait();
        HandlePatrol();
        HandleSeek();
        HandleAttack();
        HandleFlee();
        HandleDamage();
        HandleDead();
    }

    void HandleWait()
    {
        if(state != E_ENEMYSTATE.WAIT)
        {
            timeHasBeenWaiting = 0;
            return;
        }

        if(timeHasBeenWaiting <= 0f)
        {
            state = E_ENEMYSTATE.PATROL;
        }

        //COMPRUEBO SI PUEDO VER AL PLAYER
        if (CanSeePlayer())
        {
            CheckSeekOrFlee();
        }
        agent.speed = 0;
        agent.isStopped = true;
        anim.SetFloat("_speed", 0);
    }

    void HandlePatrol()
    {   
        if(state != E_ENEMYSTATE.PATROL)
        {
            return;
        }
        
        //COMPRUEBO SI PUEDO VER AL PLAYER
        if (CanSeePlayer())
        {
            CheckSeekOrFlee();
            return;
        }

        if(patrolWaypoints.Count == 0)
        {
            return;
        }
        
        agent.speed = walkSpeed;
        agent.isStopped = false;
        anim.SetFloat("_speed", agent.speed/runSpeed);  //DIVIDO WALKSPEED ENTRE RUNSPEED YA QUE AL SER RUNSPEED EL DOBLE DE WALKSPEED DA 0.5

        //COMPRUEBO SI HE LLEGADO AL DESTINO
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(patrolWaypoints[currentPatrolWpIndex].position);   //LE DIGO AL NAVMESH AGENT QUE INTENTE IR HACIA EL PUNTO
            currentPatrolWpIndex = currentPatrolWpIndex + 1; //currentPatrolWpIndex++;
            if(currentPatrolWpIndex >= patrolWaypoints.Count)
            {
                currentPatrolWpIndex = 0;
            }
            state = E_ENEMYSTATE.WAIT;
            timeHasBeenWaiting = waitTime;  //RESTABLEZCO EL TIEMPO DE ESPERA
        }

        /*
        for(int i = 0; i < patrolWaypoints.Count; i++)  //MIENTRAS EL NUMERO I SEA MENOR A LA CANTIDAD DE WAYPOINTS DEL INDICE, SE IR� SUMANDO SU CANTIDAD 
        {
            Debug.Log(patrolWaypoints[i].position);
        }
        */
    }

    void HandleSeek()
    {
        if (state != E_ENEMYSTATE.SEEK)
        {
            return;
        }

        if (CanSeePlayer())
        {
            CheckSeekOrFlee();
        }
        else
        {
            state = E_ENEMYSTATE.PATROL;
        }
        agent.SetDestination(player.transform.position); //LE DIGO AL AGENT QUE VAYA HACIA LA POSICION DEL PLAYER
        agent.speed = runSpeed;
        agent.isStopped = false;
        anim.SetFloat("_speed", agent.speed/runSpeed);  //AL DIVIDIRLO POR LA RUNSPEED DAR� 1 YA QUE ESTAR� CORRIENDO

        if(playerDistance <= attackRadius)
        {
            state = E_ENEMYSTATE.ATTACK;
        }
    }

    void HandleAttack()
    {
        if (state != E_ENEMYSTATE.ATTACK)
        {
            return;
        }

        if (playerDistance > attackRadius)
        {
            state = E_ENEMYSTATE.SEEK;
            return;
        }

        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        playerDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        agent.speed = 0;
        agent.isStopped = true;

        if (isAttacking == false && timeHasBeenInAttackCooldown <= 0)
        {
            isAttacking = true;
            timeHasBeenInAttackCooldown = attackCooldownTime;
            if (attack == E_ENEMYATTACK.MELEE)
            {
                anim.SetTrigger("_meleeAttack");

            }
            if (attack == E_ENEMYATTACK.RANGED)
            {
                anim.SetTrigger("_distanceAttack");
            }
        }
    }

    void HandleFlee()
    {
        if (state != E_ENEMYSTATE.FLEE)
        {
            return;
        }

        Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
        Vector3 fleeTarget = transform.position + (fleeDirection * fleeMaxDistance);

        if(NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, fleeMaxDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        agent.speed = runSpeed;
        agent.isStopped = false;
        anim.SetFloat("_speed", agent.speed/runSpeed);

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            state = E_ENEMYSTATE.WAIT;
        }
    }

    void HandleDamage()
    {
        if(state != E_ENEMYSTATE.DAMAGE)
        {
            return;
        }
        if (isInvincible == false)
        {
            CheckSeekOrFlee();
        }
    }

    void HandleDead()
    {

    }

    public void DoDamage(GameObject other)
    {
        if (state == E_ENEMYSTATE.DEAD)
        {
            return;
        }
        if (isInvincible == true)
        {
            return;
        }
        DamageDealer dmg = other.GetComponent<DamageDealer>();
        if (dmg != null)
        {
            currentHealth -= dmg.damageValue;
            if(currentHealth < 0)
            {
                state = E_ENEMYSTATE.DEAD;
            }
            else
            {
                state = E_ENEMYSTATE.DAMAGE;
                isInvincible = true;
                timeHasBeenInvincible = invincibleCooldownTime;
                anim.SetTrigger("_hit");
            }
        }
    }

    private bool CanSeePlayer()
    {
        if(player == null)
        {
            return false;
        }

        if(playerDistance > detectionRadius)
        {
            return false;
        }

        //RAYCAST: PUNTO DE ORIGEN, DIRECCION, DISTANCIA
        RaycastHit hit;
        Vector3 playerDirection = player.transform.position - transform.position;
        if(Physics.SphereCast(transform.position + (Vector3.up * raycastOffset), 0.5f, playerDirection, out hit, playerDistance))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    void CheckSeekOrFlee()
    {
        if(currentHealth < fleeHealthThreshold)
        {
            state = E_ENEMYSTATE.FLEE;
        }
        else
        {
            state = E_ENEMYSTATE.SEEK;
        }
    }

    public void EventShowMeleeWeapon()
    {
        meleeWeaponSocket.gameObject.SetActive(true);    //LOS GAMEOBJECTS SE ACTIVAN/DESACTIVAN CON UN .SETACTIVE
    }

    public void EventEnableMeleeCollider()
    {
        meleeWeaponCollider.enabled = true;   //LOS COMPONENTES SE ACTIVAN/DESACTIVAN CON .ENABLED
    }

    public void EventDisableMeleeCollider()
    {
        meleeWeaponCollider.enabled = false;
    }

    public void EventHideMeleeWeapon()
    {
        meleeWeaponSocket.gameObject.SetActive(false);
        isAttacking = false;
    }

    public void EventShowRangeWeapon()
    {
        //NO TIENE ARMA A DISTANCIA, SOLO LANZAR� EL PROYECTIL
        //rangedWeapon.SetActive(true);
    }

    public void EventFireRangeWeapon()
    {
        //CREO UN NUEVO PROYECTIL GENERANDO UNA INSTANCIA DE PREFAB, EN UN PUNTO CONCRETO Y CON UNA ROTACION CONCRETA
        GameObject projectile = Instantiate(rangedAttackPrefab, rangedAttackSpawnPoint.transform.position, Quaternion.identity); //QUATERNION.IDENTITY CREA UN QUATERNION CON LA ROTACION ORIGINAL DEL PREFAB
        projectile.GetComponent<Projectile>().attackDirection = transform.forward;
    }

    public void EventHideRangeWeapon()
    {
        //NO TIENE ARMA A DISTANCIA, SOLO LANZAR� EL PROYECTIL
        //rangedWeapon.SetActive(false);
        isAttacking = false;
    }
}