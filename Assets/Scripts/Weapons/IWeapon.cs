public interface IWeapon
{
  void OnTriggerDown(eTrigger triggerType);
  void OnTriggerHold(eTrigger triggerType);
  void OnTriggerRelease(eTrigger triggerType);
}
