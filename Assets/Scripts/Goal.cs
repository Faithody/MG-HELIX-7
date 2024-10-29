using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Ensure the GameManager instance exists before calling NextLevel
        if (GameManager.singleton != null)
        {
            GameManager.singleton.NextLevel();
        }
        else
        {
            Debug.LogWarning("GameManager instance is missing. Ensure GameManager.singleton is properly initialized.");
        }
    }
}
