using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
  [SerializeField, Range(1.0f, 10.0f)]
  private float m_recoveryTime = 1.0f;

  private bool m_recover = true;

  // Update is called once per frame
  void Update()
  {
    if (m_recover)
    {
      TransitionColour(Time.deltaTime / m_recoveryTime);
      RefreshLifeText();
    }
  }

  private IEnumerator DelayRecover()
  {
    m_recover = false;
    yield return new WaitForSeconds(1.0f);
    m_recover = true;
  }

  public void DealDamage(float damage)
  {
    StopCoroutine(nameof(DelayRecover));
    TransitionColour(-damage);
    RefreshLifeText();
    StartCoroutine(nameof(DelayRecover));
  }

  void TransitionColour(float modifier)
  {
    var renderer = GetComponent<Renderer>();

    var colour = renderer.material.color;
    TransistionChannel(ref colour.g, modifier);
    TransistionChannel(ref colour.b, modifier);

    renderer.material.color = colour;
  }

  void TransistionChannel(ref float channel, float modifier)
  {
    channel = Mathf.Clamp(channel + modifier, 0.0f, 1.0f);
  }

  void RefreshLifeText()
  {
    var textMesh = GetComponentInChildren<TextMesh>(true);
    var renderer = GetComponent<Renderer>();

    int life = Mathf.CeilToInt(renderer.material.color.g * 100);
    textMesh.text = life.ToString();
    textMesh.gameObject.SetActive(life != 100);
  }
}
