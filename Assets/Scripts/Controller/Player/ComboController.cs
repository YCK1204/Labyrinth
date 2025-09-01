public sealed class ComboController
{
    readonly float minGap;// 다음 공격까지 필요한 최소 시간
    readonly float resetGap;// 콤보가 초기화되는 시간
    float elapsed;
    int step;
    public ComboController(float minGap = 0.25f, float resetGap = 0.6f)
    {
        this.minGap = minGap;
        this.resetGap = resetGap;
    }
    public void Tick(float dt)
    {
        elapsed += dt;
    }
    public bool TryNext(out int attackStep)
    {
        attackStep = 0;
        if (elapsed < minGap) return false;
        if (elapsed > resetGap) step = 0;
        step = (step % 3) + 1; elapsed = 0f; attackStep = step; return true;
    }
}
