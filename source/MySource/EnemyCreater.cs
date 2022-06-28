using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreater : MonoBehaviour
{
    public List<GameObject> enemyPool = new List<GameObject>();
    [SerializeField, Tooltip("�G�̐����Ԋu")]
    private float appearanceTimeInterval = 5.0f;
    [SerializeField, Tooltip("�G�̑S�̐�")]
    private int totalEnemyNum = 50;
    [SerializeField, Tooltip("�G�̐������W�̔��a�̍ő�l")]
    int arrangementMaxRadius = 30;
    [SerializeField, Tooltip("�G�̐������W�̔��a�̍ŏ��l")]
    int arrangementMinRadius = 25;
    [SerializeField]
    Transform poolParent;
    [SerializeField, Tooltip("�������邾�����ۂ�")]
    bool isCreate = true;


    [System.Serializable]
    public class EnemyAppearanceRate
    {
        [SerializeField]
        public GameObject kind = null;         //�����������I�u�W�F�N�g
        [SerializeField]
        public float appearanceRate = 1.0f;    // �o����
    }

    public EnemyAppearanceRate[] enemyAppearanceRates;
    private System.Random random;                               // �����@
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

                    // ����͈͓̔����m�F
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
    /// �w�莞�Ԃ��Ƃɐ���
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

                //�͈͓��ɂȂ�܂ŏI���Ȃ�
                while (true)
                {
                    x = random.Next(-arrangementMaxRadius, arrangementMaxRadius);
                    y = random.Next(-arrangementMaxRadius, arrangementMaxRadius);

                    xAbs = x * x;
                    yAbs = y * y;

                    // ����͈͓̔����m�F
                    if (maxR > xAbs + yAbs && xAbs + yAbs > minR)
                    {
                        enemy.transform.position = new Vector3(x, y, 0);
                        //�����ŃA�N�e�B�u�����Ȃ���Group.cs���A�^�b�`���Ă�I�u�W�F�N�g�ɉe������
                        enemy.SetActive(true);

                        break;
                    }
                }
                break;
            }
        }
    }
}
