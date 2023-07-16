using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Transform enemy;
    public Slider hpBar;
    public float maxHp;
    public float currentHp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = enemy.position;
        hpBar.value = currentHp / maxHp;
    }
}
