using Unity.Mathematics;
using UnityEngine;

public class PhysicMovement : MonoBehaviour
{
  [SerializeField][Range(-50, 50)] float decayCoefficient = -9;
  [SerializeField] float mass = 1;
  public float Mass { get { return mass; } }
  public float3 Accelerate = 0;
  float3 _lastFrameVelocity = 0;

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
    if (math.length(v) > GameManager.Instance.PlayerMaxSpeed)
      v = math.normalize(v) * GameManager.Instance.PlayerMaxSpeed;
    if (!isMoveForward && !math.normalize(v).Equals(math.normalize(lastFrameVelocity)))
    {
      v = 0;
    }
    return v;
  }

  float3 CalculatePositionBy(float3 v, float3 lastFramePosition)
  {
    var x = lastFramePosition + v * Time.deltaTime;
    return x;
  }

  public float3 UpdatePosition(float3 lastFramePosition, bool _isMoveForward = true)
  {
    var currentVelocity = CalculateVelocityBy(_lastFrameVelocity, _isMoveForward);
    var position = CalculatePositionBy(currentVelocity, lastFramePosition);
    _lastFrameVelocity = currentVelocity;
    return position;
  }

  public void AddForce(
    float3 accelerate
  )
  {
    Accelerate = accelerate;
  }

  public void DecayVelocity()
  {
    var FPS = 1 / Time.deltaTime;
    var r = 1 - math.pow(math.E, decayCoefficient / FPS);
    _lastFrameVelocity *= 1 - r;
  }
}