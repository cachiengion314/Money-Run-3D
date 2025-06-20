using Unity.Mathematics;
using UnityEngine;

public class PlayerBlockMovement : MonoBehaviour
{
  public static PlayerBlockMovement instance;
  public float PlayerMaxSpeed;
  public bool IsHit;
  [SerializeField] Transform player;
  [Header("Datas")]
  float _lastFrameVelocity;

  private void Start()
  {
    instance = this;
    PlayerMaxSpeed = GameManager.Instance.PlayerMaxSpeed;
    _lastFrameVelocity = 0;
  }

  void Update()
  {
    if (GameManager.Instance.GameState != GameState.Gameplay) return;

    if (LevelManager.Instance.IsUserScreenTouching)
    {
      ForwardMoving(true);
      return;
    }
    ForwardMoving(false);
  }

  float MapRange(float value, float fromMin, float fromMax, float toMin, float toMax)
  {
    // toMin  +
    // (value - fromMin) * length(to) / length(from)
    // minimum new value + new range
    return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
  }

  float CalculateAccelerateBy(float lastFrameVelocity, bool isMoveForward)
  {
    // at v = 0 ==> at + V0 = 0 ==> a = -V0 / t
    if (isMoveForward)
      return -2;
    return -5 * lastFrameVelocity;
  }

  float CalculateVelocityBy(float lastFrameVelocity, bool isMoveForward)
  {
    var accelerate = CalculateAccelerateBy(lastFrameVelocity, isMoveForward);
    var v = lastFrameVelocity + accelerate * Time.deltaTime;
    if (math.abs(v) > GameManager.Instance.PlayerMaxSpeed)
      v = math.sign(v) * GameManager.Instance.PlayerMaxSpeed;
    if (!isMoveForward && math.abs(v) < .05f)
    {
      v = 0;
      // player stop moving event
      var playerAnim = GetComponentInChildren<Animator>();
      playerAnim.SetBool("IsIdle", true);
    }
    return v;
  }

  float CalculatePositionBy(float v, float lastFramePosition)
  {
    var x = lastFramePosition + v * Time.deltaTime;
    return x;
  }

  public void ForwardMoving(bool _isMoveForward = true)
  {
    var currentVelocity = CalculateVelocityBy(_lastFrameVelocity, _isMoveForward);
    var x = CalculatePositionBy(currentVelocity, transform.position.x);

    transform.position = new Vector3(x, transform.position.y, transform.position.z);

    GetComponentInChildren<Animator>().speed = math.clamp(math.abs(currentVelocity), 0, 1);
    _lastFrameVelocity = currentVelocity;
  }

  public void LeftRightMovement(Vector3 currentTouchPos)
  {
    var curvedPos = GameManager.Instance.FindCurvedPosAt(transform.position);

    var currentPosX = Mathf.Clamp(currentTouchPos.x, .25f, .75f);
    transform.position
      = new Vector3(
        transform.position.x,
        transform.position.y,
        MapRange(currentPosX, .25f, .75f, curvedPos.z - .80f, curvedPos.z + .90f)
      );
  }
}


