using SomeValidation.InlineValidation;

namespace SomeValidation.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using SomeValidation.Statements;

    [TestFixture]
    public class StatementsTest
    {
        [Test]
        public void TestStatements()
        {
            var v = new InlineValidator(
                onError: ve => Console.WriteLine(ve.ErrorMessage));

            v.ShouldNotBe("string", "").NullOrEmpty();
            v.ShouldNotBe("string", "").Empty();
            v.ShouldNotBe("string", (string)null).Null();
            v.CannotBe("int", (int?)null).Null();

            v.ShouldNotBe("icollection", (ICollection)null).NullOrEmpty();
            v.ShouldNotBe("icollection", new string[]{}).Empty();

            v.CannotBe("hashtable", (ICollection)new Hashtable()).Empty();

            v.ShouldNotBe("array", new string[]{}).Empty();
            v.ShouldNotBe("list", new List<string>()).Empty();

            v.ShouldBe("int150", 150).GreaterThanOrEqualTo(151);
            v.ShouldBe("int150", 150).GreaterThan(150);
            v.ShouldBe("int150", 150).LessThanOrEqualTo(149);
            v.ShouldBe("int150", 150).LessThan(150);

            v.ShouldBe("int150", 1500).AtLeastAndAtMost(100, 200);
            v.ShouldBe("decimalat1500", 1500d).AtLeastAndAtMost(100d, 200d);
            v.ShouldBe("decimalir1500", 1500d).InRange(100d, 200d);
            v.ShouldBe("decimalbt1500", 1500d).Between(100d, 200d);
            v.ShouldNotBe("datetimeNow", DateTime.Now).AtLeastAndAtMost(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));

            v.ShouldBe("string-abc", "abc").EqualTo("cde");
            v.ShouldBe("icollection-abc", new[] {"a", "b", "c"}).EquivalentTo(new List<string>(new[] { "a", "b", "b" }));

            v.ShouldBe("string10", "abcdefghij").Length().GreaterThan(15); // string10 length should be greater than 15.
            v.ShouldBe("icollection-abc", new[] { "a", "b", "c" }).Count().AtLeastAndAtMost(4,15);

            v.ShouldNotBe("string-abc", "abc123").Matching(@"bc\d+");

            v.ShouldNotBe("string-param", "", "The parameter '@parameterName' is invalid.").NullOrEmpty();

            //optional no fluent interface return
            v.ShouldNotBe("cardnumber5", "10345").Do(stmt =>
            {
                if (stmt.Value != null)
                    stmt.Length().LessThanOrEqualTo(10);
            });

            //branching
            v.ShouldNotBe("cardnumber6", "103456").Do(stmt =>
            {
                if (stmt.Value != null)
                    stmt.Length().LessThanOrEqualTo(10);
                else
                    stmt.Null();
            });

            //optional null check fluent interface
            v.ShouldNotBe("cardnumber7", "<1234>").NullOrEmpty()
                .Then(stmt => stmt
                    .Matching(@"<\w+>")
                    .Length().LessThanOrEqualTo(10));

            //optional keep fluent interface 1
            v.ShouldNotBe("cardnumber10", "1034567890").Do(stmt =>
            {
                if (stmt.Value == null) return stmt;

                return stmt.ApplyConstraint(
                    stmt.Value.Length >= 10 && stmt.Value.StartsWith("10"),
                    "invalid 10-16"); //cardNumber should not be invalid 10-16.
            });

            //optional keep fluent interface 2
            v.ShouldNotBe("cardnumber16", "166666666634567890").Do(stmt =>
            {
                if (stmt.Value == null) return stmt;

                if (stmt.Value.Length >= 16 && stmt.Value.StartsWith("16"))
                    return stmt.OverrideMessage("@parameterName is invalid 16.").RaiseError(); //cardNumber is invalid 16-25.

                return stmt;
            });

        }
    }
}
