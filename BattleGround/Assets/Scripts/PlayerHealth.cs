using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider; // 체력 표시하는 UI 슬라이더
    private PlayerMoveMent playerMovement; // 플레이어 움직임 컴포넌트

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMoveMent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().TurnEnd += ChangeTurn;
    }

    // 상태 초기화
    protected override void OnEnable()
    {
        base.OnEnable();


        // 체력 슬라이더(미구현)
        // 체력 슬라이더 활성화 
        healthSlider.gameObject.SetActive(true);
        // 체력 슬라이더의 값을 현재 체력값으로 변경
        healthSlider.maxValue = startingHealth;
        // 체력 슬라이더의 값을 현재 체력값으로 변경
        healthSlider.value = health;

        //플레이어 조작 받는 컴포넌트 비활성화(본인 턴일때만 활성화된다.
        playerMovement.enabled = false;
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);

        // 체력 바 초기화
        healthSlider.value = health;
    }

    // 방어도 증가
    public override void AddAmmo(int AmmoPoint)
    {
        base.AddAmmo(AmmoPoint);
    }

    // 데미지 입음
    public override void OnDamaged(int damage, Types type)
    {
        // 사망하지 않은 경우에
        if(!dead)
        {
            //피격 효과음 재생 등을 수행
        }

        base.OnDamaged(damage, type);

        //체력 바 갱신
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die()
    {
        // 사망 적용
        base.Die();

        // 체력 슬라이더 비활성화
        healthSlider.gameObject.SetActive(false);

        // 컴포넌트 비활성화
        playerMovement.enabled = false;
    }

    public override void ChangeTurn()
    {
        base.ChangeTurn();
    }
}
