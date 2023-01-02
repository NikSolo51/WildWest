using CodeBase.Infrastructure.Services;

namespace CodeBase.Services.Hud
{
    public interface IHudService : IService
    {
        void ChangeState(HudState hudState);

        HudState GetState();
    }
}