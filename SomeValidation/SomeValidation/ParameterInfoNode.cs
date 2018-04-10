using System;

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
    }
}
