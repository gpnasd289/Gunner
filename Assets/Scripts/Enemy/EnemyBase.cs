using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHp = 1000f;
    public float currentHp = 1000f;

    public float dmg = 100f;

    protected float playerRealizeRange = 10f;
    protected float atkRange = 5f;
    protected float atkCd = 5f;
    protected float atkCdCalc = 5f;
    protected bool canAtk = true;

    protected float moveSpd = 2f;

    protected GameObject Player;
    protected NavMeshAgent nvAgent;

    protected float distance;

    protected GameObject parentRoom;

    protected Animator Anim;
    protected Rigidbody rb;

    public LayerMask layerMask;
    protected void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player : " + Player);
        Debug.Log("Player.transform.position : " + Player.transform.position);

        nvAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();

        parentRoom = transform.parent.transform.parent.gameObject;

        StartCoroutine(CalcCoolTime());
    }

    protected bool CanAtkStateFun()
    {
        Vector3 targetDir = new Vector3(Player.transform.position.x - transform.position.x, 0f, Player.transform.position.z - transform.position.z);

        Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z), targetDir, out RaycastHit hit, 30f, layerMask);
        distance = Vector3.Distance(Player.transform.position, transform.position);

        if (hit.transform == null)
        {
            Debug.Log(" hit.transform == null");
            return false;
        }

        if (hit.transform.CompareTag("Player") && distance <= atkRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual IEnumerator CalcCoolTime()
    {
        while (true)
        {
            yield return null;
            if (!canAtk)
            {
                atkCdCalc -= Time.deltaTime;
                if (atkCdCalc <= 0)
                {
                    atkCdCalc = atkCd;
                    canAtk = true;
                }
            }
        }
    }
}
