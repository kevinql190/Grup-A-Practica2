using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ReceptariInfo
{
    public FoodScriptableObject FoodType;
    public bool found;
}
[Serializable]
public class LevelInfo
{
    public bool unlocked;
    public string levelName;
    public string levelScene;
}
public class GameManager : SingletonPersistent<GameManager>
{
    public ReceptariInfo[] receptariInfo;
    public LevelInfo[] levels;
}
