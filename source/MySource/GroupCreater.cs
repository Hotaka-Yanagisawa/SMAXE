using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//30,80,150,250,300 討伐数　
//30,40,50,60,80　出現数
//10体の群れ
//300体でゲーム終了

public class GroupCreater : MonoBehaviour
{
    [SerializeField, Tooltip("敵の生成座標の半径の最大値")]
    int arrangementMaxRadius = 2;
    private EnemyCreater enemyCreater;
    static public int createNum = 3;
    private void Start()
    {
        enemyCreater = GameMngr.Instance.enemyCreater;
    }
 
    private void OnEnable()
    {
  
    }

    private void FixedUpdate()
    {
        Create();
    }

    /// <summary>
    /// 指定時間ごとに生成
    /// </summary>
    /// <returns></returns>
    public void Create()
    {
        int cnt = 0;
        System.Random random = new System.Random();        
       
        foreach (var enemy in enemyCreater.enemyPool)
        {
            if (cnt >= createNum) break;
            if (!enemy.activeSelf)
            {
                int x;
                int y;

                x = random.Next(-arrangementMaxRadius, arrangementMaxRadius);
                y = random.Next(-arrangementMaxRadius, arrangementMaxRadius);
                enemy.transform.position = new Vector3(transform.position.x + x, transform.position.y + y, 0);
                enemy.SetActive(true);
                ++cnt;
            }
        }
       
        gameObject.SetActive(false);
    }
}
