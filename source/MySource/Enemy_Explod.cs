using Live2D.Cubism.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̎q�N���X
/// �N���^�̓G
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
    /// �����U���̏������s���X�e�[�g
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
            // ���S�ʒm
            owner.DeadMessageSender.SendDeadMessage(owner.gameObject);
        }

        public override void OnFixedUpdate(Enemy owner)
        {
            //�����܂ł̃J�E���g�_�E��
            if (_explodTime > 0)
            {
                _explodTime -= EnemyFixedDeltaTime;

                //�F�̐؂�ւ��i����
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

                //�U������o�� �ʃI�u�W�F�N�g��
                Instantiate(_createObj, owner.transform.position, owner.transform.rotation);
                //��A�N�e�B�u��
                owner.transform.position = Vector3.zero;
                _cubismRenderer.Color = new Color(1, 1, 1, 1);
                owner.IsChange = true;
                owner.ChangeToWait();
                owner.gameObject.SetActive(false);
            } 

        }
    }

    /// <summary>
    /// UnityEvent�p�֐�
    /// �X�e�[�g�ύX
    /// </summary>
    public void ChangeToExplod()
    {
        ChangeState(s_stateExplod);
    }
}
