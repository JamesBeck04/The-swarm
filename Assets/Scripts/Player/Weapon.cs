using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponDamage;
    public Camera playerCamera;
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    public int bulletPerBurst = 3;
    public int BurstbulletsLeft;

    public float spreadIntensity;
    public bool isActiveWeapon;

    // Start is called before the first frame update
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public GameObject muzzleEffect;
    public Transform muzzlePosition;

    public bool pausecheck;

    public enum WeaponModel 
    { 
        Pistol,
        Shotgun
    }

    public WeaponModel thisWeaponModel;



    public enum ShootingMode { 
    single,
    burst,
    Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
       
        readyToShoot = true;
        BurstbulletsLeft = bulletPerBurst;

        bulletsLeft = magazineSize;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.EmptySound.Play();
        }

        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.single || currentShootingMode == ShootingMode.burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
        {
            Reload();
        }

        if (readyToShoot && isShooting && bulletsLeft > 0)
        {
            BurstbulletsLeft = bulletPerBurst;
            FireWeapon();
        }

        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletPerBurst}/{magazineSize / bulletPerBurst}";
        }


    }
    
    private void FireWeapon()
    {
        if(Time.deltaTime <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameObject Flash = Instantiate(muzzleEffect, muzzlePosition);
            Destroy(Flash, 0.1f);

            //SoundManager.Instance.ShootingSound.Play();
            SoundManager.Instance.PlayShootingSound(thisWeaponModel);

            bulletsLeft--;
            readyToShoot = false;
            Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;



            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            Bullet bul = bullet.GetComponent<Bullet>();
            bul.bulletDamage = weaponDamage;
            bullet.transform.forward = shootingDirection;

            bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

            StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

            if (allowReset)
            {
                Invoke("ResetShot", shootingDelay);
                allowReset = false;
            }

            if (currentShootingMode == ShootingMode.burst && BurstbulletsLeft > 1)
            {
                BurstbulletsLeft--;
                Invoke("FireWeapon", shootingDelay);
            }
        }
   
    }

    private void Reload()
    {
        //SoundManager.Instance.ReloadSound.Play();
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet); 
    }
}
