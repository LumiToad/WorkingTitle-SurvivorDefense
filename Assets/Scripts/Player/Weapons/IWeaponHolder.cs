public interface IWeaponHolder
{
    public void RemoveWeapon(AbstractWeapon weapon);

    public bool TryAddWeapon(AbstractWeapon weapon);
}
