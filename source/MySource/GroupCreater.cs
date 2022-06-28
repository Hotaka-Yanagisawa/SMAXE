using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//30,80,150,250,300 �������@
//30,40,50,60,80�@�o����
//10�̂̌Q��
//300�̂ŃQ�[���I��

public class GroupCreater : MonoBehaviour
{
    [SerializeField, Tooltip("�G�̐������W�̔��a�̍ő�l")]
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
    /// �w�莞�Ԃ��Ƃɐ���
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
