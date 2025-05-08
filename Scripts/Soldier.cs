using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    // Health variables
    [SerializeField] private float maxHealth = 100f, armorPercentage, respawnTime, healCooldown = 10f;
     [SerializeField] private HealthBar healthbar;
    private float currentHealth;
    // Pathfinding variables
    private bool destinationHasChanged = false;
    private Transform startingPos;
    private NavMeshAgent agent;
    // Enemye detection variables
    private EnemyController target;
    private Transform targetLocal;
    [SerializeField] private LayerMask enemyLayer;
    private bool enemyInSightRange, enemyInAttackRange;
    [SerializeField] private float sightRange, attackRange;
    // Attack variables
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage = 18f;
    private bool canAttack = true;
    [SerializeField] private bool isDowned = false, isHonorGuard = false;
    [SerializeField] Animator anim;
    Rigidbody rb;

    private void Start() 
    {
        // Checks if the soldier is "Honor Guard" and loops their heal effect
        if(isHonorGuard)
        {
            InvokeRepeating("heal", 0f, healCooldown);
        }

        currentHealth = maxHealth;
        healthbar.updateHealthBar(maxHealth, currentHealth);
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        startingPos = gameObject.transform;    
    }

    private void Update() 
    {
        // Stops updating if the soldier is "downed"
        if(isDowned) {return;}

        // Checks if enemies are within sight and attack ranges
        enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, enemyLayer);
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, enemyLayer);

        // Attacks enemy if they are in attack range and attack cooldown has been reset
        if(target != null && enemyInAttackRange && canAttack)
        {
            StartCoroutine(AttackEnemy());
        }

        // Toggles movement on and off depending on if an enemy is seen
        if(!enemyInSightRange)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        // Handles running animations
        if(enemyInSightRange && !enemyInAttackRange)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

    }

    public void SetDestination()
    {
        if(isDowned) {return;}

        if(destinationHasChanged)
        {
            agent.SetDestination(startingPos.position);
            destinationHasChanged = false;
        }
    }

    private IEnumerator AttackEnemy()
    {
        canAttack = false;
        anim.SetTrigger("Attack");
        target.takeDamage(damage);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void SetTargets(Transform _tLocal, EnemyController _t, GameObject [] _e)
    {
        if(isDowned) {return;}

        targetLocal = _tLocal;
        target = _t;

        if(currentHealth > 0)
        {
            agent.SetDestination(targetLocal.position);
            destinationHasChanged = true;
        }    

    }

     // Updates health based on damage source
    public void takeDamage(float _damage)
    {
        if(isDowned) {return;}

        currentHealth -= _damage * (1 - armorPercentage);

        if(currentHealth <= 0f)
        {
            StartCoroutine(Revive());
        }
        else
        {
            healthbar.updateHealthBar(maxHealth, currentHealth);
        }
    }

    // Updates health after being healed
    private void heal()
    {
        if(currentHealth + (currentHealth / 3) >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += currentHealth / 3;
        }

        healthbar.updateHealthBar(maxHealth, currentHealth);
    }

    private IEnumerator Revive()
    {
        isDowned = true;
        anim.SetBool("isDowned", isDowned);
        healthbar.gameObject.SetActive(false);
        agent.isStopped = true;
        yield return new WaitForSeconds(respawnTime);

        healthbar.gameObject.SetActive(true);
        currentHealth = maxHealth;
        healthbar.updateHealthBar(maxHealth, currentHealth);
        agent.isStopped = false;
        isDowned = false;
        anim.SetBool("isDowned", isDowned);
    }

    public bool isAlive()
    {
        return !isDowned;
    }
}
