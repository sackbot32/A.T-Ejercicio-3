using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float activeTime;
    [Header("ParticleSettings")]
    [SerializeField] private GameObject damageParticle;
    [SerializeField] private GameObject impactParticle;

    private int damage;

    public int Damage { get => damage; set => damage = value; }


    private void OnEnable()
    {
        StartCoroutine(DisableOnTime());
    }


    private IEnumerator DisableOnTime()
    {
        yield return new WaitForSeconds(activeTime);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyControl>() != null)
        {
            Instantiate(damageParticle, transform.position, Quaternion.identity);
            other.GetComponent<EnemyControl>().DamageEnemy(Damage);
        }
        else if(other.GetComponent<PlayerHealth>() != null)
        {
            Instantiate(damageParticle, transform.position, Quaternion.identity);
            other.GetComponent<PlayerHealth>().TakeDamage(damage);


        }
        else
        {
            Instantiate(impactParticle, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

}
