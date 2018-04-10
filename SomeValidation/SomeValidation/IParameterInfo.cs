using System;

namespace SomeValidation
{
    public interface IParameterInfo
    {
        Guid Guid { get; }
        string Name { get; }
        string ShortName { get; }
    }
}
