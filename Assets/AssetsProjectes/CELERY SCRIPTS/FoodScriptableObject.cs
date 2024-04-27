using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Food", menuName = "Food")]
public class FoodScriptableObject : ScriptableObject
{
    public FoodType FoodType;

    [Header("Pan Info")]
    public GameObject prefabAssigned;
    public float cookingTime;
    public float spareCookingTime;

    [Header("Skill Info")]
    public Sprite skillSprite;
    public Sprite skillHudSprite;

    [Header("Enemy")]
    public int enemyHealth;

    [Header("Receptari")]
    public Sprite receptariSprite;
}
