using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Lives
{
    public float Atk; // 공격력
    public int Shield; // 방어도
    public int Defend; // 방어력(피해감소)
    public int unDefend; // 피해증폭
    public int Health; // 체력

    public int MovePower; // 이동력
}

public class LivingEntity : MonoBehaviour
{
    public GameManager GM;
    public Lives EntityInfo;
    public bool playAble; // true일때만 이동이 가능해진다.
    public bool isDead; // 사망여부 확인
    // Start is called before the first frame update
    void Start()
    {
        playAble = false;
        isDead = false;
        GM = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDamaged(int Damage, int type)
    {
        if(EntityInfo.Shield>0)
        {
            // 방어도가 존재 시 데미지를 방어도만큼 차감
            Damage -= EntityInfo.Shield;
            if (Damage < 0)
                Damage = 0;

            // 방어도 또한 받은 데미지 만큼 차감
            EntityInfo.Shield -= Damage;
            if (EntityInfo.Shield < 0)
                EntityInfo.Shield = 0;
        }
        // 그 뒤 차감된 데미지 만큼 체력에 피해를 준다.
        EntityInfo.Health -= Damage;
        if(EntityInfo.Health<0)
        {
            // 죽음

            // 속도를 -1로 변경해 최하단으로 변환
            GM.Entities[GM.Turn].Speed = -1;
        }
    }
}
