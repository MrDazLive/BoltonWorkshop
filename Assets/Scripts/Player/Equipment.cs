using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
  private Dictionary<eWeapon, GameObject> m_weapons = new Dictionary<eWeapon, GameObject>();
  private eWeapon m_activeWeapon;

  // Start is called before the first frame update
  void Start()
  {
    m_weapons.Add(eWeapon.SNIPER, GetComponentInChildren<Sniper>(true).gameObject);
    m_weapons.Add(eWeapon.MINIGUN, GetComponentInChildren<Minigun>(true).gameObject);
    m_weapons.Add(eWeapon.SHOTGUN, GetComponentInChildren<Shotgun>(true).gameObject);
    
    m_weapons[default].SetActive(true);
  }

  // Update is called once per frame
  void Update()
  {
    for (int i = 0; i < Mathf.Min(8, typeof(eWeapon).GetEnumValues().Length); ++i)
      if (Input.GetKeyDown((i + 1).ToString()))
        ActivateWeapon((eWeapon)i);
  }

  void ActivateWeapon(eWeapon weapon)
  {
    if (weapon == m_activeWeapon)
      return;

    m_weapons[m_activeWeapon].SetActive(false);
    m_activeWeapon = weapon;
    m_weapons[m_activeWeapon].SetActive(true);
  }
}
