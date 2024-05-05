public interface IDamageable
{
    int CurrentHealth { get; set; }
    void TakeDamage(int damage);
    void Die();
}
public interface IStealFoodType
{
    void StealFoodType(PanController panController);
}
