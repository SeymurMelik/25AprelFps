using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour
{
    [SerializeField] float _force;
    [SerializeField] float _radius;
    PlayerController PmMaster;

    [SerializeField] float OwnDamage;

    private void Awake()
    {
        PmMaster = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PmMaster.Smash)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
            foreach (Collider item in colliders)
            {
                Rigidbody rb = item.GetComponent<Rigidbody>();
                EnemyController enemy = item.GetComponent<EnemyController>();
                if (rb!=null)
                {
                    rb.AddExplosionForce(_force, transform.position, _radius, 3f, ForceMode.Impulse) ;
                }

                if (rb != null && enemy != null && enemy.health > 0)
                {
                    enemy.TakeDamage(OwnDamage);
                    rb.AddExplosionForce(_force, transform.position, _radius, 3f, ForceMode.Impulse);
                }
            }    
        }
    }
}
