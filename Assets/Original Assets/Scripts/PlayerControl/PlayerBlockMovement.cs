using Unity.Mathematics;
using UnityEngine;

public class PlayerBlockMovement : MonoBehaviour
{
  public static PlayerBlockMovement instance;
  [Header("Player Block Movement Dependencies")]
  [SerializeField] StackIncrease cupStack;
  [SerializeField] Transform player;
  [SerializeField] CurvedPath curvedPath;
  [Header("Datas")]
  float _lastFrameVelocity;
  public bool IsHit;

  private void Start()
  {
    instance = this;
    _lastFrameVelocity = 0;
    curvedPath.BakingCurvedPath();
  }

  void Update()
  {
    CalculateCurvedCups();

    if (GameManager.Instance.GameState != GameState.Gameplay) return;
    if (IsHit) return;

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
    // thats mean: minimum new value + new range
    return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
  }

  float CalculateAccelerateBy(float lastFrameVelocity, bool isMoveForward)
  {
    if (isMoveForward)
      return -3;
    return -3 * lastFrameVelocity;
  }

  float CalculateVelocityBy(float lastFrameVelocity, bool isMoveForward)
  {
    var accelerate = CalculateAccelerateBy(lastFrameVelocity, isMoveForward);
    var v = lastFrameVelocity + accelerate * Time.deltaTime;
    if (math.abs(v) > GameManager.Instance.PlayerMaxSpeed)
      v = math.sign(v) * GameManager.Instance.PlayerMaxSpeed;
    if (!isMoveForward && math.sign(v) != math.sign(lastFrameVelocity))
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
    var centerCurvedPos = LevelManager.Instance.CurvedPath.FindCurvedPosAt(transform.position);

    var currentPosX = Mathf.Clamp(currentTouchPos.x, .25f, .75f);
    transform.position
      = new Vector3(
        transform.position.x,
        transform.position.y,
        MapRange(currentPosX, .25f, .75f, centerCurvedPos.z - .80f, centerCurvedPos.z + .85f)
      );
  }

  public void CalculateCurvedCups()
  {
    var curvedStartPos = curvedPath.GetCurvedStartPos();
    var curvedEnd = curvedPath.GetCurvedEnd();
    var verticalEndPos = new Vector3(
      curvedStartPos.x,
      curvedStartPos.y + curvedPath.TotalLength,
      curvedStartPos.z
    );
    var dirToVerticalEnd = verticalEndPos - curvedEnd.position;
    curvedEnd.transform.position += 7 * Time.deltaTime * dirToVerticalEnd;

    cupStack.UpdateCurvedPosCups();
  }
}


