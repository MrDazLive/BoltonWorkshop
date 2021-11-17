using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Magazine))]
public class Sniper : MonoBehaviour, IWeapon
{
  private Magazine m_magazine;

  private float m_fireRate = 0.8f;
  private bool m_canFire = true;

  public void OnTriggerDown(eTrigger triggerType)
  {
    if (triggerType == eTrigger.PRIMARY)
      if (m_canFire)
        m_magazine.Fire(Shoot);
  }

  public void OnTriggerHold(eTrigger triggerType) { }

  public void OnTriggerRelease(eTrigger triggerType) { }

  // Start is called before the first frame update
  void Start()
  {
    m_magazine = GetComponent<Magazine>();
  }

  private IEnumerator CoolDownAsync()
  {
    m_canFire = false;
    yield return new WaitForSeconds(m_fireRate);
    m_canFire = true;
  }

  private void Shoot()
  {
    var player = gameObject.transform.parent;
    var hitObject = Bullet.Shoot(player.position, player.forward, (obj) => obj.TryGetComponent(out Target _));
    hitObject?.GetComponent<Target>().DealDamage(1.0f);

    StartCoroutine(nameof(CoolDownAsync));
  }
}
