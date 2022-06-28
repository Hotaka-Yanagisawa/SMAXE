/// <summary>
/// EnemyStateの抽象クラス
/// </summary>
public abstract class EnemyStateBase
{
    /// <summary>
    /// ステートを開始した時に呼ばれる
    /// </summary>
    public virtual void OnEnter(Enemy owner, EnemyStateBase prevState) { }
    /// <summary>
    /// 毎フレーム呼ばれる
    /// </summary>
    public virtual void OnUpdate(Enemy owner) { }  
    /// <summary>
    /// 一定間隔で呼ばれる
    /// </summary>
    public virtual void OnFixedUpdate(Enemy owner) { }
    /// <summary>
    /// ステートを終了した時に呼ばれる
    /// </summary>
    public virtual void OnExit(Enemy owner, EnemyStateBase nextState) { }
}
