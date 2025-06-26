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
    SetupCurrentLevel();
  }

  public void SetupCurrentLevel()
  {
    GameManager.Instance.SetGameState(GameState.Pause);
    GameManager.Instance.levelNo = GameManager.Instance.CurrentLevelIndex;
    if (isSelectedMatchedCurrentLevel)
      GameManager.Instance.levelNo = levelSelected - 1;

    LoadLevelFrom(GameManager.Instance.levelNo + 1);
    SpawnLevelObjsFrom(levelInformation);

    GameManager.Instance.levelNoDisplay.text
      = string.Format("Level " + "{0:0}", GameManager.Instance.levelNo + 1);
  }

  public void CheckLoseCondition()
  {
    if (GameManager.Instance.GameState == GameState.Pause) return;
    if (
      GameManager.Instance.PlayerBlockMovement
        .GetComponent<StackControl>().CoffeeCupAmount <= 0
    )
    {
      GameManager.Instance.SetGameState(GameState.Pause);

      GameManager.Instance.PlayerBlockMovement
        .GetComponentInChildren<Animator>()
        .SetTrigger("Dead");
      GameManager.Instance.PlayerBlockMovement
        .GetComponent<Collider>().enabled = false;

      DOVirtual.DelayedCall(
        1.25f,
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