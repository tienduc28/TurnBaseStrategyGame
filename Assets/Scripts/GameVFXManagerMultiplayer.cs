using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameVFXManager : NetworkBehaviour
{
    #region Singleton
    public static GameVFXManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GameVFXManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    const int poolSize = 5;

    [SerializeField] private Transform bulletProjectilePrefab;
    private List<Transform> bulletProjectilesPool;

    [SerializeField] private Transform bulletHitVfxPrefab;
    private List<Transform> bulletHitVfxPool;

    private void Start()
    {
        bulletProjectilesPool = new List<Transform>();
        bulletHitVfxPool = new List<Transform>();

        for (int i = 0; i < poolSize; i++)
        {
            Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab);
            bulletProjectileTransform.gameObject.SetActive(false);
            bulletProjectilesPool.Add(bulletProjectileTransform);

            Transform bulletHitVfxTransform = Instantiate(bulletHitVfxPrefab);
            bulletHitVfxTransform.gameObject.SetActive(false);
            bulletHitVfxPool.Add(bulletHitVfxTransform);
        }
    }

    public virtual Transform SpawnBulletProjectile(Vector3 shootPosition, Vector3 targetUnitShootAtPosition)
    {
        Transform bulletProjectileTransform = null;
        for (int i = 0; i < bulletProjectilesPool.Count; i++)
        {
            if (!bulletProjectilesPool[i].gameObject.activeSelf)
            {
                bulletProjectileTransform = bulletProjectilesPool[i];
                bulletProjectileTransform.gameObject.SetActive(true);
                break;
            }
        }

        if (bulletProjectileTransform == null)
        {
            bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPosition, Quaternion.identity);
        }

        bulletProjectileTransform.position = shootPosition;

        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
        bulletProjectile.PlayTrail();

        targetUnitShootAtPosition.y = shootPosition.y;
        bulletProjectile.Setup(targetUnitShootAtPosition);

        return bulletProjectileTransform;
    }

    public virtual Transform SpawnHitVFX(Vector3 position)
    {
        Transform hitVFX = null;
        for (int i = 0; i < bulletHitVfxPool.Count; i++)
        {
            if (!bulletHitVfxPool[i].gameObject.activeSelf)
            {
                hitVFX = bulletHitVfxPool[i];
                hitVFX.gameObject.SetActive(true);
                hitVFX.position = position;
                break;
            }
        }

        if (bulletHitVfxPool == null)
        {
            hitVFX = Instantiate(bulletHitVfxPrefab, position, Quaternion.identity);
        }

        ParticleSystem particleSystem = hitVFX.GetComponent<ParticleSystem>();
        particleSystem.Clear();
        particleSystem.Play();

        return hitVFX;
    }
}
