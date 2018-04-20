using System;
using System.Collections.Generic;

namespace SomeValidation
{
    public class ParameterInfoNode : ParameterInfo, IParameterInfo
    {
        public ParameterInfoNode(string parameterName)
            : base(parameterName)
        {
        }

        public ParameterInfoNode(ParameterInfo parameter)
        {
            this.Guid = parameter.Guid;
            this.ShortName = parameter.ShortName;
        }

        public ParameterInfoNode AddNext(ParameterInfo nextParameter)
        {
            this.Next = new ParameterInfoNode(nextParameter);
            this.Next.Previous = this;
            return this.Next;
        }

        public override Guid Guid { get; }
        public override string ShortName { get; }
        public override string Name
        {
            get
            {
                var current = this;
                string paramStr = current.ShortName;
                while (current.HasPrevious)
                {
                    current = current.Previous;
                    paramStr = current.ShortName + "." + paramStr;
                }

                return paramStr;
            }
        }


        public ParameterInfoNode Previous { get; private set; }
        public ParameterInfoNode Next { get; private set; }
        public bool HasPrevious => this.Previous != null;
        public bool HasNext => this.Next != null;

        public static ParameterInfoNode ChainParameters(ParameterInfo parameter, ParameterInfo p)
        {
            if (parameter == null)
                return p != null
                        ? new ParameterInfoNode(p)
                        : null;

            if (parameter is ParameterInfoNode parameterNode)
                return p != null
                        ? parameterNode.AddNext(p)
                        : parameterNode;
            else
                return p != null
                        ? new ParameterInfoNode(parameter).AddNext(p)
                        : new ParameterInfoNode(parameter);
        }

        public List<Guid> Guids
        {
            get
            {
                var current = this;
                var guids = new List<Guid> { current.Guid };
                while (current.HasPrevious)
                {
                    current = current.Previous;
                    guids.Insert(0, current.Guid);
                }

                return guids;
            }
        }

        public override int GetHashCode()
        {
            int res = 1;
            foreach (var guid in Guids)
            {
                res = res * 31 + (guid == null ? 0 : guid.GetHashCode());
            }
            return res;
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && obj.GetType() == this.GetType() && this.Equals((ParameterInfoNode)obj);
        }

        public bool Equals(ParameterInfoNode parameter)
        {
            bool result = !ReferenceEquals(parameter, null);

            if (result)
            {
                var leftGuids = this.Guids;
                var rightGuids = parameter.Guids;

                result = leftGuids.Count == rightGuids.Count;

                if (result)
                {
                    for (int i = leftGuids.Count - 1; i >= 0; i--)
                    {
                        result = result && leftGuids[i] == rightGuids[i];
                    }
                }
            }

            return result;
        }

        public new bool Equals(ParameterInfo parameter)
        {
            return this.Equals(parameter as ParameterInfoNode ?? new ParameterInfoNode(parameter));
        }

        public static bool operator ==(ParameterInfoNode left, ParameterInfoNode right)
        {
            return ReferenceEquals(left, right) || !ReferenceEquals(left, null) && left.Equals(right);
        }

        public static bool operator !=(ParameterInfoNode left, ParameterInfoNode right)
        {
            return !ReferenceEquals(left, right) && !ReferenceEquals(left, null) && !left.Equals(right);
        }

        public static ParameterInfoNode operator /(ParameterInfoNode left, ParameterInfo right)
        {
            return ChainParameters(left, right);
        }
    }
}
