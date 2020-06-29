using Morpeh;
using UnityEngine;

[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WeaponInstanceSystem))]
public class WeaponInstanceSystem : WeaponSystemBase {
    protected override void GetBulletComponents()
    {
        bullet = Instantiate(Bullet);
        
        bulletProvider = bullet.GetComponent<BulletProvider>();
        transformProvider = bullet.GetComponent<TransformProvider>();
        movementProvider = bullet.GetComponent<MovementProvider>();
        collider2DProvider = bullet.GetComponent<Collider2DProvider>();
    }

    protected override void OnBulletCollised()
    {
        bullet.SetActive(false);
    }
}