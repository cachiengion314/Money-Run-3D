using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{
  public static LevelManager Instance { get; private set; }
  [SerializeField] CurvedPath curvedPath;
  public CurvedPath CurvedPath { get { return curvedPath; } }
  [SerializeField] SplineComputer splineComputer;
  [SerializeField] BoxCollider endOfPathCollider;

  void Start()
  {
    Instance = this;

    curvedPath.BakingCurvedPath();
    SubscribeTouchEvent();

    GameManager.Instance.SetGameState(GameState.Pause);
    GameManager.Instance.levelNo = GameManager.Instance.CurrentLevelIndex;
    if (isSelectedMatchedCurrentLevel)
      GameManager.Instance.levelNo = levelSelected - 1;
    LoadLevelFrom(GameManager.Instance.levelNo);

    if (GameManager.Instance.levelNo >= GameManager.Instance.dataLevels.Count)
    {
      GameManager.Instance.levelNo = 0;
      GameManager.Instance.CurrentLevelIndex = 0;
    }
    GameManager.totalGemAmount = PlayerPrefs.GetFloat("Total_Gem", 0);

    GameManager.Instance.levelNoDisplay.text
      = string.Format("Level " + "{0:0}", GameManager.Instance.levelNo + 1);

    SpawnLevelObjsFrom(levelInformation);
  }

  public void CheckLoseCondition()
  {
    if (
      GameManager.Instance.PlayerBlockMovement
        .GetComponent<StackIncrease>().CoffeeCupAmount <= 0
    )
    {
      GameManager.Instance.PlayerBlockMovement
        .GetComponentInChildren<Animator>()
        .SetTrigger("Dead");
      GameManager.Instance.PlayerBlockMovement
        .GetComponent<Collider>().enabled = false;

      GameManager.Instance.SetGameState(GameState.Pause);
      DOVirtual.DelayedCall(
        1.5f,
        () =>
        {
          GameManager.Instance.ShowLoseScreenPopup();
        });
    }
  }

  void OnDestroy()
  {
    UnsubscribeTouchEvent();
  }
}