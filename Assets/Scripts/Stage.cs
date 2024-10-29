using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level
{
    [Tooltip("Number of active parts in this level")]
    [Range(1, 11)]
    public int partCount = 11;

    [Tooltip("Number of parts that will be marked as death parts")]
    [Range(0, 11)]
    public int deathPartCount = 1;
}

[CreateAssetMenu(fileName = "New Stage", menuName = "Game/Stage")]
public class Stage : ScriptableObject
{
    [Tooltip("Background color for this stage")]
    public Color stageBackgroundColor = Color.white;

    [Tooltip("Color of level parts in this stage")]
    public Color stageLevelPartColor = Color.white;

    [Tooltip("Color of the ball in this stage")]
    public Color stageBallColor = Color.white;

    [Tooltip("List of levels in this stage")]
    public List<Level> levels = new List<Level>();
}
