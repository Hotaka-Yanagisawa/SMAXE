using Live2D.Cubism.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の子クラス
/// 起爆型の敵
/// </summary>
public class Enemy_Explod : Enemy
{
    private StateExplod s_stateExplod;
    [SerializeField] private GameObject obj = null;
    [SerializeField] private CubismRenderer _cubismRenderer;
    [SerializeField] private AnimationCurve _animationCurve;

    protected override void OnStart()
    {
        s_stateExplod = new StateExplod(obj, this._cubismRenderer, _animationCurve);
        base.OnStart();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GameMngr.Instance.player.layer)
        {
            if(GameMngr.Instance.player.GetComponent<PlayerCommon>().currentState is PlayerCommon.StateShot)
            ChangeToExplod();
        }
    }


    /// <summary>
    /// 爆発攻撃の処理を行うステート
    /// </summary>
    public class StateExplod : EnemyStateBase
    {
        float _explodTime = 0;
        private GameObject _createObj = null;
        private CubismRenderer _cubismRenderer;
        private AnimationCurve _animationCurve;
        int rnd;

        public StateExplod (GameObject obj, CubismRenderer cubism, AnimationCurve curve)
        {
            _createObj = obj;
            _cubismRenderer = cubism;
            _animationCurve = curve;
        }

        public override void OnEnter(Enemy owner, EnemyStateBase prevState)
        {
            _explodTime = 3.0f;
            
            owner.Rigidbody.velocity = Vector2.zero;
            owner.IsChange = false;
            owner.Animator.SetTrigger("explod");
            owner.KnockbackEffect.Play();
            // 死亡通知
            owner.DeadMessageSender.SendDeadMessage(owner.gameObject);
        }

        public override void OnFixedUpdate(Enemy owner)
        {
            //爆発までのカウントダウン
            if (_explodTime > 0)
            {
                _explodTime -= EnemyFixedDeltaTime;

                //色の切り替え（明滅
                float flickering = _animationCurve.Evaluate(_explodTime / 3.0f) * 1000;

                if((int)flickering % 2 == 0)
                {
                    _cubismRenderer.Color = new Color(1, 0, 0, 1);
                }
                else
                {
                    _cubismRenderer.Color = new Color(1, 1, 1, 1);
                }
            }
            else
            {
                owner.KnockbackEffect.Stop();

                //攻撃判定出す 別オブジェクトで
                Instantiate(_createObj, owner.transform.position, owner.transform.rotation);
                //非アクティブ化
                owner.transform.position = Vector3.zero;
                _cubismRenderer.Color = new Color(1, 1, 1, 1);
                owner.IsChange = true;
                owner.ChangeToWait();
                owner.gameObject.SetActive(false);
            } 

        }
    }

    /// <summary>
    /// UnityEvent用関数
    /// ステート変更
    /// </summary>
    public void ChangeToExplod()
    {
        ChangeState(s_stateExplod);
    }
}
