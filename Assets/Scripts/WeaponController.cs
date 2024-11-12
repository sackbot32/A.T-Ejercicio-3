using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private Transform barrel;
    [Header("Ammo Settings")]
    [SerializeField]
    private int currentAmmo;
    [SerializeField]
    public int maxAmmo;
    [SerializeField]
    private bool infiniteAmmo;
    [Header("Performance")]
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float shootRate;
    [SerializeField]
    private int damage;

    private ObjectPool objPool;
    private float lastShotTime;

    private bool isPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastShotTime = Time.time;
        if(GetComponent<PlayerControl>() != null)
        {
            isPlayer = true;
        }

        objPool = GetComponent<ObjectPool>();
    }
    /// <summary>
    /// Handle Weapon Shoot
    /// </summary>
    public void Shoot()
    {
        lastShotTime = Time.time;
        if (!infiniteAmmo)
        {
            currentAmmo--;
        }

        GameObject bullet = objPool.GetGameObject();

        //Locate the ball at the barrel pos
        bullet.transform.position = barrel.position;
        bullet.transform.rotation = barrel.rotation;

        bullet.GetComponent<BulletController>().Damage = damage;
        if (isPlayer)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
            RaycastHit hit;
            Vector3 targetPoint;

            if(Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            } else
            {
                targetPoint = ray.GetPoint(5);
            }
            bullet.GetComponent<Rigidbody>().linearVelocity = (targetPoint - barrel.position).normalized * bulletSpeed;
        } else
        {
            bullet.GetComponent<Rigidbody>().linearVelocity = barrel.forward * bulletSpeed;
        }
        //TODO BulletController damage and speed
       

        bullet.SetActive(true);
    }
    /// <summary>
    /// Check if you can shoot
    /// </summary>
    /// <returns></returns>
    public bool CanShoot()
    {

        bool result = false;
        if(Time.time - lastShotTime >= shootRate)
        {
            if(currentAmmo > 0  || infiniteAmmo)
            {
                result = true;
            }
        }
        return result;
    }
}
