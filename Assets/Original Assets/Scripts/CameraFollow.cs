using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  [Header("Data")]
  Vector3 _playerOffset;

  void Start()
  {
    _playerOffset
      = transform.position - GameManager.Instance.PlayerBlockMovement.transform.position;
  }

  void Update()
  {
    var curvedPos = LevelManager.Instance.CurvedPath.FindCurvedPosAt(
      GameManager.Instance.PlayerBlockMovement.transform.position
    );

    transform.position
      = new Vector3(
        GameManager.Instance.PlayerBlockMovement.transform.position.x + _playerOffset.x,
        GameManager.Instance.PlayerBlockMovement.transform.position.y + _playerOffset.y,
        curvedPos.z + _playerOffset.z
      );
  }
}
