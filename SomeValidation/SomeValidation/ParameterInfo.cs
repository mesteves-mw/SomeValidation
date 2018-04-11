using System;

namespace SomeValidation
{
    public class ParameterInfo : IParameterInfo
    {
        protected ParameterInfo() { }
        public ParameterInfo(string parameterName)
        {
            this.ShortName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
            this.Guid = Guid.NewGuid();
        }

        public virtual Guid Guid { get; }
        public virtual string ShortName { get; }

        public virtual string Name => ShortName;

        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && obj.GetType() == this.GetType() && this.Equals((ParameterInfo)obj);
        }

        public bool Equals(ParameterInfo parameter)
        {
            return !ReferenceEquals(parameter, null) && this.GetHashCode() == parameter.GetHashCode();
        }

        public static bool operator ==(ParameterInfo left, ParameterInfo right)
        {
            return ReferenceEquals(left, right) || !ReferenceEquals(left, null) && left.Equals(right);
        }

        public static bool operator !=(ParameterInfo left, ParameterInfo right)
        {
            return !ReferenceEquals(left, right) && !ReferenceEquals(left, null) && !left.Equals(right);
        }

        public static ParameterInfoNode operator /(ParameterInfo left, ParameterInfo right)
        {
            return ParameterInfoNode.ChainParameters(left, right);
        }
    }
}
