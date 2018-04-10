namespace SomeValidation.Test
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class AbstractTValidatorTest
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

        public class CustomerValidator : AbstractValidator<Customer>
        {
            public override void Validate(Customer c, params Guid[] ruleSet)
            {
                this.ShouldNotBeNull("Name", c.Name);

                Create<AddressValidator>().Validate("AddressData", c.AddressData);

                if (c.Age == 0) this.RaiseError("Age", "{0} is 0!");

               Create<MoneyValidator>().Validate("Balance", c.Balance);
            }
        }

        public class AddressValidator : StringParameterValidator<Address>
        {
            protected override void Validate(ForName forName, Address a, params Guid[] ruleSet)
            {
                if (a == null) { this.RaiseError(forName("Address"), "{0} is null!"); return; }

                if (a.PostCode == null) this.RaiseError(forName("PostCode"), "{0} is null!");

                if (a.Street == null) this.RaiseError(forName("Street"), "{0} is null!");
            }
        }

        public class MoneyValidator : StringParameterValidator<Money>
        {
            protected override void Validate(ForName forName, Money instance, params Guid[] ruleSet)
            {
                if (instance.Amount <= 0) this.RaiseError(forName(), "{0} is negative!");

                if (instance.Amount > 10000) this.RaiseError(forName(), "{0} is higher than max!");
            }
        }

        [Test]
        public void ValidateTest_AbstractValidatorWithParameterValidators()
        {
            var cust = new Customer();
            cust.AddressData = new Address();

            var cv = new CustomerValidator();

            string errors = "";

            cv.OnError += vf => errors += string.Format(" -- " + vf.ErrorMessage + "\n", vf.ParameterName);

            cv.Validate(cust);

            AssertContainsInOrder(errors,
                " -- Name is null!\n",
                " -- AddressData.PostCode is null!\n",
                " -- AddressData.Street is null!\n",
                " -- Age is 0!",
                " -- Balance is negative!");
        }

        [Test]
        public void ValidateAndThrowTest_AbstractValidatorWithParameterValidators()
        {
            var cust = new Customer();
            cust.AddressData = new Address();

            var cv = new CustomerValidator();

            var ex = Assert.Throws<ValidationException>(() => cv.ValidateAndThrow(cust));

            AssertContainsInOrder(ex.Message,
                " -- Name is null!",
                " -- AddressData.PostCode is null!",
                " -- AddressData.Street is null!",
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
