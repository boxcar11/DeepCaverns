using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMPro.TMP_Text coinText;

    // Image UI objects for keys
    public Image yellowKeyImage;
    public Image orangeKeyImage;
    public Image grayKeyImage;
    public Image whiteKeyImage;
    public Image blackKeyImage;

    public TMP_Text winMessage;
    public CanvasGroup winPanel;


    private int coins;
    private int deathCount;

    private Dictionary<string, bool> Keys = new Dictionary<string, bool>();
    private Dictionary<string, Image> keyImages = new Dictionary<string, Image>();

    private Vector3 checkPointPos;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        coinText.text = "Coins: 0";

        // Add sprites for each key
        keyImages.Add("Yellow", yellowKeyImage);
        keyImages.Add("Orange", orangeKeyImage);
        keyImages.Add("Gray", grayKeyImage);
        keyImages.Add("White", whiteKeyImage);
        keyImages.Add("Black", blackKeyImage);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddCoin()
    {
        coins++;
        coinText.text = "Coins: " + coins.ToString();
    }

    public void AddDeath()
    {
        deathCount++;
    }

    public void AddKey(string name)
    {
        Keys.Add(name, true);
        keyImages[name].enabled = true;
    }

    public bool HasKey(string name)
    {
        return Keys.ContainsKey(name);
    }

    public void SetCheckpointPos(Vector3 p)
    {
        checkPointPos = p;
    }

    public Vector3 GetCheckpointPos()
    {
        return checkPointPos;
    }

    public void Win()
    {
        winMessage.text = "Coins: " + coins + "\n" + "Deaths: " + deathCount;
        winPanel.alpha = 1;
    }
}
