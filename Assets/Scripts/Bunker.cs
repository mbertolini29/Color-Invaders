using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : MonoBehaviour
{
    [SerializeField] int health = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        health--;

        if(health <= 0)
        {
            Destroy(gameObject, 0.1f);
        }
    }

}
