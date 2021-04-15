using System;
using CodeSwifterStarter.Common.Interfaces;

namespace CodeSwifterStarter.Infrastructure
{
    public class MachineDateTime : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}