public class WeaponDatabase {
    public enum BulletCaliber
    {
        NoBullet, Gauge12, Rifle556, Lapua, Pistol9mm
    }
    public enum BulletMod
    {
        None, Explosive
    }
}
public struct Bullet
{
    public WeaponDatabase.BulletCaliber bulletCaliber;
    public WeaponDatabase.BulletMod bulletMod;

    public Bullet(WeaponDatabase.BulletCaliber newBulletCaliber = WeaponDatabase.BulletCaliber.NoBullet, WeaponDatabase.BulletMod newBulletMod = WeaponDatabase.BulletMod.None)
    {
        bulletCaliber = newBulletCaliber;
        bulletMod = newBulletMod;
    }
}