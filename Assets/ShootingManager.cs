using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections.Generic;

public class ShootingManager : MonoBehaviour
{
    public GameObject bulletPref;
    public GameObject trigger;
    public ParticleSystem muzzleFlash;
    public Transform shootPoint;
    float elapsedSinceLastShotTime = 0;
    public float reloadTime = 1f;
    public float shootingSpeed;
    public float gravityForce;
    public float bulletLifeTime;
    public int bulletCount;
    [Space]
    public WindManager windManager;
    public Rigidbody rb;
    [Space]
    public AudioClip gunshotClip;
    public AudioClip emptyBulletClip;
    public AudioClip magSlideInClip;
    public AudioSource audioSource;
    [SerializeField, Range(0f, 1f)]
    public float volume;
    [Space]
    public Camera lensCamera;
    public float lensRaycastMaxDistance = 5000f;
    private Ray lensRay;
    private RaycastHit lensRayHit;
    public TextMeshProUGUI distanceText;
    [Space]
    public Slider windSlider;

    private void Start()
    {
        lensRay = new Ray();
        lensRayHit = new RaycastHit();
        bulletCount = 0;
    }

    void Update()
    {
        lensRay.origin = lensCamera.transform.position;
        lensRay.direction = lensCamera.transform.forward;

        UpdateWindSlider();
        if (Physics.Raycast(lensRay, out lensRayHit, lensRaycastMaxDistance)) // Check if the ray hits something withinthe max distance
        {
            // Calculate the distance to the hit point
            float distance = lensRayHit.distance;
            distanceText.text = "Dist: " + distance.ToString("F2") + " m" +

                                //"\nHit object: " + lensRayHit.collider.name  ;
                                "\nAmmo: " + bulletCount;
        }
        else
        {
            distanceText.text = "Dist: "+ lensRaycastMaxDistance.ToString()+"m+"+
                                "\nAmmo: " + bulletCount; ;
        }

        elapsedSinceLastShotTime += Time.deltaTime;
    }

    private void UpdateWindSlider()
    {
        Vector2 gunVector = new Vector2(this.transform.right.x, this.transform.right.y);
        float windProjection = Vector2.Dot(gunVector, windManager.GetWind());
        windSlider.value = windProjection / (windManager.windMaxMagnitude) + 0.5f;
    }

    public void Shoot()
    {
        if (elapsedSinceLastShotTime >= reloadTime && bulletCount > 0)
        {
            audioSource.PlayOneShot(gunshotClip, volume);
            ParticleSystem flash = Instantiate(muzzleFlash, shootPoint.position,Quaternion.identity);
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);
            GameObject bullet = Instantiate(bulletPref, shootPoint.position, shootPoint.rotation);
            ParabolicBullet bulletScript = bullet.GetComponent<ParabolicBullet>();
            if (bulletScript)
            {
                bulletScript.Initialize(shootPoint, shootingSpeed, gravityForce, windManager.GetWind());
            }
            bulletCount--;
            Destroy(bullet, bulletLifeTime);
            elapsedSinceLastShotTime = 0;
        }
        else if (bulletCount <= 0)
            audioSource.PlayOneShot(emptyBulletClip, volume);
    }

    public void resetAmmo()
    {
        bulletCount = 10;
        audioSource.PlayOneShot(magSlideInClip, volume);
    }
}
