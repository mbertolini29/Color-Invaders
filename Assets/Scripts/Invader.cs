using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public Action killed; 

    public Sprite[] animationSprites;
    public float animationTime = 1.0f;
    
    int animationFrame;
    SpriteRenderer spriteRenderer;

    public Color colorInvader;
    public Vector2 posInvader;

    //[SerializeField] int health = 1;
    [SerializeField] int scorePoints = 0;
    int numMaxMatch = 1; //

    public GameObject explosion;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    void AnimateSprite()
    {
        animationFrame++;

        if(animationFrame >= this.animationSprites.Length)
        {
            animationFrame = 0;
        }

        spriteRenderer.sprite = this.animationSprites[animationFrame]; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser") )
        {
            this.killed.Invoke();
            this.gameObject.SetActive(false);

            Instantiate(explosion, transform.position, Quaternion.identity);

            //cuando un enemigo muere, busca sus adjacentes..
            FindAllMatches();

            FuctionFibonacci(numMaxMatch);
        }
    }

    void FuctionFibonacci(int _numMax)
    {
        //GameManager.sharedInstance.Score += 10;       

        int num1 = 10;
        int num2 = 20;
        int nextNum;

        for (int i = 1; i <= _numMax; i++)
        {
            if (i == 1)
            {
                scorePoints = (i * num1);
            }
            else if (i == 2)
            {
                scorePoints = (i * num2);
            }
            else
            {
                nextNum = num1 + num2;
                num1 = num2;
                num2 = nextNum;

                scorePoints = (i * nextNum);
            }
        }

        GameManager.sharedInstance.Score += scorePoints;
    }

    List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> matchingInvaders = new List<GameObject>();

        //lanza el rayo de tu posicion hacia la direccion que le pasaste!
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);

        //aca comprobas , si tenes vecino, y si es igual al color del invader destruido, 
        //pero una vez igualado, tenes que preguntarle,
        //si su vicino tmb es igual, hasta que sea distinto el vecino

        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == 7)
            {
                if(hit.distance < 3f)
                {
                    while (hit.collider.GetComponent<Invader>().colorInvader == gameObject.GetComponent<Invader>().colorInvader) //si ese vecino, tiene el mismo color
                    {
                        //significa que tu vecino es igual 
                        matchingInvaders.Add(hit.collider.gameObject);

                        //y entonces, volvemos a preguntar, si el vecino que es igual al tuyo, tiene otro vecino igual
                        hit = Physics2D.Raycast(hit.collider.transform.position, direction); //la misma direccion actual..

                        if((hit.collider == null) || (hit.transform.gameObject.layer != 7))
                        {
                            break;
                        }
                    }
                }              
            }
        }
        return matchingInvaders; //devolves los vecinos..
    }

    bool ClearMatch(Vector2[] directions)
    {
        List<GameObject> matchingInvaders = new List<GameObject>();

        foreach (Vector2 direction in directions) //consultas las direcciones que vienen por parametros..
        {
            //agregas la lista de objetos iguales que consultaste en FindMatch
            matchingInvaders.AddRange(FindMatch(direction)); //el findmatch, agrega una coleccion, 
        }

        foreach (GameObject invader in matchingInvaders) //para cada objeto obtenido de las coincidencias de la lista anterior
        {
            invader.GetComponent<Invader>().killed.Invoke();
            invader.SetActive(false);
            numMaxMatch++;
            
            Instantiate(explosion, invader.GetComponent<Invader>().transform.position, invader.GetComponent<Invader>().transform.rotation);
        }

        return true;      
    }

    public void FindAllMatches()
    {
        if (this.gameObject == null){
            return;
        }

        //de esta manera, busca coincidencias horizontales y verticales...

        bool hMatch = ClearMatch(new Vector2[2] { //vector con 2 direcciones..

            Vector2.left, Vector2.right 
        });

        bool vMatch = ClearMatch(new Vector2[2]
        {
            Vector2.up, Vector2.down
        });
    }

}
