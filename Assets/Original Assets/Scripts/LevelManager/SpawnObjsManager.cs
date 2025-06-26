using Unity.Mathematics;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{
  [Header("Spawn Objs Manager")]
  [SerializeField] GameObject coffeeCupPref;
  [SerializeField] GameObject barrierBarPref;
  [SerializeField] GameObject helmetPref;
  [SerializeField] GameObject powerPortal;
  [SerializeField] GameObject dollarEffect;
  [SerializeField] GameObject hittingEfx;
  [SerializeField] ParticleSystem powerUpEfx;

  public ParticleSystem SpawnPowerUpEfxAt(float3 pos)
  {
    var efx = Instantiate(powerUpEfx, pos, powerUpEfx.transform.rotation);
    return efx;
  }

  public GameObject SpawnHittingEfxAt(float3 pos)
  {
    var efx = Instantiate(hittingEfx, pos, hittingEfx.transform.rotation);
    return efx;
  }

  public GameObject SpawnDollarEffectAt(float3 pos)
  {
    var efx = Instantiate(dollarEffect, pos, dollarEffect.transform.rotation);
    return efx;
  }

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

  public GameObject SpawnBarrierBarAt(float3 pos)
  {
    var obj = Instantiate(barrierBarPref, pos, barrierBarPref.transform.rotation);
    return obj;
  }

  public GameObject SpawnHelmetAt(float3 pos)
  {
    var obj = Instantiate(helmetPref, pos, helmetPref.transform.rotation);
    return obj;
  }

  public GameObject SpawnPowerPortal(float3 pos)
  {
    var obj = Instantiate(powerPortal, pos, powerPortal.transform.rotation);
    return obj;
  }
}