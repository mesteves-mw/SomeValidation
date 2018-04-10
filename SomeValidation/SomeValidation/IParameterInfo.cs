using System;

namespace SomeValidation
{
    //public class Parameter
    //{
    //    private Parameter(object value)
    //    {
    //        this._value = value;
    //    }

    //    private object _value;

    //    public static implicit operator Parameter(string parameterName)
    //    {
    //        if (parameterName == null)
    //            throw new ArgumentNullException(nameof(parameterName));

    //        return new Parameter(parameterName);
    //    }

    //    public static implicit operator Parameter(ParameterInfo parameterInfo)
    //    {
    //        if (parameterInfo == null)
    //            throw new ArgumentNullException(nameof(parameterInfo));

    //        return new Parameter(parameterInfo);
    //    }

    //    public static implicit operator Parameter(ParameterInfoNode parameterInfoNode)
    //    {
    //        if (parameterInfoNode == null)
    //            throw new ArgumentNullException(nameof(parameterInfoNode));

    //        return new Parameter(parameterInfoNode);
    //    }

    //    public static implicit operator string(Parameter parameter)
    //    {
    //        return (string)parameter._value;
    //    }

    //    public static implicit operator ParameterInfo(Parameter parameter)
    //    {
    //        return (ParameterInfo)parameter._value;
    //    }

    //    public static implicit operator ParameterInfoNode(Parameter parameter)
    //    {
    //        return (ParameterInfoNode)parameter._value;
    //    }
    //}

    public interface IParameterInfo
    {
        Guid Guid { get; }
        string Name { get; }
        string ShortName { get; }
    }
}
