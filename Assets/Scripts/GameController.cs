using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject ButtonMoveControl;
    [SerializeField] private GameObject ButtonBomb;
    [SerializeField] private List<GameObject> Enemys;
    [SerializeField] private List<GameObject> Bombs;
    [SerializeField] private List<GameObject> SpawnPos;
    [SerializeField] private List<GameObject> StartSpawnPos;
    [SerializeField] private int timeSpawnMax;
    [SerializeField] private int timeSpawnMin;
    [SerializeField] private int quantityBeforeChange;

    private int quantityChange;
    private int counterSpawnDogs;
    private int timeSpawn;
    private int countDogs;
    private bool isSpawn = true;
    private bool isGame;
    private int record;

    [Header("Canvas")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Text check;
    [SerializeField] private Text RecordText;
    [SerializeField] private GameObject crossButton;
    [SerializeField] private GameObject stickButton;

    void Start()
    {
        LoadRecord();
        timeSpawn = timeSpawnMax;
    }


    void Update()
    {
        if (isSpawn == true && isGame == true)
        {
            Invoke("Spawner", timeSpawn);
            isSpawn = false;
        }
        HZ();
        Bombs.RemoveAll(x => x == null);
    }

    void StartSpawner() // тоже крафтит собак но в ограниченной области 
    {
        int random = Random.Range(0, StartSpawnPos.Count);
        Enemys.Add(Instantiate(enemy, StartSpawnPos[random].transform.position, Quaternion.identity));  
        
    }
    public void Spawner() // крафтит собак 
    {
        if (isGame == true)
        {
            int random = Random.Range(0, SpawnPos.Count);
            Enemys.Add(Instantiate(enemy, SpawnPos[random].transform.position, Quaternion.identity));
            speedSpawn();
            isSpawn = true;
        }
    }

    void speedSpawn()  // ускор€ет по€вление собак 
    {
        counterSpawnDogs += 1;
        if (counterSpawnDogs == quantityChange && timeSpawn > timeSpawnMin)
        {
            timeSpawn -= 1;
            counterSpawnDogs = 0;
            quantityChange += 1;
        }
    }

    void HZ()  // чистит пустые €чейки и считает уибтых собак "не знаю как назвать ..."
    {
        for (int i = 0; i < Enemys.Count; i++)
        {
            if (Enemys[i] == null)
            {
                Enemys.RemoveAt(i);
                countDogs += 1;
                check.text = ""  + countDogs;
                character.GetComponent<CharController>().KillDogs();
            }
        }
    }

    public void Bomb(GameObject bomb)
    {
        Bombs.Add(bomb);
    } // что бы удалить бомбы 

    public void StartGame() // кнопка старта 
    {
        for (int i = 0; i < Enemys.Count; i++)
        {
            Destroy(Enemys[i]);
        }
        Enemys.Clear();

        for (int i = 0; i < Bombs.Count; i++)
        {
            Destroy(Bombs[i]);
        }
        Bombs.Clear();
        character.GetComponent<CharController>().StartPosition();
        menuPanel.SetActive(false);
        ButtonMoveControl.GetComponent<Button>().interactable = true;
        ButtonBomb.GetComponent<Button>().interactable = true;
        for (int i = 0; i < 3; i++)
        {
            StartSpawner();
        }
        quantityChange = quantityBeforeChange;
        timeSpawn = timeSpawnMax;
        isGame = true;
    }

    public void LoseGame()
    {
        isGame = false;
        if (countDogs > record)
        {
            record = countDogs;
            RecordText.text = "  Record " + record;
        }
        countDogs = 0;
        check.text = "";
        SaveRecord();
        for (int i = 0; i < Enemys.Count; i++)
        {
            Enemys[i].GetComponent<EnemyController>().LoseGame();
        }
        menuPanel.SetActive(true);
        ButtonMoveControl.GetComponent<Button>().interactable = false;
        ButtonBomb.GetComponent<Button>().interactable = false;
    } // после смерти

    public void StickButton()
    {
        character.GetComponent<CharController>().StikControllON();
        stickButton.GetComponent<Button>().interactable = false;
        crossButton.GetComponent<Button>().interactable = true;
    } // кнопка смены управлени€ на стик

    public void CrossButton()
    {
        character.GetComponent<CharController>().CrossControlON();
        crossButton.GetComponent<Button>().interactable = false;
        stickButton.GetComponent<Button>().interactable = true;
    } // кнопка смены управлени€ на крест

    void SaveRecord()
    {
        PlayerPrefs.SetInt("Record", record);
    } 


    void LoadRecord()
    {
        if (PlayerPrefs.HasKey("Record"))
        {
            record = PlayerPrefs.GetInt("Record");
            RecordText.text = "  Record " + record;
        }
    }
}
