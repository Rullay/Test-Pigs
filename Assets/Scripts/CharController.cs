using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharController : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject ButtonMoveControl;
    [SerializeField] private GameObject Bomb;
    [SerializeField] private GameObject GameController;
    [SerializeField] private Slider kdBomb;


    [SerializeField] private float minMoveSpped;
    [SerializeField] private float maxMoveSpped; 
    private Vector2 MoveVector;
    private Vector3 mousePosition;
    private Rigidbody2D rigid;
    private bool isMouseDawn;
    private float moveSpeed;
    private Vector3 startPosition;

    [Header("Skins")]
    [SerializeField] private GameObject ImageSkin;
    [SerializeField] private List<Sprite> CharSkins;
    private int NumberCharSkin;

    [Header("Сross Control")]
    private GameObject targetPoint;
    private bool isTrigerActive;
    private bool CrossControl;
    private Vector2 VecUp = new Vector2(0.1f, 0.9f);
    private Vector2 VecDown = new Vector2(-0.1f, -0.9f);
    private bool isCrossControl = true;



    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        gameObject.SetActive(false);
        GameController.GetComponent<GameController>().LoseGame();
    }


    void Update()
    {
        if (isCrossControl == true)
        {
            CharControlСross();  // Управление крестом
        }
        else
        {
            CharControlStick();  // Управление стиком
        }
        CharacterSkins();
        kdBomb.value += 35 * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (isMouseDawn == true)
        {
            rigid.velocity = MoveVector.normalized * moveSpeed;
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }

    }

    void CharControlСross() // Управление крестом
    {
        CrossControl = true;
        if (targetPoint != null)
        {
            Vector2 vec = targetPoint.transform.position - transform.position;
            float vecMag = vec.magnitude;
            if (vecMag < 0.1 && isTrigerActive == true)
            {
                isTrigerActive = false;
            }
        }

        if (isMouseDawn == true && isTrigerActive == false)
        {
            mousePosition = Input.mousePosition - ButtonMoveControl.transform.position;
            float magnitudePos = mousePosition.magnitude;
            magnitudePos = Mathf.Clamp(magnitudePos, 0, 130);
            cursor.transform.position = (mousePosition.normalized * magnitudePos) + ButtonMoveControl.transform.position;
            moveSpeed = ((magnitudePos / 130) * (maxMoveSpped - minMoveSpped)) + minMoveSpped;
            if (Mathf.Abs(mousePosition.normalized.x) >= Mathf.Abs(mousePosition.normalized.y))
            {
                float PosX = (1 / (Mathf.Abs(mousePosition.normalized.x))) * mousePosition.normalized.x;
                if (PosX > 0)
                {
                    NumberCharSkin = 0;
                    MoveVector = Vector2.right;
                }
                else
                {
                    NumberCharSkin = 1;
                    MoveVector = Vector2.left;
                }
            }
            else
            {
                float PosY = (1 / (Mathf.Abs(mousePosition.normalized.y))) * mousePosition.normalized.y;
                if (PosY < 0)
                {
                    NumberCharSkin = 2;
                    MoveVector = VecDown;
                }
                else
                {
                    NumberCharSkin = 3;
                    MoveVector = VecUp;
                }
            }
        }
        else
        {
            cursor.transform.position = ButtonMoveControl.transform.position;
        }

    }

    void CharControlStick() // Управление стиком
    {
        if (isMouseDawn == true)
        {
           mousePosition = Input.mousePosition - ButtonMoveControl.transform.position;
            float magnitudePos = mousePosition.magnitude;
            magnitudePos = Mathf.Clamp(magnitudePos, 0, 130);
            cursor.transform.position = (mousePosition.normalized * magnitudePos) + ButtonMoveControl.transform.position;
            moveSpeed = ((magnitudePos / 130) * (maxMoveSpped - minMoveSpped)) + minMoveSpped;
            if (Mathf.Abs(mousePosition.normalized.x) >= Mathf.Abs(mousePosition.normalized.y))
            {
                float PosX = (1 / (Mathf.Abs(mousePosition.normalized.x))) * mousePosition.normalized.x;
                if (PosX > 0)
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
                float PosY = (1 / (Mathf.Abs(mousePosition.normalized.y))) * mousePosition.normalized.y;
                if (PosY < 0)
                {
                    NumberCharSkin = 2;
                }
                else
                {
                    NumberCharSkin = 3;
                }
            }
            MoveVector = mousePosition;
        }
        else
        {
            cursor.transform.position = ButtonMoveControl.transform.position;
        }
    }

    public void MouseDown()
    {
        isMouseDawn = true;
    }

    public void MouseUp()
    {
        isMouseDawn = false;
    }

    public void StartPosition() // в стартовые условия при нажатии кнопки старк
    {
        transform.position = startPosition;
        kdBomb.value = 100;
        gameObject.SetActive(true);
    }

    void CharacterSkins()  // Меняю спрайт 
    {
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

    }

    public void PlantingBomb()  // Установка бомбы
    {
        if (kdBomb.value == 100)
        {
            kdBomb.value = 0;
            GameController.GetComponent<GameController>().Bomb(Instantiate(Bomb, transform.position, Quaternion.identity));
        }
    }

    public void KillDogs()
    {
        kdBomb.value += 50;
    } // бонус за собачку 

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bomb")
        {
            gameObject.SetActive(false);
            GameController.GetComponent<GameController>().LoseGame();
        }

        if (collision.tag == "TrigerPoint" && CrossControl == true)  // Управление крестом
        {
            targetPoint = collision.gameObject;
            MoveVector = collision.transform.position - transform.position;
            isTrigerActive = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            gameObject.SetActive(false);
            GameController.GetComponent<GameController>().LoseGame();
        }
    }

    public void CrossControlON()
    {
        isCrossControl = true;
    }

    public void StikControllON()
    {
        isCrossControl = false;
    }
}
