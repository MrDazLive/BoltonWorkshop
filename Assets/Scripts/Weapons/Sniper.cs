using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Magazine))]
public class Sniper : MonoBehaviour
{
  private Magazine m_magazine;

  private float m_fireRate = 0.8f;
  private bool m_canFire = true;

  // Start is called before the first frame update
  void Start()
  {
    m_magazine = GetComponent<Magazine>();
  }

  void Update()
  {
    if (Input.GetMouseButtonDown(0) && m_canFire)
    {
      Shoot();
    }
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
