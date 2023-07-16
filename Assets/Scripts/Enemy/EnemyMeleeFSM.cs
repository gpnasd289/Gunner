using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeFSM : EnemyBase
{
    // Start is called before the first frame update
    public enum State
    {
        Idle,
        Move,
        Attack,
    };

    public State currentState = State.Idle;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    protected void Start()
    {
        base.Start();
        parentRoom = transform.parent.transform.parent.gameObject;
        Debug.Log("Start - State :" + currentState.ToString());

        StartCoroutine(FSM());
    }
    protected virtual void InitMonster() { }

    protected virtual IEnumerator FSM()
    {
        yield return null;

        while (!parentRoom.GetComponent<RoomCondition>().playerInThisRoom)
        {
            yield return Delay500;
        }

        InitMonster();

        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsTag("IDLE"))
        {
            Anim.SetTrigger("Idle");
        }

        if (CanAtkStateFun())
        {
            if (canAtk)
            {
                currentState = State.Attack;
            }
            else
            {
                currentState = State.Idle;
                transform.LookAt(Player.transform.position);
            }
        }
        else
        {
            currentState = State.Move;
        }
    }

    protected virtual void AtkEffect() { }

    protected virtual IEnumerator Attack()
    {
        yield return null;
        //Atk
        nvAgent.stoppingDistance = 0f;
        nvAgent.isStopped = true;
        nvAgent.SetDestination(Player.transform.position);
        yield return Delay500;

        nvAgent.isStopped = false;
        nvAgent.speed = 30f;
        canAtk = false;

        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("IDLE"))
        {
            Anim.SetTrigger("Attack");
        }
        //AtkEffect();
        yield return Delay500;

        nvAgent.speed = moveSpd;
        nvAgent.stoppingDistance = atkRange;
        currentState = State.Idle;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
        //Move
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("RUN"))
        {
            Anim.SetTrigger("Run");
        }
        if (CanAtkStateFun() && canAtk)
        {
            currentState = State.Attack;
        }
        else if (distance > playerRealizeRange)
        {
            nvAgent.SetDestination(transform.parent.position - Vector3.forward * 5f);
        }
        else
        {
            nvAgent.SetDestination(Player.transform.position);
        }
    }
}
