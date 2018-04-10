namespace SomeValidation.Test
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class StringParameterValidatorRulesTest
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
            public Customer Owner { get; set; }
        }

        public struct Money
        {
            public decimal Amount { get; set; }
        }

        public class CustomerValidator : StringParameterValidator<Customer>
        {
            public static readonly Guid Rule1 = Guid.NewGuid();
            public static readonly Guid Rule2 = Guid.NewGuid();

            protected override void Validate(ForName forName, Customer c, params Guid[] ruleSet)
            {
                if (c.Name == null) this.RaiseError(forName("Name"), "{0} is null!");

                if (ruleSet.Contains(Rule1))
                    Create<AddressValidator>().Validate(forName("AddressData"), c.AddressData, ruleSet);

                if (c.Age == 0) this.RaiseError(forName("Age"), "{0} is 0!");

                if (ruleSet.Contains(Rule2))
                    Create<MoneyValidator>().Validate(forName("Balance"), c.Balance);
            }
        }

        public class AddressValidator : StringParameterValidator<Address>
        {
            public static readonly Guid Rule1 = Guid.NewGuid();
            public static readonly Guid Rule2 = Guid.NewGuid();

            protected override void Validate(ForName forName, Address a, params Guid[] ruleSet)
            {
                if (a == null) { this.RaiseError(forName("Address"), "{0} is null!"); return; }

                if (a.PostCode == null) this.RaiseError(forName("PostCode"), "{0} is null!");

                if (ruleSet.Contains(Rule1))
                    Create<CustomerValidator>().Validate(forName("Owner"), a.Owner);

                if (ruleSet.Contains(Rule2))
                    if (a.Street == null) this.RaiseError(forName("Street"), "{0} is null!");
            }
        }

        public class MoneyValidator : StringParameterValidator<Money>
        {
            protected override void Validate(ForName forName, Money value, params Guid[] ruleSet)
            {
                if (value.Amount <= 0) this.RaiseError(forName(), "{0} is negative!");

                if (value.Amount > 10000) this.RaiseError(forName(), "{0} is higher than max!");
            }
        }

        [Test]
        public void ValidateTest_StringParameterValidatorsParallelRules()
        {
            var cust = new Customer();
            cust.AddressData = new Address();
            cust.AddressData.Owner = new Customer();

            var cv = new CustomerValidator();

            //Handling raise of errors
            var errors = new List<IValidationError>();
            cv.OnError += errors.Add;

            //Calling validate
            Parallel.Invoke(
                () => cv.Validate("cust", cust, CustomerValidator.Rule1, AddressValidator.Rule2),
                () => cv.Validate("cust2", cust, CustomerValidator.Rule1, CustomerValidator.Rule2, AddressValidator.Rule1));

            //Handle validation failure list
            var errorMessage = " -- " + string.Join("\r\n -- ", errors.Select(vf => string.Format(vf.ErrorMessage, vf.ParameterName)));

            AssertContainsInOrder(errorMessage,
                " -- cust.Name is null!",
                " -- cust2.Name is null!",
                " -- cust2.AddressData.PostCode is null!",
                " -- cust.AddressData.PostCode is null!",
                " -- cust2.AddressData.Owner.Name is null!",
                " -- cust2.AddressData.Owner.Age is 0!",
                " -- cust2.Age is 0!",
                " -- cust.AddressData.Street is null!",
                " -- cust.Age is 0!",
                " -- cust2.Balance is negative!");
        }

        public static void AssertContainsInOrder(string input, params string[] subStrings)
        {
            foreach(string subStr in subStrings)
            {
                Assert.That(input, Contains.Substring(subStr));
                input = input.Replace(subStr, string.Empty);
            }
        }
    }
}
