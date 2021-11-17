using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Magazine))]
public class Minigun : MonoBehaviour, IWeapon
{
  private Magazine m_magazine;

  private float m_fireRate = 0.0f;
  private float m_minFireRate = 0.4f;
  private float m_maxFireRate = 0.1f;
  private float m_spinUpTime = 5.0f;

  private bool m_canFire = true;
  private bool m_shooting = false;

  public void OnTriggerDown(eTrigger triggerType)
  {
    m_shooting = true;
  }

  public void OnTriggerHold(eTrigger triggerType)
  {
    if (triggerType == eTrigger.PRIMARY)
    {
      m_fireRate -= Time.deltaTime / m_spinUpTime;
      m_fireRate = Mathf.Max(m_maxFireRate, m_fireRate);
      if (m_canFire)
        m_magazine.Fire(Shoot);
    }
  }

  public void OnTriggerRelease(eTrigger triggerType)
  {
    m_shooting = false;
  }

  // Start is called before the first frame update
  void Start()
  {
    m_magazine = GetComponent<Magazine>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!m_shooting || m_magazine.IsReloading)
      m_fireRate = Mathf.Min(m_minFireRate, m_fireRate + (Time.deltaTime / 2));
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
    hitObject?.GetComponent<Target>().DealDamage(0.08f);

    StartCoroutine(nameof(CoolDownAsync));
  }
}
