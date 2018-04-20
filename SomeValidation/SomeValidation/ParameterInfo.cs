using System;
using System.Collections.Generic;

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
            if (ReferenceEquals(parameter, null)) return false;

            if (parameter is ParameterInfoNode parameterInforNode)
            {
                return parameterInforNode.Equals(this);
            }

            return this.Guid == parameter.Guid;
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

        public static bool operator <=(ParameterInfo left, ParameterInfo right)
        {
            bool result = ReferenceEquals(left, right) || !ReferenceEquals(left, null);

            if (result)
            {
                var leftGuids = (left as ParameterInfoNode)?.Guids ?? new List<Guid> {left.Guid};
                var rightGuids = (right as ParameterInfoNode)?.Guids ?? new List<Guid> {right.Guid};

                for (int i = 1; i <= leftGuids.Count; i++)
                {
                    result = result && leftGuids[leftGuids.Count - i] == rightGuids[rightGuids.Count - i];
                }
            }

            return  result;
        }

        public static bool operator >=(ParameterInfo left, ParameterInfo right)
        {
            return right <= left;
        }
    }
}
