using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCheck : MonoBehaviour
{
    private BallController ballController;

    private void Start()
    {
        // Cache the reference to BallController if it exists in the scene
        ballController = FindObjectOfType<BallController>();
        if (ballController == null)
        {
            Debug.LogWarning("BallController instance not found. Ensure there is a BallController in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check that GameManager and BallController are initialized
        if (GameManager.singleton != null)
        {
            GameManager.singleton.AddScore(2);
        }
        else
        {
            Debug.LogWarning("GameManager instance is missing. Ensure GameManager.singleton is properly initialized.");
        }

        if (ballController != null)
        {
            ballController.perfectPass++;
        }
    }
}
