using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の子クラス
/// ターレット型の敵がショット攻撃をしてくる
/// </summary>
public class Enemy_Turret : Enemy
{
    [SerializeField]
    private GameObject _bullet = null;         //生成したいオブジェクト
    private StateShot s_stateShot;
    private float _shotTime = 0;
    private float _reloadTime = 0;
    private int _shotCnt = 0;
    private bool _isReload = false;
    private const int MAX_MAG = 3;             //マガジンサイズ

    //入れ子クラスにインスタンスを渡す
    protected override void OnStart()
    {
        s_stateShot = new StateShot(_bullet, this);
        currentState = s_stateWaiting;
        base.OnStart();
    }

    /// <summary>
    /// ショット攻撃の処理を行うステート
    /// </summary>
    public class StateShot : EnemyStateBase
    {
        private GameObject _bullet = null;
        private Enemy_Turret _turret = null;


        public StateShot(GameObject bullet, Enemy_Turret turret)
        {
            _bullet = bullet;
            _turret = turret;
        }

        public override void OnEnter(Enemy owner, EnemyStateBase prevState)
        {
            owner.Animator.SetBool("shot", true);
            _turret._shotTime = 0;
            _turret._reloadTime = 0;
            _turret._shotCnt = 0;
            _turret._isReload = false;
        }

        public override void OnUpdate(Enemy owner)
        {
            float vx = GameMngr.Instance.player.transform.position.x - owner.transform.position.x;
            //向き
            if (vx > 0)
            {
                owner.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(vx < 0)
            {
                owner.transform.localScale = new Vector3(1, 1, 1);
            }


            if (!_turret._isReload)
            {
                _turret._shotTime += Time.fixedDeltaTime;
            }
            //数発撃つ→リロード→三発撃つ

            //撃つ
            if (_turret._shotTime > owner.EnemyParameter.shotInterval)
            {
                Instantiate(_bullet, owner.transform.position, Quaternion.identity);
                _turret._shotTime = 0;
                _turret._shotCnt++;
            }
            //数回撃ったらリロード
            if (_turret._shotCnt >= MAX_MAG)
            {
                _turret._shotCnt = 0;
                _turret._isReload = true;

            }
            if (_turret._isReload)
            {
                _turret._reloadTime += Time.fixedDeltaTime;
            }
            //Reload終わったら
            if (_turret._reloadTime > owner.EnemyParameter.reloadInterval)
            {
                _turret._reloadTime = 0;
                _turret._isReload = false;
            }
        }

        public override void OnExit(Enemy owner, EnemyStateBase nextState)
        {
            owner.Animator.SetBool("shot", false);
        }
    }
    /// <summary>
    /// UnityEvent用関数
    /// 攻撃のステートに変更
    /// </summary>
    public void ChangeToShot()
    {
        if (currentState != s_stateWaiting)
        {
            return;
        }
        ChangeState(s_stateShot);
    }
}

 

