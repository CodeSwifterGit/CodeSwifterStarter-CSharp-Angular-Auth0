using System;

namespace CodeSwifterStarter.Common.Interfaces
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}