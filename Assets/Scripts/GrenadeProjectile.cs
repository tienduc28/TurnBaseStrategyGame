using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;
    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float moveSpeed = 15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance)
        {
            Debug.Log(transform.position + " reached target " + targetPosition);
            float damageRadius = 3f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        }
    }


    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }

}
