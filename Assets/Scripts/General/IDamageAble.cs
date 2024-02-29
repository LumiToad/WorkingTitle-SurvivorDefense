using UnityEngine;

public interface IDamageAble
{
    public void TakeDamage(int damage, IDamageSource source);
    public GameObject gameObject { get; }
}

public interface IDamageSource
{
    public string glueSourceName { get; }
    public GameObject gameObject { get; }
}
