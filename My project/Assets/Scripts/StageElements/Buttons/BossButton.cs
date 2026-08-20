using UnityEngine;

public class BossButton : RevealButton
{
    [SerializeField] FinalBossCannon _boss;
    [SerializeField] Phase _phase;
    public GameObject[] Disable;
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    public override void Stomped()
    {
        base.Stomped();
        _boss.Phase = _phase;
        foreach (var disable in Disable)
        {
            disable.gameObject.SetActive(false);
        }
    }
}
