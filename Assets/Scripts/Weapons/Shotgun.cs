using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Magazine))]
public class Shotgun : MonoBehaviour
{
  private Magazine m_magazine;

  private float m_fireRate = 0.6f;
  private float m_secondaryDelay = 0.2f;
  private float m_spread = 0.1f;
  private int m_pelletCount = 8;

  private bool m_canFire = true;

  // Start is called before the first frame update
  void Start()
  {
    m_magazine = GetComponent<Magazine>();
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      SingleShot();
    }

    if (Input.GetMouseButtonDown(1))
    {
      StartCoroutine(nameof(DoubleShot));
    }
  }

  private IEnumerator Cooldown()
  {
    m_canFire = false;
    yield return new WaitForSeconds(m_fireRate);
    m_canFire = true;
  }

  private void Shoot()
  {
    var player = gameObject.transform.parent;
    for (int i = 0; i < m_pelletCount; ++i)
    {
      var direction = player.up * Random.Range(-1.0f, 1.0f);
      direction += player.right * Random.Range(-1.0f, 1.0f);
      direction = direction.normalized * Random.Range(0.0f, m_spread);
      direction += player.forward;

      var hitObject = Bullet.Shoot(player.position, direction, (obj) => obj.TryGetComponent(out Target _));
      hitObject?.GetComponent<Target>().DealDamage(0.1f);
    }
  }

  private void SingleShot()
  {
    StartCoroutine(nameof(Cooldown));
    m_magazine.Fire(Shoot);
  }

  private IEnumerator DoubleShot()
  {
    StartCoroutine(nameof(Cooldown));
    m_magazine.Fire(Shoot);
    yield return new WaitForSeconds(m_secondaryDelay);
    m_magazine.Fire(Shoot);
  }
}
