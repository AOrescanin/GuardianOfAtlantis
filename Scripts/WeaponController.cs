using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the players weapon
public class WeaponController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private MeshCollider coll;
    private PlayerController player;
    [SerializeField] private GameObject sword;
    [SerializeField] private float attackCooldown = .33f;
    [SerializeField] public float damage = 25f;
    [SerializeField] private AudioClip [] swingSounds;
    private AudioClip currentClip;
    private AudioSource source;
    private bool canAttack = true;
    StateManager stateManager;

    private void Awake() 
    {
        coll = sword.GetComponent<MeshCollider>();
        player = FindObjectOfType<PlayerController>();
        stateManager = StateManager.instance;
        source = GetComponent<AudioSource>();
    }

    void Update() 
    {   
        if(stateManager.getState() != StateManager.SpawnState.PLAYING || !player.isAlive()) {return;}
        
        if(Input.GetMouseButtonDown(0))
        {
            if(canAttack)
            {
                StartCoroutine(Swing());
            }
        }
    }

    IEnumerator Swing()
    {
        canAttack = false;
        coll.enabled = true;
        anim.SetTrigger("Attack");
        currentClip = swingSounds[Random.Range(0, swingSounds.Length)];
        source.clip = currentClip;
        source.Play();

        yield return new WaitForSeconds(attackCooldown);
        coll.enabled = false;
        canAttack = true;
    }
}
