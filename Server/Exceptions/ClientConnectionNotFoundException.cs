using System;

namespace Plugins.ECSPowerNetcode.Server.Exceptions
{
    public class ClientConnectionNotFoundException : Exception
    {
        public ClientConnectionNotFoundException()
        {
        }

        public ClientConnectionNotFoundException(string message) : base(message)
        {
        }
    }
}