using System.Collections;
using UnityEngine;

/// <summary>
/// 敵クラスのステート処理
/// </summary>
public partial class Enemy : MonoBehaviour , IDamageble
{
    protected static readonly StateWaiting s_stateWaiting = new StateWaiting();
    protected static readonly StateTracing s_stateTracing = new StateTracing();
    protected static readonly StateKnockback s_stateKnockback = new StateKnockback();
    protected static readonly StateDead s_stateDead = new StateDead();
    /// <summary>
    /// 現在のステート
    /// </summary>
    protected EnemyStateBase currentState = s_stateTracing;

    /// <summary>
    /// 待機
    /// </summary>
    public class StateWaiting : EnemyStateBase
    {
        public override void OnFixedUpdate(Enemy owner)
        {
            if (owner._rigidbody.velocity.magnitude > 0)
            {
                owner._rigidbody.velocity = new Vector2(0, 0);
            }
            owner.Animator.SetFloat("speed", 0);
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public class StateDead : EnemyStateBase
    {
        public override void OnEnter(Enemy owner, EnemyStateBase prevState)
        {
            // 死亡通知
            owner.DeadMessageSender.SendDeadMessage(owner.gameObject);           
        }
        public override void OnFixedUpdate(Enemy owner)
        {
            owner.switchDelay += Time.fixedDeltaTime * enemyTimeScale;
            float size = 0;
            owner.t += Time.fixedDeltaTime*enemyTimeScale;
            //敵のサイズを小さくしていき消す。
            size = Mathf.Lerp(owner.EnemyParameter.Size, 0.01f, owner.t);
            
            if(size < 0.1f)
            {
                owner.transform.gameObject.SetActive(false);
                owner.ChangeState(s_stateTracing);
                return;
            }
            owner._live2D.transform.localScale = new Vector3(size, size, 1);

        }
        public override void OnExit(Enemy owner, EnemyStateBase nextState)
        {
            owner.transform.position = Vector3.zero;
            owner._live2D.SetActive(true);
        }
    }

    /// <summary>
    /// 追跡
    /// </summary>
    public class StateTracing : EnemyStateBase
    {
        public override void OnEnter(Enemy owner, EnemyStateBase prevState)
        {
            
        }
        public override void OnFixedUpdate(Enemy owner)
        {
            if (owner._rigidbody.velocity.magnitude < owner._enemyParameter.MoveSpeed)
            {
                //1.プレイヤー追尾
                float vx = owner._playerTransform.position.x - owner.transform.position.x;
                float vy = owner._playerTransform.position.y - owner.transform.position.y;
                //2.群種アルゴリズム
                float bx = _boids.Total(owner.transform.position, owner.Rigidbody).x;
                float by = _boids.Total(owner.transform.position, owner.Rigidbody).y;
                //1.2を合成して移動方向を決める
                Vector2 direction = new Vector2(vx + bx, vy + by).normalized;
               
               owner._rigidbody.AddForce(direction * EnemyTimeScale, ForceMode2D.Impulse);
               owner.Animator.SetFloat("speed", owner._rigidbody.velocity.magnitude);
            }
        }
    }

    /// <summary>
    /// ノックバック
    /// </summary>
    public class StateKnockback : EnemyStateBase
    {
        public override void OnEnter(Enemy owner, EnemyStateBase prevState)
        {
            owner.z = 0;
            owner.changeStateCount = changeStateTime;
            owner._knockbackEffect.Play();
            owner.IsChange = false;
            owner.Animator.SetTrigger("knockback");
            owner.Animator.SetFloat("speed", 0);
            owner.Rigidbody.velocity = Vector2.zero;
            owner.Rigidbody.AddForce(owner._impulseForce, ForceMode2D.Impulse);
            if(owner.EnemyParameter.kind == EnemyParameter.EnemyKind.Golem && owner.Hp > 0)
            {
                SoundMngr.Instance.PlaySE(SoundMngr.E_SE.ENEMY_GOLEM);
            }
        }
        public override void OnFixedUpdate(Enemy owner)
        {
            //ノックバック時モデルを回転させる
            owner.z += 30;
            owner._live2D.transform.localRotation = Quaternion.Euler(0, 0, owner.z);
            owner.changeStateCount -= Time.fixedDeltaTime;
            if(owner.changeStateCount <= 0 && owner.Hp > 0)
            {
                owner.IsChange = true;
                if (owner.EnemyParameter.kind != EnemyParameter.EnemyKind.Turret)
                {
                    owner.ChangeState(s_stateTracing);
                }
                else
                {
                    owner.ChangeState(s_stateWaiting);
                }
            }
            else if(owner.changeStateCount <= 0 && owner.Hp <= 0)
            {
                owner.IsChange = true;
                owner.ChangeState(s_stateDead);
            }
        }
        public override void OnExit(Enemy owner, EnemyStateBase nextState)
        {
            owner._knockbackEffect.Stop();
            owner.Animator.SetBool("knockback", false);
            owner._live2D.transform.localRotation = Quaternion.identity;
        }
    }

    protected virtual void OnStart()
    {
        currentState.OnEnter(this, null);
    }

    protected virtual void OnUpdate()
    {
        currentState.OnUpdate(this);
    }

    protected virtual void OnFixedUpdate()
    {
        currentState.OnFixedUpdate(this);
    }

    /// <param name="nextState"></param>
    protected virtual void ChangeState(EnemyStateBase nextState)
    {
        if (currentState != nextState && IsChange)
        {
            currentState.OnExit(this, nextState);
            nextState.OnEnter(this, currentState);
            currentState = nextState;
        }
    }

    public void ChangeToKnockback(Vector2 impulseForce)
    {
        _impulseForce = impulseForce;
        if(EnemyParameter.Name != "Explod")
        ChangeState(s_stateKnockback);
    }

    /// <summary>
    /// UnityEvent
    /// </summary>
    public void ChangeToWait()
    {
        ChangeState(s_stateWaiting);
    }

    /// <summary>
    /// UnityEvent
    /// </summary>
    public void ChangeToTracing()
    {
        ChangeState(s_stateTracing);
    }

    public void AddDamage(float damage)
    {
        Hp -= damage;
    }
}
