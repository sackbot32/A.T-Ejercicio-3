using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{

    [SerializeField]
    private string enemyName;
    [SerializeField]
    private string description;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float shootRate;
    [SerializeField]
    private Material enemyMat;
    [SerializeField]
    private int maxLife;

    public float Speed { get => speed;}
    public int MaxLife { get => maxLife;}
    public float ShootRate { get => shootRate;}

    public string EnemyName { get => enemyName;}
    public string Description { get => description;}

    public Material Material { get => enemyMat;}
}
