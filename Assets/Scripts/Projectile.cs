using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    public Action destroyed; //aca se crea la accion (un delegado y un evento en la misma linea)

    void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        //el ? se refiere a si tenes algun metodo agregado, lo invoca.. igual a (this.destroyed != null)
        this.destroyed?.Invoke();
        //cuando colisiona con algo, ejecuta/invoca al metodo creado en player..
        //para que una vez que colisiona con algo, vuelva a activar el disparo..

        Destroy(this.gameObject);
    }
}
