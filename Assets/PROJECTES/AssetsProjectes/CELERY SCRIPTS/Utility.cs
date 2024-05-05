using UnityEngine;
public enum FoodType
{
    Default,
    Tomato,
    Carrot,
    Leek
}
[System.Serializable]
public class Cooldown
{
    #region Variables

    [SerializeField] public float CooldownTime
    {
        get => cooldownTime;
        private set => cooldownTime = value;
    }
    [SerializeField] private float cooldownTime;
    private float _nextFireTime;

    #endregion

    public bool IsCoolingDown => Time.time < _nextFireTime;
    public void StartCooldown() => _nextFireTime = Time.time + CooldownTime;
}


public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }
    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    private static readonly Object syncRoot = new();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType(typeof(T)) as T;
                        if (instance == null)
                            Debug.LogError(
                                "SingletoneBase<T>: Could not found GameObject of type " + typeof(T).Name);
                    }
                }
            }
            return instance;
        }
    }
}