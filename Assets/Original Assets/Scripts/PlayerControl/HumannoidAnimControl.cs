using UnityEngine;

public partial class HumannoidAnimControl : MonoBehaviour
{
  [SerializeField] Transform rightHand;
  public Transform RightHand { get { return rightHand; } }
  [SerializeField] Transform leftHand;
  public Transform LeftHand { get { return leftHand; } }
}