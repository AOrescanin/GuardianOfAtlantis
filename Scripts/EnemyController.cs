using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    // Health variables
    [SerializeField] private HealthBar healthbar;
    [SerializeField] private float maxHealth = 100f, armorPecentage, magicResistance, livesTaken;
    private float currentHealth;
    private bool isAlive = true;

    // Pathfinding variable
     private NavMeshAgent agent;
    private Vector3 dest;
    private bool destinationHasChanged = false;
    private float startSpeed;

    // Player/base detection variables
    private PlayerController player;
    private WeaponController weapon;
    [SerializeField] private LayerMask playerLayer, baseLayer, soldierLayer;
    [SerializeField] private float sightRange, attackRange;
     private Transform targetSoldierLocal;
	private Soldier targetSoldier;
    private Base baseObj;
    private bool playerInSightRange, playerInAttackRange, baseInSightRange, baseInAttackRange, seesBase = false, soldierInAttackRange, soldierInSightRange;

    // Attack variables
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage = 6f;
    private bool canAttack = true;
    private bool canMove = true;
    
    // Enemy types
    [SerializeField] private bool isShaman;
    [SerializeField] private float shahamHealCooldown = 9f, shamanHealAmount = 50f;

    // Animations 
    [SerializeField] Animator anim;
    [SerializeField] float attackAnimBuffer;

    private void Start() 
    {
        // Starts the finding of soldiers
        InvokeRepeating("UpdateTargetSoldier", 0f, 0.3f);

        // If the enemy is a shaman, it begins their "healing spell"
        if(isShaman)
        {
            InvokeRepeating("HealAllies", 0f, shahamHealCooldown);
        }

        // Sets the objects of player, weapon, and base
        player = FindObjectOfType<PlayerController>();
        weapon = FindObjectOfType<WeaponController>();
        baseObj = FindObjectOfType<Base>();
        
        // Sets the base health of enemy
        currentHealth = maxHealth;
        healthbar.updateHealthBar(maxHealth, currentHealth);

        // Sets up AI navigations and gives a semi-random destination in the center of the map
        agent = GetComponent<NavMeshAgent>();
        dest = new Vector3((Random.Range(-6f, 6f)),0,(Random.Range(-6f, 6f)));
        agent.SetDestination(dest);

        // Sets the enemies base speed in case it is modified 
        startSpeed = agent.speed;
    }

    private void Update() 
    {
        // Stops updating if the enemy dies until it is destroyed
        if(!isAlive) {return;}

        // Checks if player, base, and soldiers are within sight and attack ranges
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        baseInSightRange = Physics.CheckSphere(transform.position, sightRange, baseLayer);
        baseInAttackRange = Physics.CheckSphere(transform.position, attackRange, baseLayer);
        soldierInSightRange = Physics.CheckSphere(transform.position, sightRange, soldierLayer);
        soldierInAttackRange = Physics.CheckSphere(transform.position, attackRange, soldierLayer);

        // Hierarchically sets the action of the enemy depending on what is in sight and attack range; prioritizes Soldier->Player->Base for gameplay
        if(((!playerInSightRange && !playerInAttackRange && !baseInSightRange && !baseInAttackRange && !soldierInSightRange && !soldierInAttackRange) || !player.isAlive() || (targetSoldier != null && !targetSoldier.isAlive())) && destinationHasChanged)
        {
            SetDestination();
        }

        if(player.isAlive() && playerInSightRange && !playerInAttackRange && !soldierInSightRange && targetSoldierLocal == null)
        {
            ChasePlayer();
        }
        else if(baseInSightRange && !baseInAttackRange && !playerInAttackRange && !seesBase && targetSoldierLocal == null)
        {
            GoToBase();
        }
        
        if(!canMove)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        if(player.isAlive() && playerInAttackRange && !soldierInAttackRange && canAttack && canMove)
        {
            StartCoroutine(AttackPlayer());
        }
        else if(baseInAttackRange && canAttack && (!playerInSightRange || !player.isAlive()))
        {
            attackBase();
        }
    }

    private void UpdateTargetSoldier()
    {
        // Stops updating if the enemy dies until it is destroyed
        if(!isAlive) {return;}

        // Stores all the soldiers in an array
        GameObject [] soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestSoldier = null;

        // Find the closest soldier
        foreach(GameObject soldier in soldiers)
        {
            float distanceToSoldier = Vector3.Distance(transform.position, soldier.transform.position);

            if(distanceToSoldier < shortestDistance)
            {
                shortestDistance = distanceToSoldier;
                nearestSoldier = soldier;
            }
        }

        // Sets the action for enemy against a soldier based on if they are within attack or sight range
        if(nearestSoldier != null && shortestDistance <= attackRange && targetSoldier.isAlive())
        {
            StartCoroutine(AttackSoldier());
        }

        if(nearestSoldier != null && shortestDistance <= sightRange)
        {
            targetSoldierLocal = nearestSoldier.transform;
            targetSoldier = nearestSoldier.GetComponent<Soldier>();

            if(targetSoldier != null && targetSoldier.isAlive())
            {
                ChaseSoldier();
            }
        }
        else
        {
            targetSoldierLocal = null;
        }
    }

    // Used by the "shaman" enemy type to heal other enemies
    private void HealAllies()
    {
        Collider [] hitObjs = Physics.OverlapSphere(transform.position, sightRange);
         foreach(Collider hitObj in hitObjs)
         {
            if(hitObj.tag == "Enemy")
            {
                getAllyToHeal(hitObj.transform);
            }
         }
    }

    // Heals enemies if within range of a shaman
    private void getAllyToHeal(Transform enemy)
    {
        EnemyController e = enemy.GetComponent<EnemyController>();

        if(e != null)
        {
            e.heal(shamanHealAmount);
        }
    }

    // Heals enemies
    public void heal(float health)
    {
        if(currentHealth + health >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += health;
        }

        healthbar.updateHealthBar(maxHealth, currentHealth);
    }

    private void SetDestination()
    {
        agent.SetDestination(dest);
        destinationHasChanged = false;
        seesBase = false;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.gameObject.transform.position);
        destinationHasChanged = true;
        seesBase = false;
    }

    private void GoToBase()
    {
        seesBase = true;
        agent.SetDestination(new Vector3(0,0,0));
    }

    private void ChaseSoldier()
    {
        agent.SetDestination(targetSoldierLocal.position);
        destinationHasChanged = true;
        seesBase = false;
    }

    private IEnumerator AttackPlayer()
    {
        canAttack = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackAnimBuffer);
        player.takeDamage(damage);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private void attackBase()
    {
        baseObj.takeDamage(livesTaken);

        Destroy(gameObject);
    }

    private IEnumerator AttackSoldier()
    {
        canAttack = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(attackAnimBuffer);
        targetSoldier.takeDamage(damage);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Checks for collision with player weapon
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Weapon")
        {
            takeDamage(weapon.damage);
        }
    }

    // Updates health based on damage source
    public void takeDamage(float _damage)
    {
        currentHealth -= _damage;

        // Starts Die coroutine if the health goes below zero
        if(currentHealth <= 0f && isAlive)
        {
            StartCoroutine("Die");
        }
        else
        {
            healthbar.updateHealthBar(maxHealth, currentHealth);
        }
    }

    // Used to slow down enemies if hit by Sunbeam Laser
    public void SlowDown(float slowPercentage)
    {
        agent.speed = startSpeed * slowPercentage;
        anim.SetBool("isSlowed", true);
        StartCoroutine("ResetSpeed");
    }

    // Used to stun enemies if hit by Tesla
    public void Stun(float _stunTime)
    {
        canMove = false;
        float stunTime = _stunTime;

        if(isAlive)
        {
            anim.SetTrigger("Stun");
        }

        StartCoroutine("ResetStun");
    }

    public float getArmor()
    {
        return armorPecentage;
    }

    public float getMagicRes()
    {
        return magicResistance;
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(6f);
        anim.SetBool("isSlowed", false);
        agent.speed = startSpeed;
    }


    private IEnumerator ResetStun()
    {
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    // Death process for enemies
    private IEnumerator Die()
    {
        isAlive = false;
        healthbar.gameObject.SetActive(false);
        gameObject.tag = "Untagged";
        agent.isStopped = true;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(2.1f);
        Destroy(gameObject);
    }

    // Used to see enemies sight and attack range in the editor
    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
