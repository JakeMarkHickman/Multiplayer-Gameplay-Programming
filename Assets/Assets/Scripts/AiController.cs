using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class AiController : NetworkBehaviour
{
    [SerializeField] Movement moveScript;
    [SerializeField] MainHand hand;
    
    [SerializeField] string TargetTag;
    [SerializeField] float SearchDistance = 20.0f;
    [SerializeField] private float AttackDistance = 3.0f;
    [SerializeField] private float AttackCooldown = 5.0f;

    [SerializeField] private NetworkObject Target;

    private Damage damageComp;
    private Coroutine c_Attack;
    private bool canAttack = true;

    private void OnEnable()
    {
        damageComp = gameObject.GetComponent<Damage>();
    }


    private void FixedUpdate()
    {
        if (!IsServer)
            return;

        SetTarget(FindClosestTarget());
        
        if (!Target || !(GetDistanceToObject(Target.gameObject) <= AttackDistance))
        {
            MoveTowardsTarget();
        }

        if (damageComp && canAttack)
        {
            c_Attack = StartCoroutine(AttackTarget());
        }
    }

    private IEnumerator AttackTarget()
    {
        if (Target && GetDistanceToObject(Target.gameObject) <= AttackDistance)
        {
            if (Target.TryGetComponent<Health>(out Health healthComp))
            {
                canAttack = false;
                healthComp.TakeDamageRPC(damageComp.GetDamageType(), damageComp.GetDamage(), gameObject.name);

                yield return new WaitForSecondsRealtime(AttackCooldown);

                canAttack = true;
            }
        }
        
        yield return null;
    }
    
    private void MoveTowardsTarget()
    {
        if (!moveScript)
            return;

        if (!Target)
            return;

        moveScript.Move((Target.transform.position - gameObject.transform.position).normalized);
    }

    private void SetTarget(NetworkObject value)
    {
        Target = value;
    }

    private NetworkObject FindClosestTarget()
    {
        NetworkObject curTarget = null;
        GameObject[] targets = GameObject.FindGameObjectsWithTag(TargetTag);
        GameObject closest = null;

        if (targets.Length <= 0)
            return null;

        foreach(GameObject cur in targets)
        {
            if (!cur.GetComponent<NetworkObject>())
                continue;

            if(!closest)
                closest = cur;

            float closestEntDis = Vector3.Distance(closest.gameObject.transform.position, gameObject.transform.position);

            float closestDistance = SearchDistance < closestEntDis ? SearchDistance : closestEntDis;
            float distance = GetDistanceToObject(cur);

            if (closestDistance < distance)
                continue;

            closest = cur;
            curTarget = cur.GetComponent<NetworkObject>();
        }

        return curTarget;
    }

    private float GetDistanceToObject(GameObject ObjectToTrack)
    {
        return Vector3.Distance(ObjectToTrack.gameObject.transform.position, gameObject.transform.position);
    }
}
