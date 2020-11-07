using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Plugins.Shared.ECSPowerNetcode.Server.Exceptions
{
    public class ServerException : Exception
    {
        public ServerException()
        {
        }

        protected ServerException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ServerException(string message) : base(message)
        {
        }

        public ServerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}