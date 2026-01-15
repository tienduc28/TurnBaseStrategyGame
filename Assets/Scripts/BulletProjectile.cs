using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;

    private Vector3 targetPosition;
    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;

            ResetTrail();

            gameObject.SetActive(false);

            GameVFXManager.Instance.SpawnHitVFX(targetPosition);
        }

    }

    public void ResetTrail()
    {
        trailRenderer.emitting = false;
        trailRenderer.Clear();
    }

    public void PlayTrail()
    {
        trailRenderer.Clear();
        trailRenderer.emitting = true;
    }
}
