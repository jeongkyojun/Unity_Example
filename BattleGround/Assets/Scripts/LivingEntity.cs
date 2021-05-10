using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    GameManager GM;

    public float startingHealth;
    public int Ammo { get; protected set; } // 방어도
    public float health { get; protected set; } // 체력

    // 상태이상 - 출혈, 기절, 독, 고통(받는피해 증가), 열정(주는피해 증가)
    #region Bleed
    public bool isBleeding { get; protected set; } // 출혈 여부
    public int bleedingTurn { get; protected set; } // 출혈 턴
    public int bleedingStack { get; protected set; } // 출혈 스택
    #endregion

    #region Poison
    public bool isPoison { get; protected set; } // 독 여부
    public int PoisonTurn { get; protected set; } // 독 지속 턴
    public int PoisonStack { get; protected set; } // 독 스택
    #endregion

    #region Stun
    public bool isStun { get; protected set;} // 기절 여부
    public int StunTurn { get; protected set; } // 기절 턴
    #endregion

    #region Pain
    public bool isPain { get; protected set; } // 고통(받는피해 증가) 여부
    public int PainTurn { get; protected set; } // 고통 지속 턴
    public int PainStack { get; protected set; } // 고통 값
    #endregion

    #region Passion
    public bool isPassion { get; protected set; } // 열정(주는피해 증가) 여부
    public int PassionTurn { get; protected set; }// 열정 지속 턴
    public int PassionStack { get; protected set; }// 열정 값
    #endregion


    public bool dead { get; protected set; } // 사망여부
    public event Action onDeath; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    // 피해를 받는 기능
    public virtual void OnDamaged(int damage, Types type)
    {

        if (isPain)
        {
            // 고통상태인 경우 고통값만큼 데미지 증가
            damage += PainStack;
        }

        // 직접 데미지인 경우 방어도를 무시한다.
        if (type != Types.Direct)
        {
            // 방어력이 존재한다면 방어력 감소
            if (Ammo > 0)
            {
                if (Ammo > damage)
                {
                    Ammo -= damage;
                    damage = 0;
                }
                else
                {
                    damage -= Ammo;
                    Ammo = 0;
                }
            }
        }

        // 방어력 먼저 깎은 뒤, 체력 감소
        health -= damage;

        // 체력이 0 이하인경우 사망처리
        if (health <= 0 && !dead)
        {
            Die();
        }
    }


    // 사망 처리
    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있는경우 실행
        if(onDeath != null)
        {
            onDeath();
        }

        // 사망상태를 참으로 변경
        dead = true;
    }

    // 체력 회복 처리
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return;
        }
        health += newHealth;
    }

    // 방어력 증가 처리
    public virtual void AddAmmo(int AmmoPoint)
    {
        // 죽었을 경우 회복 불가
        if(dead)
        {
            return;
        }
        Ammo += AmmoPoint;
    }

    #region 상태이상 처리
    // 출혈
    public virtual void BleedingSet(int turn, int stack)
    {
        if (!isBleeding)
            isBleeding = true;
        
        // 현재 턴과 지정된 턴 중 큰쪽으로 갱신
        bleedingTurn = (bleedingTurn > turn ? bleedingTurn : turn);

        // 지정된 스택만큼 스택 증가
        bleedingStack += stack;
    }

    // 기절
    public virtual void StunSet(int turn)
    {
        if (!isStun)
            isStun = true;

        // 현재 턴과 지정된 턴 중 큰쪽으로 갱신
        StunTurn = (StunTurn > turn ? StunTurn : turn);
    }

    // 독
    public virtual void PoisonSet(int turn, int stack)
    {
        if (!isPoison)
            isPoison = true;

        // 현재 턴과 지정된 턴 중 큰쪽으로 갱신
        PoisonTurn = (PoisonTurn > turn ? PoisonTurn : turn);

        // 지정된 스택만큼 스택 증가
        PoisonStack += stack;
    }

    // 고통
    public virtual void PainSet(int turn, int stack)
    {

        if (!isPain)
            isPain = true;

        // 현재 턴과 지정된 턴 중 큰쪽으로 갱신
        PainTurn = (PainTurn > turn ? PainTurn : turn);

        // 지정된 스택만큼 스택 증가
        PainStack += stack;
    }

    // 열정
    public virtual void PassionSet(int turn, int stack)
    {
        if (!isPassion)
            isPassion = true;

        // 현재 턴과 지정된 턴 중 큰쪽으로 갱신
        PassionTurn = (PassionTurn > turn ? PassionTurn : turn);

        // 지정된 스택만큼 스택 증가
        PassionStack += stack;
    }
    #endregion

    // 턴 넘김
    public virtual void ChangeTurn()
    {
        // 출혈 상태인 경우 출혈 턴 감소, 피해 입음(미구현)
        if(isBleeding)
        {
            // 출혈 상태에 따른 피해를 입는다.(미구현)

            // 출혈 턴을 하나 줄인다. 턴이 0인경우 출혈상태를 해제한다.
            bleedingTurn--;
            if (bleedingTurn <= 0)
            {
                bleedingTurn = 0;
                bleedingStack = 0;
                isBleeding = false;
            }
        }

        // 독 상태인 경우 독 턴 감소, 피해 입음(미구현)
        if(isPoison)
        {
            // 독 상태에 따른 피해를 입는다. (미구현)

            // 독 턴을 하나 줄인다. 턴이 0인경우 독을 해제한다.
            PoisonTurn--;
            if (PoisonTurn <= 0)
            {
                PoisonTurn = 0;
                PoisonStack = 0;
                isPoison = false;
            }
        }

        // 기절 상태인 경우 기절 턴 감소
        if(isStun)
        {
            StunTurn--;
            if (StunTurn <= 0)
            {
                StunTurn = 0;
                isStun = false;
            }
        }
        // 고통 상태인 경우 고통 턴 감소
        if(isPain)
        {
            PainTurn--;
            if (PainTurn <= 0)
            {
                PainTurn = 0;
                PainStack = 0;
                isPain = false;
            }
        }
        // 열정 상태인 경우 열정 턴 감소
        if (isPassion)
        {
            PassionTurn--;
            if (PassionTurn <= 0)
            {
                PassionTurn = 0;
                PassionStack = 0;
                isPassion = false;
            }
        }
    }
}
