using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootingManager : MonoBehaviour
{
    public GameObject bulletPref;
    public Transform shootPoint;
    float T = 0;
    public float reloadTime = 1f;
    public float shootingSpeed;
    public float gravityForce;
    public float bulletLifeTime;
    [Space]
    public WindManager windManager;
    public Rigidbody rb;
    [Space]
    public AudioClip gunshotSound; 
    public AudioSource audioSource;
    [SerializeField, Range(0f, 1f)]
    public float volume;


    void Update()
    {
      T += Time.deltaTime;

    }

    public void Shoot()
    {
        if (T >= reloadTime)
        {
            audioSource.PlayOneShot(gunshotSound,volume);


            rb.AddForce(Vector3.up * 300f, ForceMode.Impulse);
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
