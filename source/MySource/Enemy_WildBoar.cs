using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̎q�N���X
/// �C�m�V�V�^�̓G�œːi�U�������Ă���
/// </summary>
public class Enemy_WildBoar : Enemy
{
    private readonly StateCharge s_stateCharge = new StateCharge();

    /// <summary>
    /// �ːi�U���̏������s���X�e�[�g
    /// </summary>
    public class StateCharge : EnemyStateBase
    {
        float chargeTime = 0;
        float atkTime = 0;
        float rigidityTime = 0;
        float vx = 0;
        float vy = 0;
        bool isCharge = true;
        public override void OnEnter(Enemy owner, EnemyStateBase prevState)
        {
            isCharge = true;
            chargeTime = 0.8f;
            atkTime = 0.5f;
            rigidityTime = 0.8f;
            owner.Rigidbody.velocity = Vector2.zero;
            vx = owner.PlayerTransform.position.x - owner.transform.position.x;
            vy = owner.PlayerTransform.position.y - owner.transform.position.y;
            owner.Animator.SetBool("Charge", true);
        }

        public override void OnFixedUpdate(Enemy owner)
        {
            //�U���̗���
            if (chargeTime > 0)
            {
                chargeTime -= Time.fixedDeltaTime;
            }
            //���߂��I���Ɠːi
            else if (isCharge)
            {
                isCharge = false;
 
                Vector2 direction = new Vector2(vx, vy).normalized; //�ǐՕ���
                owner.Rigidbody.AddForce(direction * 5.0f, ForceMode2D.Impulse);
            }
            //�U�����Ԃ̃J�E���g
            if (!isCharge && atkTime > 0)
            {
                atkTime -= Time.fixedDeltaTime;
           
                Vector2 direction = new Vector2(vx, vy).normalized; //�ǐՕ���
                owner.Rigidbody.AddForce(direction * 2.0f, ForceMode2D.Impulse);

            }
        
            //�����J�n
            if (!isCharge && rigidityTime > 0 && atkTime < 0)
            {
                rigidityTime -= Time.fixedDeltaTime;
            }
            //���̃X�e�[�g��
            else if(rigidityTime <= 0)
            {
                owner.ChangeToTracing();
            }
            owner.Animator.SetFloat("speed", owner.Rigidbody.velocity.magnitude);
        }

        public override void OnExit(Enemy owner, EnemyStateBase nextState)
        {
            owner.Animator.SetBool("Charge", false);
        }
    }

    /// <summary>
    /// UnityEvent�p�֐�
    /// �ːi�U���̃X�e�[�g�ɕύX
    /// </summary>
    public void ChangeToCharge()
    {
        if (currentState != s_stateTracing)
        {
            return;
        }
        ChangeState(s_stateCharge);
    }
}
