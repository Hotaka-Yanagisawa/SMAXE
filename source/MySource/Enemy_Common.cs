using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵クラスのステート以外の処理
/// 親クラス
/// </summary>
public partial class Enemy : MonoBehaviour
{    
    #region serialize field
    //[SerializeField]
    //private SpriteRenderer _spriteRenderer = null;  //カメラ外か判定用
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
    static private float enemyTimeScale = 1.0f; //エネミー独自の時間
    static public int activeEnemyNum = 0;       //active状態のエネミー
    static public Boids _boids = new Boids();   //群衆アルゴリズム計算
    #endregion

    #region フィールド
    private Transform _playerTransform = null;
    private float _hp = 50;
    private bool _isChange = true;
    private DeadMessageSender _deadMessageSender = new DeadMessageSender();
    Vector2 _impulseForce = Vector2.zero;
    #endregion

    #region プロパティ
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

    #region 変更の余地あり
    public float t;
    public float switchDelay = 0;
    public const float DELAY = 0.5f;
    public int z = 0;
    public const float changeStateTime = 1.0f;
    public float changeStateCount = 0.0f;
    #endregion
    /// <summary>
    /// ゲーム開始時に行われる処理
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
    /// 毎フレーム呼ばれる処理
    /// </summary>
    protected virtual void Update()
    {
        OnUpdate();
    }
    /// <summary>
    /// 一定間隔で行われる処理
    /// </summary>
    protected virtual void FixedUpdate()
    {
        OnFixedUpdate();
        //速度でモデルが向く方向を変える
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