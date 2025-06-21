using Unity.Mathematics;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{
  [Header("Spawn Item Manager")]
  [SerializeField] GameObject coffeeCupPref;

  public GameObject SpawnCoffeeCupAt(float3 pos)
  {
    var obj = Instantiate(coffeeCupPref, pos, coffeeCupPref.transform.rotation);
    return obj;
  }

  public GameObject SpawnCoffeeCupAt(Transform parent)
  {
    var obj = Instantiate(coffeeCupPref, parent);
    return obj;
  }
}