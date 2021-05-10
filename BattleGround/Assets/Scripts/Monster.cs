using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingEntity
{

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().TurnEnd += ChangeTurn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDamaged(int damage, Types type)
    {
        // 사망하지 않은 경우
        if (!dead)
        {
            //피격효과
        }

        base.OnDamaged(damage, type);
    }

    public override void ChangeTurn()
    {
        base.ChangeTurn();
    }
}
