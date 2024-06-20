using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool shootAuto;
    [SerializeField] private bool isDummy;
    [SerializeField] private Rigidbody rb;
    [SerializeField] float shootPower;
    [SerializeField] float bulletLife;

    [SerializeField] GameObject hitParticle;
    
    void Start()
    {
      
        if(shootAuto)
        {
            shoot(shootPower,bulletLife);
        }
    }

    public void shoot(float speed,float life)
    {
        
       
        rb.velocity = transform.forward * speed;
        Destroy(this.gameObject, life);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (isDummy)
        {
            if (hitParticle != null) Destroy(Instantiate(hitParticle, transform.position, transform.rotation), 3f);
            Destroy(this.gameObject);
            return;
        }



        if (collision.gameObject.GetComponent<DamageRecieve>())
        {
            collision.gameObject.GetComponent<DamageRecieve>().Damage(100);
            GamePlayHandler.Instance.AddScore(1);
        }
        if(collision.gameObject.tag=="Test")
        {
            Destroy(collision.gameObject);
            GamePlayHandler.Instance.AddScore(1);
        }
        if (hitParticle != null) Destroy(Instantiate(hitParticle, transform.position, transform.rotation), 3f);
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(isDummy)
        {
            if (hitParticle != null) Destroy(Instantiate(hitParticle, transform.position, transform.rotation), 3f);
            Destroy(this.gameObject);
            return;
        }
     
        if(collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<DamageRecieve>().Damage(100);
            GamePlayHandler.Instance.AddScore(1);
        }
        if (collision.gameObject.tag == "Test")
        {
            GamePlayHandler.Instance.AddScore(1);
            Destroy(collision.gameObject);
        }

        if (hitParticle != null) Destroy(Instantiate(hitParticle, transform.position, transform.rotation), 3f);
        Destroy(this.gameObject);
    }
}
