public abstract class PlayerState
{
    public abstract void Enter(PlayerController playerController);
    public abstract void Execute(PlayerController playerController);
    public abstract void Exit(PlayerController playerController);

}