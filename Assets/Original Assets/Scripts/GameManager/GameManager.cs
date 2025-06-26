using Dreamteck.Splines;
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
  public GameObject powerDisplay;
  public Text levelNoDisplay;
  public Image secondStar;
  public Image thirdStar;
  public Image fillDistanceBar;
  public int levelNo;
  public float totalNumberOfStack;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      InitUserData();
    }
    else Destroy(gameObject);
  }

  private void Update()
  {
    LetUsStartTheGame();
  }

  void LetUsStartTheGame()
  {
    if (
      Input.GetMouseButtonDown(0) &&
      canvas.transform.GetChild(0).gameObject.activeSelf &&
      !EventSystem.current.IsPointerOverGameObject(0)
    )
    {
      canvas.transform.GetChild(0).gameObject.SetActive(false);
      canvas.transform.GetChild(1).gameObject.SetActive(true);

      LevelManager.Instance.PlayerControl.transform.rotation
        = Quaternion.Euler(0, -90, 0);
      LevelManager.Instance.PlayerControl
        .GetComponentInChildren<Animator>().SetBool("IsIdle", false);

      SetGameState(GameState.Gameplay);
    }
  }

  public void ShowWinScreenPopup()
  {
    canvas.transform.GetChild(2).gameObject.SetActive(true);
  }

  public void ShowLoseScreenPopup()
  {
    canvas.transform.GetChild(3).gameObject.SetActive(true);
  }

  public void SetGameState(GameState gameState)
  {
    _gameState = gameState;
  }
}
