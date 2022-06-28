using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyParameter")]
public class EnemyParameter : ScriptableObject
{
    public int MaxHP = 100;
    public float MoveSpeed = 5.0f;
    public float Attack = 5.0f;
    public float Size = 2.0f;
    public string Name = " ";
    public enum EnemyKind
    {
        Normal = 0,     // �ʏ�
        Explod,         // ����
        Golem,          // �S�[����
        WildBoar,       // �z�[�~���O
        Turret,         // �^���b�g

        MAX
    }
    public EnemyKind kind = EnemyKind.Normal;
    //�^�[���b�g�^�p
    public float shotInterval = 0.2f;
    public float reloadInterval = 2.0f;
}