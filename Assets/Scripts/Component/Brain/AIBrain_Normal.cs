using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain_Normal : AIBrain
{

    public override void BulletApproach(Vector3 pos)
    {
        //нч
    }
    public override void FindPlayer(Transform target)
    {
        if (canThink)
        {
            this.target = target;
            ChangeCondition(AICondition.Attack);
        }
    }
    public override void LostPlayer()
    {
        target = null;
        StopCoroutine("RefreshPlayerPos");
        ChangeCondition(AICondition.Patrol);
    }
    public override void BeAttacked(Vector3 pos)
    {

    }
    IEnumerator OutOfControl(float time)
    {
        canThink = false;
        yield return new WaitForSeconds(time);
        canThink = true;
        switch (condition)
        {
            case AICondition.Attack:
                StartCoroutine("AttackThink");
                break;
            case AICondition.Patrol:
                StartCoroutine("PatrolThink");
                break;
        }
        onStunOver.Invoke();
    }
    public override void Die()
    {
        if (gameObject.activeSelf)
            PoolManager.Instance().Recycle(gameObject);
    }
}
