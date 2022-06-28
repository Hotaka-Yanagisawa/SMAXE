using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�N���X�̃X�e�[�g�ȊO�̏���
/// �e�N���X
/// </summary>
public partial class Enemy : MonoBehaviour
{    
    #region serialize field
    //[SerializeField]
    //private SpriteRenderer _spriteRenderer = null;  //�J�����O������p
    [SerializeField]
    private Rigidbody2D _rigidbody = null;
    [SerializeField]
    private EnemyParameter _enemyParameter = null;
    [SerializeField]
    private ParticleSystem _knockbackEffect = null;
    [SerializeField]
    private Animator _animator = null;
    [SerializeField]
    private GameObject _live2D = null;
    #endregion

    #region static
    static private float enemyTimeScale = 1.0f; //�G�l�~�[�Ǝ��̎���
    static public int activeEnemyNum = 0;       //active��Ԃ̃G�l�~�[
    static public Boids _boids = new Boids();   //�Q�O�A���S���Y���v�Z
    #endregion

    #region �t�B�[���h
    private Transform _playerTransform = null;
    private float _hp = 50;
    private bool _isChange = true;
    private DeadMessageSender _deadMessageSender = new DeadMessageSender();
    Vector2 _impulseForce = Vector2.zero;
    #endregion

    #region �v���p�e�B
    static public float EnemyFixedDeltaTime
    {
        get { return enemyTimeScale * Time.fixedDeltaTime; }
    }
    static public float EnemyDeltaTime
    {
        get { return enemyTimeScale * Time.deltaTime; }
    }
    static public float EnemyTimeScale
    {
        get { return enemyTimeScale; }
        set { enemyTimeScale = value;}
    }
    public ParticleSystem KnockbackEffect
    {
        get { return _knockbackEffect; }
    }
    public GameObject Live2D
    {
        get { return _live2D; }
    }
    public float Hp
    {
        get { return _hp; }
        set { _hp = value; }
    }
    public Animator Animator
    {
        get { return _animator;}
    }
    public bool IsChange
    {
        get { return _isChange; }
        set { _isChange = value; }
    }
    public Transform PlayerTransform
    {
        get { return _playerTransform; }
    }
    //public SpriteRenderer SpriteRenderer
    //{
    //    get { return _spriteRenderer; }
    //}
    public Rigidbody2D Rigidbody
    {
        get { return _rigidbody; }
    }
    public EnemyParameter EnemyParameter
    {
        get { return _enemyParameter; }
    }
    public DeadMessageSender DeadMessageSender
    {
        get { return _deadMessageSender; }
    }
    #endregion

    #region �ύX�̗]�n����
    public float t;
    public float switchDelay = 0;
    public const float DELAY = 0.5f;
    public int z = 0;
    public const float changeStateTime = 1.0f;
    public float changeStateCount = 0.0f;
    #endregion
    /// <summary>
    /// �Q�[���J�n���ɍs���鏈��
    /// </summary>
    protected virtual void Start()
    {
        _hp = _enemyParameter.MaxHP;
        _playerTransform = GameMngr.Instance.player.transform;
        _boids.SetList(Rigidbody, transform);
        _deadMessageSender.Init();
        OnStart();
    }
    private void OnEnable()
    {
        _hp = _enemyParameter.MaxHP;
        _live2D.SetActive(true);
        _live2D.transform.localScale = new Vector3(EnemyParameter.Size, EnemyParameter.Size,1);
        ++activeEnemyNum;
        t = 0;
        switchDelay = 0;
    }

    private void OnDisable()
    {
        --activeEnemyNum;
    }

    /// <summary>
    /// ���t���[���Ă΂�鏈��
    /// </summary>
    protected virtual void Update()
    {
        OnUpdate();
    }
    /// <summary>
    /// ���Ԋu�ōs���鏈��
    /// </summary>
    protected virtual void FixedUpdate()
    {
        OnFixedUpdate();
        //���x�Ń��f��������������ς���
        if (_rigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(_rigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}