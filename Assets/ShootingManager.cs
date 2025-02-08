using StarterAssets;
using UnityEngine;


public class ShootingManager : MonoBehaviour
{
    public GameObject bulletPref;
    public Transform shootPoint;
    float T = 0;
    public float reloadTime = 1f;

    [Space]
    public WindManager windManager;
    public float shootingSpeed;
    public float gravityForce;
    public float bulletLifeTime;


    void Update()
    {
      T += Time.deltaTime;   
    }

    public void Shoot()
    {
        if (T >= reloadTime)
        {
            GameObject bullet = Instantiate(bulletPref, shootPoint.position, shootPoint.rotation);
            ParabolicBullet bulletScript = bullet.GetComponent<ParabolicBullet>();
            if (bulletScript)
            {
                bulletScript.Initialize(shootPoint, shootingSpeed, gravityForce, windManager.GetWind());
            }
            Destroy(bullet, bulletLifeTime);
            T = 0;
        }

    }


}
