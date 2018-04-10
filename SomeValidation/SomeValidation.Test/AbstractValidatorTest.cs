namespace SomeValidation.Test
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class AbstractValidatorTest
    {
        public class Customer
        {
            public string Name { get; set; }
            public Address AddressData { get; set; }
            public int Age { get; set; }
            public Money Balance { get; set; }
        }

        public class Address
        {
            public string PostCode { get; set; }
            public string Street { get; set; }
        }

        public struct Money
        {
            public decimal Amount { get; set; }
        }

        public class CustomerValidator : AbstractValidator
        {
            public void Validate(Customer c)
            {
                this.ShouldNotBeNull("Name", c.Name);

                Create<AddressValidator>().Validate(c.AddressData);

                if (c.Age == 0) this.RaiseError("Age", "{0} is 0!");

               Create<MoneyValidator>().Validate("Balance", c.Balance);
            }
        }

        public class AddressValidator : AbstractValidator
        {
            public void Validate(Address a)
            {
                if (a == null) { this.RaiseError("Address", "{0} is null!"); return; }

                if (a.PostCode == null) this.RaiseError("PostCode", "{0} is null!");
                
                if (a.Street == null) this.RaiseError("Street", "{0} is null!");
            }
        }

        public class MoneyValidator : AbstractValidator
        {
            public void Validate(string parameterName, Money value)
            {
                if (value.Amount <= 0) this.RaiseError(parameterName, "{0} is negative!");

                if (value.Amount > 10000) this.RaiseError(parameterName, "{0} is higher than max!");
            }
        }

        [Test]
        public void ValidateTest_AbstractValidators()
        {
            var cust = new Customer();
            cust.AddressData = new Address();

            var cv = new CustomerValidator();

            string errors = "";

            cv.OnError += vf => errors += string.Format(" -- " + vf.ErrorMessage + "\n", vf.ParameterName);

            cv.Validate(cust);

            AssertContainsInOrder(errors,
                " -- Name is null!\n",
                " -- PostCode is null!\n",
                " -- Street is null!\n",
                " -- Age is 0!",
                " -- Balance is negative!");
        }
        
        public static void AssertContainsInOrder(string input, params string[] subStrings)
        {
            foreach (string subStr in subStrings)
            {
                Assert.That(input, Contains.Substring(subStr));
                input = input.Replace(subStr, string.Empty);
            }
        }
    }
}
