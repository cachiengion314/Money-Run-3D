using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum GameState
{
  Pause,
  Gameplay,
}

public partial class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }

  [Header("Dependencies")]
  GameState _gameState;
  public GameState GameState { get { return _gameState; } }
  [SerializeField] CurvedPath curvedPath;
  public CurvedPath CurvedPath { get { return curvedPath; } }
  [SerializeField] SplineFollower splineFollower;
  public SplineFollower SplineFollower { get { return splineFollower; } }
  public GameObject splineComputer;
  public SplineComputer spline;

  public static float totalGemAmount;
  public float currentGemCollected = 0f;

  public float gemWithIncome;
  public float gemWithStackMoney;
  public float gemByCompleteMap;
  public float gemByStar;

  public bool gainPower = false;

  public GameObject player;

  public GameObject canvas;
  public bool hasWon = false;
  public bool gameOver = false;
  public bool startLosing = true;
  public GameObject powerDisplay;
  public Text powerDisplayText;
  public Text levelNoDisplay;

  public Image secondStar;
  public Image thirdStar;

  public Image fillDistanceBar;
  public GameObject cameraFollowPoint;

  public int levelNo;

  public float totalNumberOfStack;

  public GameObject stackPos;
  public float increamentBlockSpeed;

  public GameObject[] dataObstacle;

  public List<GameObject> dataLevels;


  private void Awake()
  {
    Instance = this;

    Time.timeScale = 0;
    SetGameState(GameState.Pause);
    levelNo = PlayerPrefs.GetInt("Level_Number", 0);
    if (levelNo >= dataLevels.Count)
    {
      levelNo = 0;
    }
    totalGemAmount = PlayerPrefs.GetFloat("Total_Gem", 0);

    InitUserData();
  }

  // Start is called before the first frame update
  void Start()
  {
    levelNoDisplay.text = string.Format("Level " + "{0:0}", levelNo + 1);

    ObstacleSpawn();

    //Setup money stack value through upgrade
    foreach (var stack in GameObject.FindGameObjectsWithTag("Uncollected"))
    {
      stack
        .GetComponent<MoneyStackValue>()
        .moneyValue
        += stack
        .GetComponent<MoneyStackValue>().moneyValue * MenuManager.instance.moneyStackMod;
    }

    curvedPath.BakingCurvedPath();
  }

  private void Update()
  {
    LetUsStartTheGame();

    WinScreenPopup();

    LoseCondition();
    LoseScreenPopup();

    currentGemCollected = gemWithStackMoney + gemByCompleteMap + gemByStar + gemWithIncome;
  }

  void LetUsStartTheGame()
  {
    if (
      Input.GetMouseButtonDown(0) &&
      canvas.transform.GetChild(0).gameObject.activeSelf &&
      !EventSystem.current.IsPointerOverGameObject(0)
    )
    {
      Time.timeScale = 1.0f;
      canvas.transform.GetChild(0).gameObject.SetActive(false);
      canvas.transform.GetChild(1).gameObject.SetActive(true);

      player.transform.rotation = Quaternion.Euler(0, -90, 0);

      SetGameState(GameState.Gameplay);
    }
  }

  void WinScreenPopup()
  {
    if (hasWon)
    {
      canvas.transform.GetChild(2).gameObject.SetActive(true);
    }
  }

  void LoseScreenPopup()
  {
    if (gameOver)
    {
      canvas.transform.GetChild(3).gameObject.SetActive(true);
    }
  }

  void LoseCondition()
  {
    if (player.GetComponent<PlayerPowerController>().moneyAmount < 0 && startLosing)
    {
      startLosing = false;
      StartCoroutine(LoseScreenDelay());
    }
  }

  IEnumerator LoseScreenDelay()
  {
    yield return new WaitForSeconds(1.5f);
    gameOver = true;
  }

  void ObstacleSpawn()
  {
    Instantiate(dataLevels[levelNo], dataLevels[levelNo].transform.position, dataLevels[levelNo].transform.rotation);
    StartCoroutine(DelayCountingStack());
  }

  IEnumerator DelayCountingStack()
  {
    yield return new WaitForSecondsRealtime(0.5f);
    totalNumberOfStack = GameObject.FindGameObjectsWithTag("Uncollected").Length;
  }

  public void SetGameState(GameState gameState)
  {
    _gameState = gameState;
  }
}
