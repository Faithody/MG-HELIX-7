using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPart : MonoBehaviour
{
    [SerializeField] private Color deathPartColor = Color.red; // Allows custom color in the editor

    private void OnEnable()
    {
        // Set the color when enabled
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = deathPartColor;
        }
        else
        {
            Debug.LogWarning("Renderer not found on DeathPart. Ensure there is a Renderer component attached.");
        }
    }

    public void HittedDeathPart()
    {
        // Check that GameManager instance exists before calling RestartLevel
        if (GameManager.singleton != null)
        {
            GameManager.singleton.RestartLevel();
        }
        else
        {
            Debug.LogWarning("GameManager instance is missing. Ensure GameManager.singleton is properly initialized.");
        }
    }
}
