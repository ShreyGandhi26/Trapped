using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public float maxDist;

    public float Damage;

    public ParticleSystem bloodBurst;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        maxDist += 1 * Time.deltaTime;
        if (maxDist >= 5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyAi>() != null)
        {
            other.GetComponent<EnemyAi>().health -= Damage;
            Instantiate(bloodBurst, transform.position, transform.rotation);
            Destroy(gameObject);

        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Walls"))
        {
            Destroy(this.gameObject);
        }
    }
}
