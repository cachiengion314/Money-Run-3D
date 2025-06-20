using System;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{
  [Header("Touch Control System")]
  bool _isUserScreenTouching;
  public bool IsUserScreenTouching { get { return _isUserScreenTouching; } }

  void SubscribeTouchEvent()
  {
    LeanTouch.OnGesture += OnGesture;
    LeanTouch.OnFingerInactive += OnFingerInactive;
  }

  void UnsubscribeTouchEvent()
  {
    LeanTouch.OnGesture -= OnGesture;
    LeanTouch.OnFingerInactive -= OnFingerInactive;
  }

  void OnGesture(List<LeanFinger> list)
  {
    if (list.Count > 1) return;
    _isUserScreenTouching = true;

    var finger = list[0];
    TouchRun(finger);
  }

  private void OnFingerInactive(LeanFinger finger)
  {
    _isUserScreenTouching = false;

    TouchStop();
  }

  public void TouchRun(LeanFinger finger)
  {
    if (GameManager.Instance.GameState != GameState.Gameplay) return;

    var player = GameManager.Instance.PlayerBlockMovement;
    if (player.IsHit) { return; }

    player.GetComponentInChildren<Animator>().SetBool("IsIdle", false);

    var curentTouchPos = Camera.main.ScreenToViewportPoint(finger.ScreenPosition);
    player.LeftRightMovement(curentTouchPos);
  }

  public void TouchStop()
  {
    if (GameManager.Instance.GameState != GameState.Gameplay) return;

    var player = GameManager.Instance.PlayerBlockMovement;
    if (player.IsHit) { return; }
  }
}