using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int bulletDamage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + " !");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            print("hit");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }


        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("hit enemy");
            collision.gameObject.GetComponent<EnemyMovement>().TakeDamage(bulletDamage);
            Destroy(gameObject);

        }
    }

    void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject hole = Instantiate(
            ReferenceManager.Instance.BulletImpactStoneEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(collision.gameObject.transform);

    }
}
