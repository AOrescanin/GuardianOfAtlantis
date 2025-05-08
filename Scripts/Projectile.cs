using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 30f, speed = 12f, explosionRadius = 0f;
    [SerializeField] private bool isFMJ, isElectricity, isMagic;
    [SerializeField] private GameObject impactEffect;
    private Transform target;

    private void Update()
    {
        // Destroys projectile of the target is null
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Sets ditection of the projectile towards the target
        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }

        // Movesthe projectile towards the target
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void HitTarget()
    {
        // Determines if projectile is doing AoE damage or single target
        if(explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        // Plays particle effect if ther is one
        if(impactEffect != null)
        {
            GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 3f);
        }

        Destroy(gameObject);
    }

    // Handles AoE projectile damage
    private void Explode()
    {
        Collider [] hitObjs = Physics.OverlapSphere(transform.position, explosionRadius);
         foreach(Collider hitObj in hitObjs)
         {
            if(hitObj.tag == "Enemy")
            {
                Damage(hitObj.transform);
            }
         }
    }

    // Handles damage of enemies and accounts for armor, magic resistance, and if it will stun
    private void Damage(Transform enemy)
    {
        EnemyController e = enemy.GetComponent<EnemyController>();

        if(e != null)
        {
            float armorPercentage = 1 - e.getArmor();
            float magicResPercentage = 1 - e.getMagicRes();
            if(isFMJ)
            {
                e.takeDamage(damage * (armorPercentage / 2));
            }
            else if(isMagic)
            {
                e.takeDamage(damage * magicResPercentage);
                e.SlowDown(.5f);
            }
            else if(isElectricity)
            {
                e.takeDamage(damage * magicResPercentage);
                e.Stun(1f);
            }
            else
            {
                e.takeDamage(damage * armorPercentage);
            }
        }
    }

    // Shows radius of AoE projectiles in editor
    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
