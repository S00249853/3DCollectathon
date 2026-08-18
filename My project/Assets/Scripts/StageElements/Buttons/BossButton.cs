using UnityEngine;

public class BossButton : RevealButton
{
    [SerializeField] FinalBossCannon _boss;
    [SerializeField] Phase _phase;
      protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    public override void Stomped()
    {
        base.Stomped();
        _boss.Phase = _phase;
    }
}
