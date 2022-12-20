using CodeBase.Infrastructure.Services;

namespace CodeBase.Services.Update
{
    public interface IUpdateService : IService
    {
        void Update();
        void FixedUpdate();
        void LateUpdate();
        void Register(ICacheUpdate obj);
        void Unregister(ICacheUpdate obj);
    }
}