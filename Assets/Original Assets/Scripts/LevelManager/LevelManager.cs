using UnityEngine;

public partial class LevelManager : MonoBehaviour
{
  public static LevelManager Instance { get; private set; }

  void Start()
  {
    Instance = this;

    SubscribeTouchEvent();
  }

  void OnDestroy()
  {
    UnsubscribeTouchEvent();
  }
}