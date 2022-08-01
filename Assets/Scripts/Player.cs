using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Stats myStats;

    public Projectile laserPrefab;
    public bool laserActive;

    public float speed = 5.0f;

    [SerializeField] int health;

    [SerializeField] AudioSource shootClip;
    [SerializeField] AudioSource damageClip;

    public int Health //propiedad
    {
        get => health;
        set
        {
            health = value; //el valor que entra a la propiedad lo asignas a health...
            GameManager.sharedInstance.UpdateUIHealth(health);
        }
    }

    private void Start()
    {
        Health = myStats.life;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (!laserActive)
        {
            shootClip.Play();

            Projectile projectile = Instantiate(this.laserPrefab, transform.position, Quaternion.identity);
            //agrega el metodo a la accion destroyed, para que luego se ejecute..
            projectile.destroyed += LaserDestroyed; 
            laserActive = true;
        }
    }

    void LaserDestroyed()
    {
        laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            damageClip.Play();
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        Health--;   

        if (Health <= 0)
        {
            Destroy(gameObject, 0.1f);

            //
            myStats.life = 3;
            myStats.score = 0;
            SaveManager saveManager = FindObjectOfType<SaveManager>();
            saveManager.Save();

            GameManager.sharedInstance.GameOver();
        }
    }
}
