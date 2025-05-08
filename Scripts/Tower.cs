using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Tower : MonoBehaviour
{
    private BuildManager buildManager;
    private ResourceManager resourceManager;
    [SerializeField] private float attackRange = 9; 
    [SerializeField] private float attackCooldown = 1;
    private float rotationSpeed = 6f;
    [SerializeField] private float damageOverTime = 30f;
    private bool canAttack = true;
    [SerializeField]  private GameObject projectilePrefab;
    [SerializeField]  private int upgradeLvl = 0;
    private Transform target;
	private EnemyController targetEnemy;

    [Header("General")]
    [SerializeField] private bool isArcher = false;
    [SerializeField] private bool isCannon = false;
    [SerializeField] private bool isLaser = false;
    [SerializeField] private bool isTesla = false;
    [SerializeField] private bool isBarracks = false;
    [SerializeField] private AudioClip [] audioClips;
    [SerializeField] private AudioClip upgradeSound;
    [SerializeField] private AudioSource laserUpgradeSound;
    private AudioSource source;
    private AudioClip currentClip;
    [SerializeField] ParticleSystem shootParticle, upgradeParticle;
    private Transform firePoint;

    [Header("Upgrades")]
    [SerializeField] private BoxCollider [] boxColliders;
    [SerializeField] private Transform [] firePoints;
    [SerializeField] private GameObject [] towerPrefabs;
    [SerializeField] private GameObject [] projectilePrefabs;
    [SerializeField] private float [] attackRanges; 
    [SerializeField] private float [] attackCooldowns;
    [SerializeField] private int [] woodCosts;
    [SerializeField] private int [] atlCosts;
    [SerializeField] private int [] foodCosts;
    [SerializeField] private int [] sellWoodCosts;
    [SerializeField] private int [] sellAtlCosts;

    [Header("Cannon")]
    [SerializeField] private Transform secondFirePoint;
    private int counter = 0;

    [Header("Energy Beam")]
    [SerializeField] private float [] dmgOT;
    [SerializeField] private LineRenderer lineRend;
    [SerializeField] private bool isSunburst;
    [SerializeField] private GameObject sunburstProjectile;
    [SerializeField] private float sunburstCooldown;
    [SerializeField] private float sunburstRange;
    [SerializeField] private AudioClip laserSound;

    [Header("Tesla Coil")]
    [SerializeField] private ParticleSystem [] electricities;
    private ParticleSystem electricity;

    [Header("Barracks")]
    [SerializeField] private GameObject [] soldierUpgrades;
    [SerializeField] private Soldier [] soldiers;
    private GameObject currentSoldiers;

    private void Start()
    {
        // Starts the loop of scanning for targets
        InvokeRepeating("UpdateTarget", 0f, 0.3f);

        buildManager = BuildManager.instance;
        resourceManager = ResourceManager.instance;
        source = GetComponent<AudioSource>();

    // Sets the stats and properties of the tower depending on its type

        if(!isLaser && !isBarracks && projectilePrefabs != null)
        {
            projectilePrefab = projectilePrefabs[upgradeLvl];
        }

        if(!isBarracks && attackRanges != null && firePoints != null)
        {
            attackRange = attackRanges[upgradeLvl];
            firePoint = firePoints[upgradeLvl];
        }

        if(!isLaser && !isBarracks && attackCooldowns != null)
        {
            attackCooldown = attackCooldowns[upgradeLvl];
        }

        if(isLaser && dmgOT != null)
        {
            damageOverTime = dmgOT[upgradeLvl];
        }

        if(isTesla && electricities != null)
        {
            electricity = electricities[upgradeLvl];
        }

        if(isBarracks && soldierUpgrades != null)
        {
            currentSoldiers = soldierUpgrades[upgradeLvl];
        }     
    }

    private void Update()
    {   
        // Handles laser beam rendering
        if(target == null) 
        {
            if(isLaser && !isSunburst)
            {
                if(lineRend.enabled)
                {
                    lineRend.enabled = false;
                }

                source.loop = false;
                source.enabled = false;
            }

            return;
        }

        // rotates the cannons and lasers to face the targets
        if(isCannon || isLaser)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        // Handles the type of attack each tower can do
        if(isLaser && !isSunburst)
        {
            EnergyBeam();
            currentClip = laserSound;
            source.clip = currentClip;
            source.loop = true;
            source.enabled = true;
        }
        else if(canAttack && isCannon && upgradeLvl == 4)
        {
            StartCoroutine(DoubleShot());
        }
        else if(canAttack && !isBarracks)
        {
            StartCoroutine(Shoot());
        }
    }

    private void UpdateTarget()
    {
        // Stores all enemeis in an array
        GameObject [] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // Finds the nearest enemy
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // Handles enemy detections for the barracks
        if(nearestEnemy != null && shortestDistance <= attackRange)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<EnemyController>();

            if(isBarracks)
            {
                for(int i = (upgradeLvl * 3); i < ((upgradeLvl * 3) + 3); i++)
                {
                    if(soldiers[i] != null)
                    {
                        soldiers[i].SetTargets(target, targetEnemy, enemies);
                    }
                }
            }
        }
        else
        {
            target = null;

            if(isBarracks)
            {
                for(int i = (upgradeLvl * 3); i < ((upgradeLvl * 3) + 3); i++)
                {
                    if(soldiers[i] != null)
                    {
                        soldiers[i].SetDestination();
                    }
                }
            }
        }
    }

    // Shoots laser
    private void EnergyBeam()
    {
        float magicResistancePercentage = 1 - targetEnemy.getMagicRes();
        targetEnemy.takeDamage((damageOverTime * Time.deltaTime) * magicResistancePercentage);
        targetEnemy.SlowDown(.5f);

        if(!lineRend.enabled)
        {
            lineRend.enabled = true;
        }

        lineRend.SetPosition(0, firePoint.position);
        lineRend.SetPosition(1, target.position);
    }

    // Shoots tower's projectile
    private IEnumerator Shoot()
    {
        canAttack = false;

        GameObject projectileGO = (GameObject)Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile _projectile = projectileGO.GetComponent<Projectile>();

        // Handles how a Tesla works vs other towers
        if(isTesla && _projectile != null)
        {
            _projectile.Seek(gameObject.transform);
            electricity.Play();
        }
        else if(_projectile != null)
        {
            _projectile.Seek(target);
        }

        // SFX
        currentClip = audioClips[Random.Range(0, audioClips.Length)];
        source.clip = currentClip;
        source.Play();

        // Particles
        if(shootParticle != null)
        {
            shootParticle.transform.position = firePoint.transform.position;
            shootParticle.Play();
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Handles how a max level Cannon shoots twice by alternating the shots and particle effects
    private IEnumerator DoubleShot()
    {
        canAttack = false;

        if(counter % 2 == 0)
        {
            GameObject projectileGO = (GameObject)Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Projectile _projectile = projectileGO.GetComponent<Projectile>();

            if(_projectile != null)
            {
                _projectile.Seek(target);
        
            }

            if(shootParticle != null)
            {
                shootParticle.transform.position = firePoint.transform.position;
                shootParticle.Play();
            }
        }
        else
        {
            GameObject projectileGO = (GameObject)Instantiate(projectilePrefab, secondFirePoint.position, secondFirePoint.rotation);
            Projectile _projectile = projectileGO.GetComponent<Projectile>();

            if(_projectile != null)
            {
                _projectile.Seek(target);
            }

            if(shootParticle != null)
            {
                shootParticle.transform.position = secondFirePoint.transform.position;
                shootParticle.Play();
            }
        }

        counter ++;

        // SFX
        currentClip = audioClips[Random.Range(0, audioClips.Length)];
        source.clip = currentClip;
        source.Play();

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnMouseDown() 
    {
        buildManager.TowerSelected(this);
    }

    // Upgrades all necessary stats and properties when player upgrades a tower
    public void upgrade(bool isOther)
    {
        boxColliders[upgradeLvl].enabled = false;
        towerPrefabs[upgradeLvl].SetActive(false);

        upgradeLvl++;

        if(isOther)
        {
            upgradeLvl++;
            
            if(isLaser)
            {
                isSunburst = true;
                projectilePrefab = sunburstProjectile;
                attackRange = sunburstRange;
                attackCooldown = sunburstCooldown;
                source.playOnAwake = false;
                source.enabled = true;
            }
        }

        boxColliders[upgradeLvl].enabled = true;
        towerPrefabs[upgradeLvl].SetActive(true);

        if(!isBarracks)
        {
            firePoint = firePoints[upgradeLvl];

            if(!isSunburst)
            {
                attackRange = attackRanges[upgradeLvl];

                if(!isLaser)
                {
                    projectilePrefab = projectilePrefabs[upgradeLvl];
                    attackCooldown = attackCooldowns[upgradeLvl];
                }
                else
                {
                    damageOverTime = dmgOT[upgradeLvl];
                }

                if(isTesla)
                {
                    electricity = electricities[upgradeLvl];
                }
            }
        }
        else if(isBarracks)
        {
            currentSoldiers.SetActive(false);
            currentSoldiers = soldierUpgrades[upgradeLvl];
            currentSoldiers.SetActive(true);
        }

        resourceManager.setWood(-getWoodCost());
        resourceManager.setAtlantium(-getAtlantiumCost());
        resourceManager.setFood(-getFoodCost());

        upgradeParticle.Play();

        if(!isLaser)
        {
            source.enabled = true;
            source.clip = upgradeSound;
            source.Play();
        }
        else
        {
            laserUpgradeSound.Play();
        }
    }

    public int getType()
    {
        if(isArcher)
        {
            return 1;
        }
        else if(isCannon)
        {
            return 2;
        }
        else if (isLaser)
        {
            return 3;
        }
        else if(isTesla)
        {
            return 4;
        }
        else if(isBarracks)
        {
            return 5;
        }
        
        return 0;
    }

    public float getRange()
    {
        return attackRange;
    }

    public int getUpgradeLvl()
    {
        return upgradeLvl;
    }

    public int getWoodCost()
    {
        return woodCosts[upgradeLvl];
    }

    public int getAtlantiumCost()
    {
        return atlCosts[upgradeLvl];
    }

    public int getFoodCost()
    {
        return foodCosts[upgradeLvl];
    }

    public int getSellWoodCost()
    {
        return sellWoodCosts[upgradeLvl];
    }

    public int getSellAtlCost()
    {
        return sellAtlCosts[upgradeLvl];
    }

    public void Deconstruct()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}