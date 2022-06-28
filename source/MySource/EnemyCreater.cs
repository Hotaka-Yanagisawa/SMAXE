using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreater : MonoBehaviour
{
    public List<GameObject> enemyPool = new List<GameObject>();
    [SerializeField, Tooltip("敵の生成間隔")]
    private float appearanceTimeInterval = 5.0f;
    [SerializeField, Tooltip("敵の全体数")]
    private int totalEnemyNum = 50;
    [SerializeField, Tooltip("敵の生成座標の半径の最大値")]
    int arrangementMaxRadius = 30;
    [SerializeField, Tooltip("敵の生成座標の半径の最小値")]
    int arrangementMinRadius = 25;
    [SerializeField]
    Transform poolParent;
    [SerializeField, Tooltip("生成するだけか否か")]
    bool isCreate = true;


    [System.Serializable]
    public class EnemyAppearanceRate
    {
        [SerializeField]
        public GameObject kind = null;         //生成したいオブジェクト
        [SerializeField]
        public float appearanceRate = 1.0f;    // 出現率
    }

    public EnemyAppearanceRate[] enemyAppearanceRates;
    private System.Random random;                               // 乱数機
    private float totalVal = 0;

    void Start()
    {
        random = new System.Random();
        for (int i = 0; i < enemyAppearanceRates.Length; ++i)
        {
            totalVal += enemyAppearanceRates[i].appearanceRate;
        }

        for (int i = 0; i < enemyAppearanceRates.Length; ++i)
        {
            float rate = enemyAppearanceRates[i].appearanceRate / totalVal;
            float createNum = totalEnemyNum * rate;
            for (int j = 0; j < createNum; ++j)
            {
                var enemy = enemyAppearanceRates[i].kind;
                enemy.SetActive(false);

                int x;
                int y;

                float xAbs;
                float yAbs;

                while (true)
                {
                    float maxR = arrangementMaxRadius * arrangementMaxRadius;
                    float minR = arrangementMinRadius * arrangementMinRadius;

                    x = random.Next(-arrangementMaxRadius, arrangementMaxRadius);
                    y = random.Next(-arrangementMaxRadius, arrangementMaxRadius);

                    xAbs = x * x;
                    yAbs = y * y;

                    // 特定の範囲内か確認
                    if (maxR > xAbs + yAbs && xAbs + yAbs > minR)
                    {
                        enemyPool.Add(Instantiate(enemy, transform.position + new Vector3(x, y, 0), Quaternion.identity, poolParent));
                        break;
                    }
                }
            }
        }
        if(isCreate) InvokeRepeating("Create", 1.0f, appearanceTimeInterval);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        if (isCreate) CancelInvoke("Create");
    }

    void Update()
    {
    }

    /// <summary>
    /// 指定時間ごとに生成
    /// </summary>
    /// <returns></returns>
    public void Create()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeSelf)
            {
                int x;
                int y;

                float xAbs;
                float yAbs;

                float maxR = arrangementMaxRadius * arrangementMaxRadius;
                float minR = arrangementMinRadius * arrangementMinRadius;

                //範囲内になるまで終わらない
                while (true)
                {
                    x = random.Next(-arrangementMaxRadius, arrangementMaxRadius);
                    y = random.Next(-arrangementMaxRadius, arrangementMaxRadius);

                    xAbs = x * x;
                    yAbs = y * y;

                    // 特定の範囲内か確認
                    if (maxR > xAbs + yAbs && xAbs + yAbs > minR)
                    {
                        enemy.transform.position = new Vector3(x, y, 0);
                        //ここでアクティブ化しないとGroup.csをアタッチしてるオブジェクトに影響あり
                        enemy.SetActive(true);

                        break;
                    }
                }
                break;
            }
        }
    }
}
