using Unity.Mathematics;
using UnityEngine;

public class PhysicMovement : MonoBehaviour
{
  [Header("Settings")]
  [Tooltip("Enable if you want movement of the obj itself is auto controlled by component's algorithms")]
  [SerializeField] bool enableAutoUpdate;
  [SerializeField] bool useGravity;
  [SerializeField] float3 gravity;
  [SerializeField][Range(-50, 50)] float decayCoefficient = -1;
  [SerializeField][Range(.01f, 100)] float mass = 1;
  public float Mass { get { return mass; } }
  public float3 Accelerate = 0;
  float3 _lastFrameVelocity = 0;

  void FixedUpdate()
  {
    if (!enableAutoUpdate) return;

    if (useGravity) Accelerate += gravity;
    if (_lastFrameVelocity.Equals(0))
      _lastFrameVelocity = transform.position;
    transform.position = UpdatePosition(_lastFrameVelocity);
  }

  float3 CalculateAccelerateBy(float3 lastFrameVelocity, bool isMoveForward)
  {
    if (!isMoveForward)
      return Mass * -1 * Accelerate * lastFrameVelocity;
    return Mass * Accelerate;
  }

  float3 CalculateVelocityBy(float3 lastFrameVelocity, bool isMoveForward)
  {
    var accelerate = CalculateAccelerateBy(lastFrameVelocity, isMoveForward);
    var v = lastFrameVelocity + accelerate * Time.deltaTime;
    return v;
  }

  float3 CalculatePositionBy(float3 v, float3 lastFramePosition)
  {
    var x = lastFramePosition + v * Time.deltaTime;
    return x;
  }

  /// <summary>
  /// Manual controlling obj movement for an advanced use case
  /// </summary>
  /// <param name="lastFramePosition"></param>
  /// <param name="_isMoveForward"></param>
  /// <returns></returns>
  public float3 UpdatePosition(float3 lastFramePosition, bool _isMoveForward = true)
  {
    var currentVelocity = CalculateVelocityBy(_lastFrameVelocity, _isMoveForward);
    var position = CalculatePositionBy(currentVelocity, lastFramePosition);
    _lastFrameVelocity = currentVelocity;
    return position;
  }

  public void AddForce(float3 accelerate)
  {
    Accelerate = accelerate;
  }

  public void ApplyVelocity(float3 velocity)
  {
    _lastFrameVelocity = velocity;
  }

  public void DecayVelocity()
  {
    var FPS = 1 / Time.deltaTime;
    var r = 1 - math.pow(math.E, decayCoefficient / FPS);
    _lastFrameVelocity *= 1 - r;
    if (math.lengthsq(_lastFrameVelocity) < .01f)
      _lastFrameVelocity = 0;
  }

  public void SetAutoUpdate(bool shouldAuto)
  {
    enableAutoUpdate = shouldAuto;
  }
}