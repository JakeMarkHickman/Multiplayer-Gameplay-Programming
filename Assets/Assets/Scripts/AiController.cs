using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class AiController : NetworkBehaviour
{
    [SerializeField] Movement moveScript;
    [SerializeField] MainHand hand;
    [SerializeField] string TargetTag;
    [SerializeField] float SearchDistance = 20.0f;

    [SerializeField] private NetworkObject Target;

    private void FixedUpdate()
    {
        if (!IsServer)
            return;


        SetTarget(FindClosestTarget());

        MoveTowardsTarget();
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
            float distance = Vector3.Distance(cur.gameObject.transform.position, gameObject.transform.position);

            if (closestDistance < distance)
                continue;

            closest = cur;
            curTarget = cur.GetComponent<NetworkObject>();
        }

        return curTarget;
    }


}
