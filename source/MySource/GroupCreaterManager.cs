using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//30,80,150,250,300 討伐数　
//30,40,50,60,80　出現数
//10体の群れ
//300体でゲーム終了

//グループ生成を管理する
public class GroupCreaterManager : SingletonMonoBehaviour<GroupCreaterManager>
{
    List<GameObject> createrList = new List<GameObject>();
    private System.Random random = new System.Random();                               // 乱数機
    [SerializeField]
    private GameObject groupCreater = null;
    [SerializeField]
    int createNum = 10;
    [SerializeField]
    private AnimationCurve spawnCurve;
    void Start()
    {
        float x = 0;
        float y = 0;
        float rad = 0;

        for (int i = 0; i < createNum; ++i)
        {
            rad = 360.0f / (float)createNum * (float)i * (Mathf.PI / 180.0f); 
            x = Mathf.Cos(rad) * 23.0f;
            y = Mathf.Sin(rad) * 15.0f;

            createrList.Add(Instantiate(groupCreater, transform.position + new Vector3(x, y, 0), Quaternion.identity, transform));
        }
    }

    void FixedUpdate()
    {
        int activeMax = (int)spawnCurve.Evaluate(GameMngr.Instance.deadEnemyCounter.deadTotal);
        int activeObjNum = random.Next(0, createNum);

        transform.position = GameMngr.Instance.player.transform.position;

        //敵の数が減ったら
        if (activeMax - GroupCreater.createNum > Enemy.activeEnemyNum)
        {
            int cnt = 0;
            //リストの中から
            foreach (var creater in createrList)
            {
                //ランダムなものをactive
                if (activeObjNum == cnt)
                {
                    creater.SetActive(true);
                    break;
                }
                ++cnt;
            }
        }
    }
}
