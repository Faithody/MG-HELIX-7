using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float impulseForce = 5f;

    private Vector3 startPos;
    public int perfectPass = 0;
    private bool ignoreNextCollision;
    public bool isSuperSpeedActive;

    private void Awake()
    {
        startPos = transform.position;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody not found on BallController. Please attach a Rigidbody.");
            }
        }
    }

    private void Start()
    {
        // Optional: apply an initial impulse to start movement if needed
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (ignoreNextCollision) return;

        if (isSuperSpeedActive)
        {
            if (!other.transform.GetComponent<Goal>())
            {
                Destroy(other.transform.parent.gameObject);
            }
        }
        else
        {
            DeathPart deathPart = other.transform.GetComponent<DeathPart>();
            if (deathPart)
            {
                deathPart.HittedDeathPart();
            }
        }

        // Apply a new upward impulse each collision instead of setting velocity to zero
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        // Safety check to avoid rapid multiple collisions
        ignoreNextCollision = true;
        Invoke("AllowCollision", .2f);

        // Resetting super speed
        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void Update()
    {
        // Activate super speed if perfectPass threshold is reached
        if (perfectPass >= 3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }
}
 