using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InvadersManager : MonoBehaviour
{
    public static InvadersManager sharedInstance; //la instancia compartida

    public List<Invader> prefabInvader = new List<Invader>();
    public GameObject invaderBase;

    //Matrix
    //GameObject[,] invadersMatriz; //matriz con filas y columnas
    public Invader[,] invadersMatriz; //matriz con filas y columnas
    public int xSize, ySize;
    float paddingX = 1.0f, paddingY = 1.0f;

    //Enemy
    public int amountKilled { get; set; }
    public int amountAlive => totalInvaders - amountKilled;
    public int totalInvaders => xSize * ySize;
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;

    //float speed = 1.0f;
    public AnimationCurve speed;
    Vector3 direction = Vector2.right;

    [Header("Projectile")]
    public Projectile missileEnemy;
    public float missileAttackRate = 1.0f;

    private void Start()
    {
        //se compara si existe una intancia ya creada
        if (sharedInstance == null)
        {
            sharedInstance = this; //se inicia, siendo el unico director..
        }
        else
        {
            Destroy(gameObject);
        }

        Vector2 offset = new Vector2(invaderBase.GetComponent<BoxCollider2D>().size.x + paddingX,
                                        invaderBase.GetComponent<BoxCollider2D>().size.y + paddingY);
        CreateInvaders(offset);

        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void Update()
    {
        this.transform.position += direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invaderTranform in this.transform)
        {
            if (!invaderTranform.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (direction == Vector3.right && invaderTranform.position.x >= rightEdge.x - 1.0f) //un leve padding..
            {
                AdvanceRow();
            }
            else if (direction == Vector3.left && invaderTranform.position.x <= leftEdge.x + 1.0f)
            {
                AdvanceRow();
            }
        }
    }

    void AdvanceRow()
    {
        direction.x *= -1.0f;

        Vector3 pos = this.transform.position;
        pos.y -= 1.0f;
        this.transform.position = pos;
    }

    void CreateInvaders(Vector2 _offset)
    {
        //invadersMatriz = new GameObject[xSize, ySize];
        invadersMatriz = new Invader[xSize, ySize];

        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        int aux;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                aux = Random.Range(0, prefabInvader.Count); //elije el numero al azar.

                //agrega la nave
                Invader invader = Instantiate(prefabInvader[aux],
                                                 new Vector3(startX + (_offset.x * x), startY + (_offset.y * y), 0),
                                                 invaderBase.transform.rotation);

                //guardar la posicion
                invader.posInvader = new Vector2(x, y);

                //para saber que color es..
                ChooseColor(invader, aux);
     
                //agrega el metodo a la accion killed, para que se ejecute cuando un enemigo muere, y sume una muerte..
                invader.killed += InvaderKilled; 

                //nombre a cada nave, para concer su ubicacion..
                invader.name = string.Format("Invader[{0}][{1}]", x, y);

                //lo haces hijo del controlador para que quede mejor ordenado..
                invader.transform.parent = this.transform;

                //lo guardas en la matriz
                invadersMatriz[x, y] = invader;
            }
        }
    }

    void ChooseColor(Invader _invader, int _aux)
    {
        switch (_aux)
        {
            case 0:
                _invader.colorInvader = Color.blue;
                break;
            case 1:
                _invader.colorInvader = Color.green;
                break;
            case 2:
                _invader.colorInvader = Color.red;
                break;
            case 3:
                _invader.colorInvader = Color.yellow;
                break;
        }
    }

    void InvaderKilled()
    {
        this.amountKilled++;

        if (this.amountKilled >= this.totalInvaders)
        {
            GameManager.sharedInstance.Win();
        }
    }

    void MissileAttack()
    {
        foreach (Transform invaderTranform in this.transform)
        {
            if (!invaderTranform.gameObject.activeInHierarchy) //si tal nave enemiga esta desactivada, es pq esta muerta
            {
                continue;
            }

            if (Random.value < (1.0f / (float)this.amountAlive))
            {
                Instantiate(this.missileEnemy, invaderTranform.position, Quaternion.identity);
                break; //para que sea un solo misil, por vez.
            }
        }
    }
}
