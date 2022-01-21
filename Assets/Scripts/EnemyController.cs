using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float  moveSpeed;
    private Rigidbody2D rigid;
    private GameObject targetPoint;
    private bool isTrigerActive;
    private Coroutine checkDelay;
    private bool loseGame;

    private Vector2 MoveVector;
    private Vector2 VecUp = new Vector2(0.1f, 0.9f);
    private Vector2 VecDown = new Vector2(-0.1f, -0.9f);
    private Vector2 collisionPosition;

    [Header("Skins")]
    private int NumberCharSkin;
    [SerializeField] private GameObject ImageSkin;
    [SerializeField] private List<Sprite> CharSkins;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        MoveVector = Vector2.left;
        NumberCharSkin = -1;
        loseGame = false;
    }


    void Update()
    {
        EnemySkins();
        if (targetPoint != null && loseGame == false)
        {
            Vector2 vec = targetPoint.transform.position - transform.position;
            float vecMag = vec.magnitude;
            if (vecMag < 0.1 && isTrigerActive == true)
            {
                TrafficSide(transform.position);
                AgainstError();
                isTrigerActive = false;
            }

        }     
    }

    private void FixedUpdate()
    {
        rigid.velocity = MoveVector.normalized * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.tag == "TrigerPoint")
        {
            targetPoint = collision.gameObject;
            MoveVector = collision.transform.position - transform.position;
            isTrigerActive = true;
            collisionPosition = transform.position;
            if (checkDelay != null)
            {
                StopCoroutine(checkDelay);
            }
        }

        if (collision.tag == "Bomb")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boundaries" || collision.gameObject.tag == "Enemy")
        {
            MoveVector = -MoveVector;
        }
    }

    void TrafficSide(Vector2 actualPosition)
    {
        Vector2 vec = actualPosition - collisionPosition;

        if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
        {
            if (vec.x > 0)
            {
                RandomDirection(Vector2.right, VecUp);

            }
            else
            {
                RandomDirection(Vector2.left , VecUp);

            }
        }
        else
        {
            if (vec.y > 0)
            {
                RandomDirection(VecUp, Vector2.right);
 
            }
            else
            {
                RandomDirection(VecDown, Vector2.right);

            }
          
        }
    } // ”знаю в какую сторону двигалс€

    void EnemySkins()
    {
        if (Mathf.Abs(MoveVector.x) > Mathf.Abs(MoveVector.y))
        {
            if (MoveVector.x > 0)
            {
                NumberCharSkin = 0;
            }
            else
            {
                NumberCharSkin = 1;
            }
        }
        else
        {
            if (MoveVector.y > 0)
            {
                NumberCharSkin = 3;
            }
            else
            {
                NumberCharSkin = 2;
            }
        }

        switch (NumberCharSkin)
        {

            case 0:
                ImageSkin.GetComponent<SpriteRenderer>().sprite = CharSkins[0];
                break;
            case 1:
                ImageSkin.GetComponent<SpriteRenderer>().sprite = CharSkins[1];
                break;
            case 2:
                ImageSkin.GetComponent<SpriteRenderer>().sprite = CharSkins[2];
                break;
            case 3:
                ImageSkin.GetComponent<SpriteRenderer>().sprite = CharSkins[3];
                break;
        }

    }  //ћен€ю спрайт

    void RandomDirection(Vector2 currentDirection, Vector2 perpendicularDirection)
    {
        int random = Random.Range(1, 100);
        if (random < 51)
        {
            MoveVector = currentDirection;
        }
        else if (random > 50 && random < 71)
        {
            MoveVector = perpendicularDirection;
        }
        else if (random > 70 && random < 91)
        {
            MoveVector = -perpendicularDirection;
        }
        else
        {
            MoveVector = -currentDirection;
        }
    }  // —лучайное направление 

    void AgainstError()
    {
        checkDelay = StartCoroutine(CheckDelay());
    } // на случай застревани€ 

    IEnumerator CheckDelay()
    {
        yield return new WaitForSeconds(3);
        if (isTrigerActive == false && loseGame == false)
        {
            transform.position = targetPoint.transform.position;
        }
    }

    public void LoseGame()
    {
        loseGame = true;
        MoveVector = Vector2.zero;
    }

}
