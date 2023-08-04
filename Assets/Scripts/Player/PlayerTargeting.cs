using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public static PlayerTargeting Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerTargeting>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("PlayerTargeting");
                    instance = instanceContainer.AddComponent<PlayerTargeting>();
                }
            }
            return instance;
        }
    }
    private static PlayerTargeting instance;

    public bool getATarget = false;
    float currentDist = 0;
    float closestDist = 100f;
    float targetDist = 100f;
    int closestDistIndex = 0;
    public int targetIndex = -1;
    public int prevTargetIndex = 0;

    public LayerMask layerMask;
    public List<GameObject> MonsterList = new List<GameObject>();

    //public GameObject PlayerBolt;
    public Transform AttackPoint;

    public float atkSpd = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();
        AtkTarget();
    }
    private void OnDrawGizmos()
    {
        if (getATarget)
        {
            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { 
                    return; 
                }

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position,
                                            out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawRay(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position); 
            }
        }
    }
    void Attack()
    {
        PlayerMovement.Instance.Anim.SetFloat("AttackSpeed", atkSpd);
        Instantiate(PlayerData.Instance.PlayerBolt, AttackPoint.position, transform.rotation);
    }
    void SetTarget()
    {
        if (MonsterList.Count != 0)
        {
            prevTargetIndex = targetIndex;
            currentDist = 0f;
            closestDistIndex = 0;
            targetIndex = -1;

            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { return; }   
                currentDist = Vector3.Distance(transform.position, MonsterList[i].transform.GetChild(0).position); 

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position,
                                            out hit, 20f, layerMask);

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    if (targetDist >= currentDist)
                    {
                        targetIndex = i;

                        targetDist = currentDist;

                        if (!JoyStickMovement.Instance.isPlayerMoving && prevTargetIndex != targetIndex) 
                        {
                            targetIndex = prevTargetIndex;
                        }
                    }
                }

                if (closestDist >= currentDist)
                {
                    closestDistIndex = i;
                    closestDist = currentDist;
                }
            }

            if (targetIndex == -1)
            {
                targetIndex = closestDistIndex;
            }

            closestDist = 100f;
            targetDist = 100f;
            getATarget = true;
        }

    }
    void AtkTarget()
    {
        if (targetIndex == -1 || MonsterList.Count == 0)  
        {
            PlayerMovement.Instance.Anim.SetBool("Attack", false);
            return;
        }
        if (getATarget && !JoyStickMovement.Instance.isPlayerMoving && MonsterList.Count != 0)
        {
            transform.LookAt(MonsterList[targetIndex].transform.GetChild(0));
            
            if (PlayerMovement.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            {
                PlayerMovement.Instance.Anim.SetBool("Idle", false);
                PlayerMovement.Instance.Anim.SetBool("Walk", false);
                PlayerMovement.Instance.Anim.SetBool("Attack", true);
            }
        }
        else if (JoyStickMovement.Instance.isPlayerMoving)
        {
            if (!PlayerMovement.Instance.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Walk"))
            {
                PlayerMovement.Instance.Anim.SetBool("Attack", false);
                PlayerMovement.Instance.Anim.SetBool("Idle", false);
                PlayerMovement.Instance.Anim.SetBool("Walk", true);
            }
        }
        else 
        {
            PlayerMovement.Instance.Anim.SetBool("Attack", false);
            PlayerMovement.Instance.Anim.SetBool("Idle", true);
            PlayerMovement.Instance.Anim.SetBool("Walk", false);
        }
    }
}
