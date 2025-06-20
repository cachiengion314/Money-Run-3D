using UnityEngine;

public partial class GameManager : MonoBehaviour
{
  [Header("Curved path")]
  public int resolution = 100;
  public Color gizmoColor = Color.green;
  [SerializeField] Transform[] points;
  [SerializeField] Transform[] controls;
  float _totalLength;

  void BakingCurvedPath()
  {
    _totalLength = (GetCurvedEndPos() - GetCurvedStartPos()).magnitude;
  }

  /// <summary>
  /// visualization
  /// </summary>
  void OnDrawGizmos()
  {
    Gizmos.color = gizmoColor;
    Vector3 prevPoint = Vector3.zero;
    bool first = true;

    for (int i = 0; i <= resolution; i++)
    {
      float t = i / (float)resolution;
      if (points.Length < 2) return;
      if (controls.Length < 1) return;

      Vector3 B01 = Lerp(points[0].position, controls[0].position, t);
      Vector3 B02 = Lerp(B01, points[1].position, t);

      Vector3 point = transform.position + B02;
      if (!first)
        Gizmos.DrawLine(prevPoint, point);

      prevPoint = point;
      first = false;
    }
  }

  Vector3 Lerp(Vector3 start, Vector3 end, float t)
  {
    return (1 - t) * start + t * end;
  }

  /// <summary>
  /// t running from 0 to 1
  /// </summary>
  /// <param name="t"></param>
  /// <returns></returns>
  public Vector3 FindCurvedPosAt(float t)
  {
    Vector3 B01 = Lerp(points[0].position, controls[0].position, t);
    Vector3 B02 = Lerp(B01, points[1].position, t);

    Vector3 point = transform.position + B02;
    return point;
  }

  public Vector3 GetCurvedStartPos()
  {
    return points[0].position;
  }

  public Vector3 GetCurvedEndPos()
  {
    return points[1].position;
  }

  /// <summary>
  /// this function will snap near position into curved path postion
  /// </summary>
  /// <param name="nearPosition"></param>
  /// <returns></returns>
  public Vector3 FindCurvedPosAt(Vector3 nearPosition)
  {
    var startPos = GetCurvedStartPos();
    var currLength = (nearPosition - startPos).magnitude;
    var t = currLength / _totalLength;
    var curvedPos = FindCurvedPosAt(t);

    return curvedPos;
  }
}