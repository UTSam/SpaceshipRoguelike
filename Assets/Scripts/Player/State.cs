/*
    Authors:
      Robbert Ritsema
*/

public abstract class State
{
    protected Player player;

    public abstract void Tick();
    public abstract void FixedTick();

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(Player player)
    {
        this.player = player;
    }
}