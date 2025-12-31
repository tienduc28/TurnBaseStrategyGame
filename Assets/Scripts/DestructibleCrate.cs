using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestructibleCrate : MonoBehaviour
{
    [SerializeField] private Transform crateDestroyedPrefab;
    public static event EventHandler OnAnyDestructibleCrateDestroyed;
    private GridPosition gridPosition;

    public void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, Quaternion.identity);

        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);

        Destroy(gameObject);

        OnAnyDestructibleCrateDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
