using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�^�[���b�g����ł�e�̃N���X
public class TurretBullet : MonoBehaviour
{
    #region serialize field
    [SerializeField]
    private EnemyParameter parameter;
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;  //�J�����O������p
    [SerializeField]
    private Rigidbody2D _rigidbody = null;
    [SerializeField]
    private float _bulletSpeed = 5;
    [SerializeField]
    private float _destroyTime = 3.0f;
    #endregion

    #region �t�B�[���h
    private Vector3 _playerPosition;
    private Vector2 direction;
    bool isSE = true;
    #endregion

    #region �v���p�e�B
    public Vector3 PlayerPosition
    {
        get { return _playerPosition; }
    }
    public SpriteRenderer SpriteRenderer
    {
        get { return _spriteRenderer; }
    }
    public Rigidbody2D Rigidbody
    {
        get { return _rigidbody; }
    }
    #endregion

    void Start()
    {
        _playerPosition = GameMngr.Instance.player.transform.position;
        float vx = _playerPosition.x - transform.position.x;
        float vy = _playerPosition.y - transform.position.y;
        direction = new Vector2(vx, vy).normalized; //���˕���
        isSE = true;
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = direction * _bulletSpeed;
        _destroyTime -= Time.fixedDeltaTime;
        if (_destroyTime <= 0)
        {
            Destroy(gameObject);
            return;
        }

        //��ʊO�ŏ���
        if (!_spriteRenderer.isVisible)
        {
            Destroy(gameObject);
            return;
        }
        if (isSE)
        {
            isSE = false;
            SoundMngr.Instance.PlaySE(SoundMngr.E_SE.ENEMY_TURRET);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameMngr.Instance.IsPlayer(collision.gameObject))
        {
            GameMngr.Instance.playerCommonCS.AddDamage(parameter.Attack);
            Destroy(gameObject);
        }
    }
}
