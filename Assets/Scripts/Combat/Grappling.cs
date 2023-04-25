using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    public PlayerController PmMaster;
    public Transform GunTip;
    public Transform Cam;
    public LayerMask WhatIsEnemy;
    public LineRenderer Line;
    GameObject _gb;



    [Header("Variables")]
    public float MaxGrabAbleDistance;
    public float MaxGrabAbleDelay;

    Vector3 _grappPoint;


    [Header("Cooldown")]
    public float GrapplingCooldown, MaxGrapplingCooldown;

    [SerializeField] bool _grappling;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) StartGrappling();

        if (GrapplingCooldown > 0) GrapplingCooldown -= Time.deltaTime;
    }
    private void LateUpdate()
    {
        if (_grappling) Line.SetPosition(0, GunTip.position);
    }

    void StartGrappling()
    {
        if (GrapplingCooldown > 0) return;

        _grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(Cam.position, Cam.forward, out hit, MaxGrabAbleDistance, WhatIsEnemy))
        {
            _gb = hit.collider.gameObject;
            _grappPoint = hit.point;
            Invoke(nameof(ExecuteGrappling), MaxGrabAbleDelay);

        }
        else
        {
            _grappPoint = Cam.position + Cam.forward * MaxGrabAbleDistance;

            Invoke(nameof(StopGrappling), MaxGrabAbleDelay);
        }
        Line.enabled = true;
        Line.SetPosition(1, _grappPoint);
    }
    void ExecuteGrappling()
    {
        Vector3 pos = (PmMaster.transform.position - _gb.transform.position).normalized;

        if (_gb.GetComponent<Rigidbody>()) _gb.GetComponent<Rigidbody>().AddForce(pos * 50, ForceMode.Impulse);

        Invoke(nameof(StopGrappling), MaxGrabAbleDelay);
    }
    void StopGrappling()
    {
        _grappling = false;

        GrapplingCooldown = MaxGrapplingCooldown;
        Line.enabled = false;
    }
}
