using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  [Header("Data")]
  Vector3 _playerOffset;
  [SerializeField] private float xOffset;
  [SerializeField] private float yOffset;
  [SerializeField] private float zOffset;

  [SerializeField] private Transform followPoint;
  [SerializeField] private PlayerControl playerControl;

  void Start()
  {
    _playerOffset
      = transform.position - GameManager.Instance.PlayerControl.transform.position;
  }

  void Update()
  {
    var curvedPos = GameManager.Instance.FindCurvedPosAt(
      GameManager.Instance.PlayerControl.transform.position
    );

    transform.position
      = new Vector3(
        GameManager.Instance.PlayerControl.transform.position.x + _playerOffset.x,
        GameManager.Instance.PlayerControl.transform.position.y + _playerOffset.y,
        curvedPos.z + _playerOffset.z
      );
  }
}
