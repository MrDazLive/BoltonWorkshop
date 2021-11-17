using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Magazine))]
public class Weapon : MonoBehaviour
{
  private Magazine m_magazine;

  [SerializeField]
  private eWeapon m_weaponType;

  [SerializeField]
  private float m_bulletDamage = 1.0f;

  [SerializeField]
  private float m_fireRate = 0.8f;

  private bool m_shooting = false;
  private bool m_canFire = true;
  private float m_spread = 0.1f;
  private int m_pelletCount = 8;
  private float m_spinUpTime = 5.0f;

  private float m_minFireRate = 0.4f;
  private float m_maxFireRate = 0.1f;
  private float m_secondaryFireRate = 0.2f;

  // Start is called before the first frame update
  void Start()
  {
    m_magazine = GetComponent<Magazine>();
  }

  // Update is called once per frame
  void Update()
  {
    if(m_weaponType == eWeapon.MINIGUN)
      if (!m_shooting || m_magazine.IsReloading)
        m_fireRate = Mathf.Min(m_minFireRate, m_fireRate + (Time.deltaTime / 2));

    if (Input.GetMouseButtonDown(0))
    {
      m_shooting = true;
      switch(m_weaponType)
      {
        case eWeapon.SNIPER:
        case eWeapon.SHOTGUN:
          if(m_canFire)
            SingleShot();
          break;
        default:
          break;
      }
    }

    if (Input.GetMouseButton(0))
    {
      if (m_weaponType == eWeapon.MINIGUN)
      {
        m_fireRate -= Time.deltaTime / m_spinUpTime;
        m_fireRate = Mathf.Max(m_maxFireRate, m_fireRate);
        if (m_canFire)
          SingleShot();
      }
    }

    if (Input.GetMouseButtonUp(0))
    {
      m_shooting = false;
    }

    if (Input.GetMouseButtonDown(1))
      if(m_canFire && m_weaponType == eWeapon.SHOTGUN)
        StartCoroutine(nameof(DoubleShot));
  }

  private IEnumerator CoolDownAsync()
  {
    m_canFire = false;
    yield return new WaitForSeconds(m_fireRate);
    m_canFire = true;
  }

  private Vector3 DirectShot(Transform transform)
    => transform.forward;

  private Vector3 SpreadShot(Transform transform)
  {
    var direction = transform.up * Random.Range(-1.0f, 1.0f);
    direction += transform.right * Random.Range(-1.0f, 1.0f);
    direction = direction.normalized * Random.Range(0.0f, m_spread);
    direction += transform.forward;
    return direction;
  }

  private void Shoot()
  {
    int shrapnelCount = m_weaponType == eWeapon.SHOTGUN ? m_pelletCount : 1;

    for (int i = 0; i < shrapnelCount; ++i)
    {
      var player = gameObject.transform.parent;
      var direction = m_weaponType == eWeapon.SHOTGUN ? SpreadShot(player) : DirectShot(player);
      var hitObject = Bullet.Shoot(player.position, direction, (obj) => obj.TryGetComponent(out Target _));
      hitObject?.GetComponent<Target>().DealDamage(m_bulletDamage);
    }
  }

  private void SingleShot()
  {
    StartCoroutine(nameof(CoolDownAsync));
    m_magazine.Fire(Shoot);
  }

  private IEnumerator DoubleShot()
  {
    StartCoroutine(nameof(CoolDownAsync));
    m_magazine.Fire(Shoot);
    yield return new WaitForSeconds(m_secondaryFireRate);
    m_magazine.Fire(Shoot);
  }
}
