using Unity.Mathematics;
using UnityEngine;

public partial class PlayerControl : MonoBehaviour
{
  [Header("Player Block Movement Dependencies")]
  [SerializeField] StackControl stackControl;
  [SerializeField] Transform player;
  [SerializeField] CurvedPath curvedPath;
  [Header("Datas")]
  float _lastFrameVelocity;

  private void Start()
  {
    _lastFrameVelocity = 0;
    curvedPath.BakingCurvedPath();
    stackControl.UpdateCurvedEndPosition(1);

    stackControl.AddCoffeeCupsBy(1);
  }

  void FixedUpdate()
  {
    CalculateCurvedEnd();
    CalculateCurvedPosCups();

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
    var v = lastFrameVelocity + accelerate * Time.fixedDeltaTime;
    if (math.abs(v) > LevelManager.Instance.PlayerMaxSpeed)
      v = math.sign(v) * LevelManager.Instance.PlayerMaxSpeed;
    if (!isMoveForward && math.sign(v) != math.sign(lastFrameVelocity))
    {
      v = 0;
      // player stop moving event
    }
    return v;
  }

  float CalculatePositionBy(float v, float lastFramePosition)
  {
    var x = lastFramePosition + v * Time.fixedDeltaTime;
    return x;
  }

  public void ForwardMoving(bool _isMoveForward = true)
  {
    var currentVelocity = CalculateVelocityBy(_lastFrameVelocity, _isMoveForward);
    var x = CalculatePositionBy(currentVelocity, transform.position.x);

    transform.position = new Vector3(x, transform.position.y, transform.position.z);

    GetComponentInChildren<Animator>().speed = math.clamp(math.abs(currentVelocity), 0.0f, 1);
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

  public void CalculateCurvedPosCups()
  {
    var curvedStartPos = curvedPath.GetCurvedStartPos();
    var curvedEnd = curvedPath.GetCurvedEnd();
    var verticalEndPos = new Vector3(
      curvedStartPos.x,
      curvedStartPos.y + curvedPath.TotalLength,
      curvedStartPos.z
    );
    var dirToVerticalEnd = verticalEndPos - curvedEnd.position;
    if (curvedEnd.TryGetComponent<PhysicMovement>(out var curvedPhysic))
    {
      curvedPhysic.AddForce(12 * dirToVerticalEnd);
      curvedEnd.transform.position = curvedPhysic.UpdatePosition(curvedEnd.transform.position);
    }

    stackControl.UpdateCurvedPosCups();
  }

  public void CalculateCurvedEnd()
  {
    var curvedEnd = curvedPath.GetCurvedEnd();
    if (!curvedEnd.TryGetComponent<PhysicMovement>(out var curvedPhysic)) return;

    curvedPhysic.DecayVelocity();
    curvedEnd.transform.position = curvedPhysic.UpdatePosition(curvedEnd.transform.position);
  }
}


