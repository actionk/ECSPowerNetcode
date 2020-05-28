namespace Plugins.ECSPowerNetcode.Server.Entities
{
    public interface INetworkEntityIdFactory
    {
        ulong NextId();
    }
}