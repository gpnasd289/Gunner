using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : EnemyMeleeFSM
{
    public GameObject enemyCanvasGo;
    public GameObject meleeAtkArea;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRealizeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }

    void Start()
    {
        base.Start();
        atkCd = 2f;
        atkCdCalc = atkCd;

        atkRange = 3f;
        nvAgent.stoppingDistance = 1f;

        StartCoroutine(ResetAtkArea());
    }

    IEnumerator ResetAtkArea()
    {
        while (true)
        {
            yield return null;
            if (!meleeAtkArea.activeInHierarchy && currentState == State.Attack)
            {
                yield return new WaitForSeconds(atkCd);
                meleeAtkArea.SetActive(true);
            }
        }
    }

    protected override void InitMonster()
    {
        maxHp += (StageMgr.Instance.currentStage + 1) * 100f;
        currentHp = maxHp;
        dmg += (StageMgr.Instance.currentStage + 1) * 10f;
    }

    void Update()
    {
        if (currentHp <= 0)
        {
            nvAgent.isStopped = true;

            rb.gameObject.SetActive(false);
            PlayerTargeting.Instance.MonsterList.Remove(transform.gameObject);
            PlayerTargeting.Instance.targetIndex = -1;
            Destroy(transform.parent.gameObject);
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {
            enemyCanvasGo.GetComponent<EnemyHpBar>().currentHp -= 250f;
            currentHp -= 250f;
        }
    }
}
