using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  [Header("Data")]
  Vector3 _playerOffset;

  void Start()
  {
    _playerOffset = transform.position - LevelManager.Instance.PlayerControl.transform.position;
  }

  void Update()
  {
    var curvedPos = LevelManager.Instance.CurvedPath.FindCurvedPosAt(
      LevelManager.Instance.PlayerControl.transform.position
    );

    transform.position
      = new Vector3(
        LevelManager.Instance.PlayerControl.transform.position.x + _playerOffset.x,
        LevelManager.Instance.PlayerControl.transform.position.y + _playerOffset.y,
        curvedPos.z + _playerOffset.z
      );
  }
}
