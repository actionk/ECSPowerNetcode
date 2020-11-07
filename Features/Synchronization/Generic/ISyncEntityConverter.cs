using Unity.Networking.Transport;

namespace Plugins.Shared.ECSPowerNetcode.Features.Synchronization.Generic
{
    public interface ISyncEntityConverter<T>
    {
        void Convert(T value);
        void Serialize(ref DataStreamWriter writer);
        void Deserialize(ref DataStreamReader reader);
        T Value { get; }
    }
}