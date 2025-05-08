using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private ResourceManager resourceManager;

    [Header("Movement")]
    [SerializeField] Animator anim;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 360;
    private Vector2 _direction;
    private Vector3 _input;
    private Rigidbody rb;
    private Vector3 startingPos;

    [Header("Health")]
    [SerializeField] private HealthBar healthbar;
    [SerializeField] private float maxHealth = 369f, healAmount = 9f, reviveCooldown = 6f;
    private float currentHealth;
    private bool isDowned = false;    

    void Start() 
    {
        resourceManager = ResourceManager.instance;

        startingPos = this.transform.position;
        rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
        healthbar.updateHealthBar(maxHealth, currentHealth);
    }

    private void Update() 
    {
        // Stops updating of the player is "downed"
        if(isDowned) {return;}

        GatherInput(); 
        RunningAnim();
    }

    private void FixedUpdate() 
    {
        // Stops updating of the player is "downed"
        if(isDowned) {return;}

        Look();
        Move();
    }

    // Solution for a bug where the player is forces through the boundaries of the map
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Water")
        {
            Debug.Log("Drown");
            this.transform.position = startingPos;
        }
    }

    private void GatherInput() 
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look() 
    {
        if (_input == Vector3.zero) return;

        // Turns the player
        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
    }

    private void Move() 
    {
        rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * speed * Time.deltaTime);
    }

    private void RunningAnim()
    {
        if(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    // Updates health based on damage source
    public void takeDamage(float _damage)
    {
        currentHealth -= _damage;

        if(currentHealth <= 0)
        {
            StartCoroutine(Revive());
        }
        else
        {
            healthbar.updateHealthBar(maxHealth, currentHealth);
        }
    }

    // Controls the healing button
    public void healHealth()
    {
        if(resourceManager.getFood() >= 1f && currentHealth != maxHealth)
        {
            if(currentHealth + healAmount >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += healAmount;
            }

            healthbar.updateHealthBar(maxHealth, currentHealth);
            resourceManager.setFood(-1f);
        }
    }

    private IEnumerator Revive()
    {
        isDowned = true;
        healthbar.gameObject.SetActive(false);
        anim.SetBool("isDowned", isDowned);
        yield return new WaitForSeconds(reviveCooldown);

        healthbar.gameObject.SetActive(true);
        currentHealth = maxHealth / 2;
        healthbar.updateHealthBar(maxHealth, currentHealth);
        isDowned = false;
        anim.SetBool("isDowned", isDowned);
    }

    public bool isAlive()
    {
        return !isDowned;
    }

    public void buffHealth()
    {
        maxHealth *= 3;
        healthbar.updateHealthBar(maxHealth, currentHealth);
    }

    public void buffSpeed()
    {
        speed = 7.8f;
    }
}

public static class Helpers 
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
