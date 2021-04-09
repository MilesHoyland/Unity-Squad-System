using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    Rigidbody rb;
    float speed = 20f;
    float damage = 20f;
    Vector3 direction = Vector3.zero;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        Destroy(this.gameObject, 3f);
    }

    public void Fire(Vector3 directionIn)
    {
        direction = directionIn;
    }

    private void Update()
    {
        if(direction != Vector3.zero)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;

        if(collision.gameObject.GetComponent<EnemyAI>())
        {
            collision.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
            Destroy(this);
        }
        else
        {
            if (LayerMask.ReferenceEquals(collision.gameObject.layer, LayerMask.NameToLayer("Environment")))
            {
                Destroy(this);
            }
        }
    }

}
