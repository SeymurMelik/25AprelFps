using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Gun variables")]
    public int Damage, MagazineSize, BulletsPerTap, BulletsLeft, BulletsShot;
    public float TimeBetweenShooting, spread, Range, ReloadTime, TimeBetweenShots;


    [Header("Bools")]
    public bool AllowButtonHold;
    bool _readyToShot;
    bool _isShooting;
    bool allowInvoke;

    [Header("References")]
    public Camera FpsCam;
    public Transform AttachPoint;
    public LayerMask WhatIsEnemy;

    [Header("Particle")]
    public ParticleSystem MuzzleFlash;

    private void Awake()
    {
        BulletsLeft = MagazineSize;
        _readyToShot = true;
        allowInvoke = true;
    }

    private void Update()
    {
        MyInputs();
    }

    void MyInputs()
    {
        if (AllowButtonHold) _isShooting =  Input.GetMouseButton(0);
        else _isShooting =  Input.GetKeyDown(KeyCode.Mouse0);

        if (_readyToShot && _isShooting)
        {
            BulletsShot = BulletsPerTap;
            Shoot();
        }
    }
    void Shoot()
    {
        MuzzleFlash.Play();
        _readyToShot = false;
        Ray ray = FpsCam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        if (Physics.Raycast(ray,out hit,Range))
        {
            targetPoint = hit.point;
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(Damage);
            }
        }
        else
        {
            targetPoint = ray.GetPoint(50f);
        }

        Vector3 dirWithOutSpread = targetPoint - AttachPoint.position;

        Vector3 dirWithSpread = dirWithOutSpread + new Vector3(x,y,0);

        Instantiate(MuzzleFlash, hit.point, Quaternion.Euler(0,180,0));

        if (allowInvoke)
        {
            Invoke(nameof(ResetShoot), TimeBetweenShooting);
            allowInvoke = false;
        }

        if ( BulletsLeft > 0 && BulletsShot < BulletsPerTap)
        {
            Invoke(nameof(Shoot), TimeBetweenShots);
        }
    }

    void ResetShoot()
    {
        _readyToShot = true;
        allowInvoke = true;
    }
}
