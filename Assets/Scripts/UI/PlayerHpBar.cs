using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHpBar : MonoBehaviour
{
    public static PlayerHpBar Instance // singleton     
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerHpBar>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerHpBar");
                    instance = instanceContainer.AddComponent<PlayerHpBar>();
                }
            }
            return instance;
        }
    }
    private static PlayerHpBar instance;

    public Transform player;
    public Slider hpBar;
    public float maxHp;
    public float currentHp;

    public TMP_Text playerHpText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        hpBar.value = currentHp / maxHp;
        playerHpText.text = ("" + currentHp);
    }
}
