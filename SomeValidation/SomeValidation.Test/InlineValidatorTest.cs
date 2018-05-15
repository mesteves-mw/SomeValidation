using System.Collections.Generic;

namespace SomeValidation.Test
{
    using NUnit.Framework;
    using System.Linq;
    using SomeValidation.InlineValidation;
    using SomeValidation.Statements;

    [TestFixture]
    public class InlineValidatorTest
    {
        public class Customer
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Money Balance { get; set; }
        }

        public struct Money
        {
            public decimal Amount { get; set; }
        }

        [Test]
        public void TestInlineValidator()
        {
            var customer = new Customer();

            var errors = new List<IValidationError>();

            var val = new InlineValidator(
                onError: errors.Add);

            val.ShouldNotBe("Name", customer.Name)
                .NullOrEmpty()
                .Length().AtLeastAndAtMost(0, 100);

            val.ShouldBe("Age", customer.Age)
                .GreaterThan(18);

            AssertContainsInOrder(string.Join("", errors.Select(x => x.ErrorMessage)),
                "Name should not be null or empty.",
                "Name length should not be at least 0 and at most 100.",
                "Age should be greater than 18.");
        }

        [Test]
        public void TestInlineValidatorNull()
        {
            var customer = new Customer();

            var errors = new List<IValidationError>();

            var val = new InlineValidator(
                onError: errors.Add);

            customer = null;

            val.ShouldNotBe("customer", customer).Null()
                .Then(_ =>
                {
                    val.ShouldNotBe("Name", customer.Name)
                        .NullOrEmpty()
                        .Length().AtLeastAndAtMost(0, 100);

                    val.ShouldBe("Age", customer.Age)
                        .GreaterThan(18);
                });

            AssertContainsInOrder(string.Join("", errors.Select(x => x.ErrorMessage)),
                "customer should not be null.");
        }

        [Test]
        public void TestInlineValidator_Delegate()
        {
            var customer = new Customer();

            var errors = InlineValidator.Validate(customer, 
                (val, cust) =>
                {
                    val.ShouldNotBe("Name", cust.Name)
                        .NullOrEmpty()
                        .Length().AtLeastAndAtMost(0,100);

                    val.ShouldBe("Age", cust.Age)
                        .GreaterThan(18);
                });

            AssertContainsInOrder(string.Join("", errors.Select(x => x.ErrorMessage)),
                "Name should not be null or empty.",
                "Name length should not be at least 0 and at most 100.",
                "Age should be greater than 18.");
        }

        [Test]
        public void TestInlineValidator_DelegateStringParameter()
        {
            Customer customer = new Customer();

            var errors = InlineValidator.Validate("customer", customer,
                (val, forName, cust) =>
                {
                    val.ShouldNotBe(forName(), cust).Null();

                    if (cust == null) return;

                    val.ShouldNotBe(forName("Name"), cust.Name)
                        .NullOrEmpty()
                        .Length().AtLeastAndAtMost(0, 100);

                    val.ShouldBe(forName("Age"), cust.Age)
                        .GreaterThan(18);

                    val.Validate(forName("Balance"), cust.Balance, 
                        (val2, forName2, balance) =>
                        {
                            val2.ShouldBe(forName2("Amount"), balance.Amount)
                                .Positive()
                                .EqualTo(5);
                        });
                });

            AssertContainsInOrder(string.Join("", errors.Select(x => x.ErrorMessage)),
                "customer.Name should not be null or empty.",
                "customer.Name length should not be at least 0 and at most 100.",
                "customer.Age should be greater than 18.",
                "customer.Balance.Amount should be positive.",
                "customer.Balance.Amount should be equal to 5.");
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
