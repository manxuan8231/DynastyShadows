public abstract class BaseState
{
    protected BossScript boss;

    public BaseState(BossScript boss)
    {
        this.boss = boss;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void ExitState() { }
}