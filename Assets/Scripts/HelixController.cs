using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour
{
    private Vector3 startRotation;
    private Vector2 lastTapPos;
    private float helixDistance;
    public Transform topTransform;
    public Transform goalTransform;
    public GameObject helixLevelPrefab;
    public List<Stage> allStages = new List<Stage>();
    private List<GameObject> spawnedLevels = new List<GameObject>();

    private void Start()
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + 0.1f);
        LoadStage(0);
    }

    private void Update()
    {
        // Check for drag input to rotate the helix
        if (Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;

            if (lastTapPos == Vector2.zero)
                lastTapPos = curTapPos;

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            // Rotate based on delta
            transform.Rotate(Vector3.up * delta);
        }

        // Reset lastTapPos when finger/mouse is lifted
        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage(int stageNumber)
    {
        // Get the correct stage and validate
        if (stageNumber < 0 || stageNumber >= allStages.Count)
        {
            Debug.LogError($"Stage {stageNumber} not found. Please check the allStages list.");
            return;
        }

        Stage stage = allStages[stageNumber];

        // Set the background color if Camera.main exists
        if (Camera.main != null)
            Camera.main.backgroundColor = stage.stageBackgroundColor;

        // Set the ball's color if the BallController exists
        BallController ballController = FindObjectOfType<BallController>();
        if (ballController != null)
            ballController.GetComponent<Renderer>().material.color = stage.stageBallColor;

        // Reset the helix rotation
        transform.localEulerAngles = startRotation;

        // Destroy old levels
        foreach (GameObject level in spawnedLevels)
            Destroy(level);
        spawnedLevels.Clear();

        // Calculate the distance between levels
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        // Spawn new levels
        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;
            GameObject level = Instantiate(helixLevelPrefab, transform);
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);

            // Disable parts based on the setup
            int partsToDisable = 12 - stage.levels[i].partCount;
            List<GameObject> disabledParts = new List<GameObject>();

            while (disabledParts.Count < partsToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                }
            }

            // Color active parts and mark death parts
            List<GameObject> leftParts = new List<GameObject>();
            foreach (Transform t in level.transform)
            {
                if (t.gameObject.activeInHierarchy)
                {
                    t.GetComponent<Renderer>().material.color = stage.stageLevelPartColor;
                    leftParts.Add(t.gameObject);
                }
            }

            // Mark parts as death parts
            List<GameObject> deathParts = new List<GameObject>();
            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[Random.Range(0, leftParts.Count)];
                if (!deathParts.Contains(randomPart))
                {
                    randomPart.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                }
            }
        }
    }
}
