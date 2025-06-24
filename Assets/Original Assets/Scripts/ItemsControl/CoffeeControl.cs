using UnityEngine;

public class CoffeeControl : MonoBehaviour, ICollectable
{
  [SerializeField][Range(0, 10000)] int value;

  public int GetValue()
  {
    return value;
  }
}