using System;

namespace SomeValidation
{
    public class ParameterInfo
    {
        private ParameterInfo() { }
        private ParameterInfo(string parameterName)
        {
            this.Name = parameterName;
            this.Guid = Guid.NewGuid();
        }

        public Guid Guid { get; }
        public string Name { get; }
        //public ParameterInfo Parent { get; set; }
        //public ParameterInfo Child { get; set; }
        //public bool HasParent => this.Parent == null;
        //public bool HasChild => this.Child == null;

        public static implicit operator ParameterInfo(string parameterName)
        {
            if (parameterName == null)
                throw new ArgumentNullException(nameof(parameterName));

            return new ParameterInfo(parameterName);
        }

        public static implicit operator string(ParameterInfo parameterInfo)
        {
            return parameterInfo.Name;
        }
    }
}
