using System;
using System.Collections.Generic;
using System.Linq;
using ExploreFluentAssertions.Business;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExploreFluentAssertions.Tests
{
    [TestClass]
    public class CheckInstallationTests
    {
        [TestMethod]
        public void BasicAssertionTest()
        {
            object anObject = null;

            anObject.Should().BeNull("the value is null");

            Utils.Throws(() => anObject.Should().NotBeNull("we expect this to fail"));

            anObject = "Sumanesh";

            //Check for the type of the object. CAUTION : BeOfType expects the object has exactly be of that type
            anObject.Should().BeOfType<string>("we assigned a string to anObject");
            anObject.Should().BeOfType(typeof(string), "this is just another way to test for types");

            anObject = new Student { Name = "Sumanesh", Age = 34 };
            //Check whether an object is of a specific type you are interested in and get the instance to check further... cool..
            anObject.Should().BeOfType<Student>().Which.Name.Should().Be("Sumanesh");

            //We should be able to compare two objects (uses object.Equals to compare)
            anObject.Should().NotBe(new Student { Name = "Aadhavan", Age = 4 }, "we should be able to differentiate between diff objects");
            anObject.Should().Be(new Student { Name = "Sumanesh", Age = 34 }, "we should find similar objects correctly");


            var ex = new ArgumentException();

            //Check whether ex derives from Exception class
            ex.Should().BeAssignableTo<Exception>("ArgumentException is also an Exception");

            //Check for Serializability - 
            //CAUTION : Something wrong. We expect to see an exception as we have marked a field as NonSerialized, but it doesn't throw any
            new Student().Should().BeXmlSerializable();
            new Student().Should().BeBinarySerializable("we have marked PersonalInfo as non-serializable");

            //(In theory), you could exclude any fields while comparing for Serializability
            new Student().Should().BeBinarySerializable<Student>(options => options.Excluding(s => s.PersonalInfo));
        }

        [TestMethod]
        public void NullabilityTests()
        {
            int? nullInt = null;
            nullInt.Should().NotHaveValue("we explicitly assigned null");

            DateTime? notNullDateTime = DateTime.Now;
            notNullDateTime.Should().HaveValue("we assigned something");
        }

        [TestMethod]
        public void BooleanTests()
        {
            bool aBoolean = false;

            aBoolean.Should().BeFalse("obviously");

            Utils.Throws(() => aBoolean.Should().BeTrue("Expect this to fail"));
        }

        [TestMethod]
        public void StringTests()
        {
            string aString = "";

            aString.Should().BeEmpty();
            aString.Should().NotBeNull();
            Utils.Throws(() => aString.Should().NotBeEmpty("fails"));
            aString.Should().HaveLength(0);
            aString.Should().BeNullOrWhiteSpace();
            Utils.Throws(() => aString.Should().NotBeNullOrEmpty("fails"));

            aString = "This is a test";
            aString.Should().Be("This is a test");
            aString.Should().NotBe("THIS IS A TEST"); //NotBe -> Case sensitive comparison
            aString.Should().BeEquivalentTo("THIS IS A TEST"); //BeEquivalentTo -> Case insensitive comparison

            aString.Should().Contain("is a ");
            aString.Should().NotContain("not it");
            aString.Should().ContainEquivalentOf("IS A");
            aString.Should().NotContainEquivalentOf("NOT IT");

            aString.Should().StartWith("This");
            aString.Should().NotStartWith("this");
            aString.Should().StartWithEquivalent("this");
            Utils.Throws(() => aString.Should().NotStartWithEquivalentOf("this"));

            aString.Should().EndWith("a test");
            aString.Should().EndWithEquivalent("A TEST");
            aString.Should().NotEndWith("A TEST");
            Utils.Throws(() => aString.Should().NotEndWithEquivalentOf("A TEST"));


            //Wild cards match
            string validEmailAddress = "sumaneshm@yahoo.com";
            string invalidEmailAddress = "NotAValidEmailAddress";

            validEmailAddress.Should().Match("*@*.com");
            Utils.Throws(() => invalidEmailAddress.Should().Match("*@*.com"));

            validEmailAddress.Should().MatchEquivalentOf("*@*.???");


            //RegExp
            string regexTest = @"hello Sumanesh world.";
            regexTest.Should().MatchRegex("h.*\\sworld.$");

            Utils.Throws(() => regexTest.Should().NotMatchRegex(".*"));
        }

        [TestMethod]
        public void NumericTests()
        {
            int aZero = 0;

            aZero.Should().NotBe(10);
            aZero.Should().BeGreaterThan(-1);
            aZero.Should().BeGreaterOrEqualTo(0);
            Utils.Throws(() => aZero.Should().BePositive("Zero is not positive"));
            Utils.Throws(() => aZero.Should().BePositive("Zero is not negative"));
            aZero.Should().BeInRange(-10, 20);


            float aDouble = 5.2f;
            aDouble.Should().Be(5.2f);
            aDouble.Should().BeApproximately(5.2f, 0.001f);

            aZero.Should().BeOneOf(Enumerable.Range(-1, 3).ToList());

        }

        [TestMethod]
        public void DateTimeTests()
        {
            //Cool way to initialize a date. Very readable.
            DateTime _26Dec2014_22_13 = 26.December(2014).At(22, 13, 30);

            _26Dec2014_22_13.Should().BeAfter(1.December(2014));
            _26Dec2014_22_13.Should().BeBefore(1.January(2015));
            _26Dec2014_22_13.Should().BeOnOrAfter(19.December(1980));

            _26Dec2014_22_13.Should().Be(26.December(2014).At(22, 13, 30));

            _26Dec2014_22_13.Should().HaveDay(26);
            _26Dec2014_22_13.Should().HaveMonth(12);
            _26Dec2014_22_13.Should().HaveYear(2014);
            _26Dec2014_22_13.Should().HaveHour(22);
            _26Dec2014_22_13.Should().HaveMinute(13);
            _26Dec2014_22_13.Should().HaveSecond(30);


            _26Dec2014_22_13.Should().BeWithin(10.Minutes()).Before(26.December(2014).At(21, 13, 20));
            _26Dec2014_22_13.Should().BeWithin(20.Minutes()).After(26.December(2014).At(22, 3));

            //BeMoreThan is equivalent to >
            Utils.Throws(() => _26Dec2014_22_13.Should().BeMoreThan(1.Days()).Before(_26Dec2014_22_13.AddDays(1)));                       // >

            //BeAtLeast is equivalent to >=.
            _26Dec2014_22_13.Should().BeAtLeast(1.Days()).Before(_26Dec2014_22_13.AddDays(1));

            _26Dec2014_22_13.Should().BeExactly(24.Hours()).Before(_26Dec2014_22_13.AddDays(1));


            //BeCloseTo - See how close to the given time with the difference expected in ms given
            _26Dec2014_22_13.Should().BeCloseTo(26.December(2014).At(22, 13, 31), 1000);

        }

        [TestMethod]
        public void CollectionsTest()
        {
            IEnumerable<int> collections = new[] { 1, 2, 5, 8 };


            collections.Should().NotBeEmpty()
                .And.HaveCount(4)                           //Checks the array length
                .And.ContainInOrder(new[] { 2, 5 });          //Checks whether all the given numbers are in the same order


            collections.Should().ContainInOrder(new[] { 2, 8 }, "Testing array {2,8} need not appear in the same order in the testing array, just in the order, but can have elements inbetween");
            Utils.Throws(() => collections.Should().ContainInOrder(new[] { 5, 2 }, "5 appears after 2 in the testing array"));

            collections.Should().ContainItemsAssignableTo<int>();

            IEnumerable<object> derivedClassCollections = new[] { new DerivedClass(), new DerivedClass() };
            derivedClassCollections.Should().ContainItemsAssignableTo<DerivedClass>("All the items in the collection are derived class");


            collections.Should().Equal(new List<int> { 1, 2, 5, 8 });
            collections.Should().Equal(1, 2, 5, 8);
            collections.Should().BeEquivalentTo(new[] { 2, 5, 1, 8 }, "can be in any order");
            collections.Should().NotBeEquivalentTo(new[] { 2, 3, 4 }, "this is a different array");

            collections.Should().HaveCount(c => c >= 4); // Collections count should be >= 4
            collections.Should().OnlyHaveUniqueItems();

            Utils.Throws(() => (new[] { 1, 1 }).Should().OnlyHaveUniqueItems());

            collections.Should().HaveSameCount(new[] { 1, 2, 3, 4 });

            collections.Should().BeSubsetOf(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            collections.Should().Contain(8).And.HaveElementAt(0, 1); //At index 0, we should have 1
            collections.Should().HaveElementAt(1, 2);
            collections.Should().HaveElementAt(2, 5);
            collections.Should().HaveElementAt(3, 8);

            collections.Should().Contain(new[] { 1, 5 });

            Utils.Throws(() => collections.Should().Contain(x => x > 12)); //There is no element in this array is > 12

            collections.Should().OnlyContain(x => x < 10);
            collections.Should().NotContain(x => x > 10);

            Utils.Throws(() => collections.Should().BeEmpty());   //Collection is not empty
            collections.Should().BeInAscendingOrder();
            Utils.Throws(() => collections.Should().BeInDescendingOrder()); // Not in ascending order
            collections.Should().IntersectWith(new[] { 1 }); //Contains one or more elements in common with the array being passed
            collections.Should().NotIntersectWith(new[] { 3, 4, 6 }); //Nothing is in common

            var students = new[]
            {
                new Student {Age = 34, Name = "Sumanesh"},
                new Student {Age = 31, Name = "Saveetha"},
                new Student {Age = 4, Name = "Aadhavan"},
                new Student {Age = 1, Name = "Aghilan"},
            };

            students.Should().BeInDescendingOrder(s => s.Age);  //Performs the comparison based on Age property

            var newStudents = new[]
            {
                new Student {Age = 34, Name = "Sumanesh"},
                new Student {Age = 31, Name = "Saveetha"},
                new Student {Age = 4, Name = "Aadhavan"},
                new Student {Age = 1, Name = "Aghilan"},
            };

            students.Should().Equal(newStudents, (m, s) => m.Name == s.Name);
        }

        [TestMethod]
        public void DictionariesTest()
        {
            Dictionary<string, string> firstDictionary = new Dictionary<string, string>
            {
                {"97CS28", "Sumanesh"},
                {"97CS18", "Pradeep"},
                {"97CS41", "Malathi"},
                {"97CS35", "Priya"},
                {"97CS106", "Kumar"},
                {"97CS29", "Thiru"},
                {"97CS30", "Vijay"},
            };


            Dictionary<string, string> secondDictionary = new Dictionary<string, string>
            {
                {"97CS28", "Sumanesh"},
                {"97CS18", "Pradeep"},
                {"97CS35", "Priya"},
                {"97CS30", "Vijay"},
                {"97CS106", "Kumar"},
                {"97CS41", "Malathi"},
                {"97CS29", "Thiru"},
            };

            firstDictionary.Should().Equal(secondDictionary);

            firstDictionary.Should().ContainKey("97CS28");

            firstDictionary.Should().ContainValue("Sumanesh");

            firstDictionary.Should().Contain("97CS28", "Sumanesh");

            firstDictionary.Should().NotContain("97CS28", "Pradeep");

            firstDictionary.Should().HaveCount(l => l > 5);

        }

        [TestMethod]
        public void GuidTests()
        {
            Guid a = Guid.NewGuid();
            Guid b = Guid.NewGuid();

            Guid sameAsA = a;

            a.Should().NotBe(b);
            a.Should().Be(sameAsA);

        }

        class MyOwnException : Exception
        {
            public Student ThrownBy { get; set; }
        }


        [TestMethod]
        public void ExceptionsTests()
        {
            //Action - Check
            Action throwsInnerException = () => { throw new InvalidOperationException("Test", new ArgumentException("Something wrong")); };

            throwsInnerException.ShouldThrow<InvalidOperationException>()
                .WithInnerException<ArgumentException>()
                .WithInnerMessage("*wrong");

            Action throwsNormalException = () => { throw new NotSupportedException("This operation is not supported"); };

            throwsNormalException.ShouldThrow<NotSupportedException>()
                .WithMessage("*not supported");

            throwsNormalException.ShouldThrow<NotSupportedException>().Where(e => e.Message.StartsWith("This"));

            Action throwsMyOwnException = () => { throw new MyOwnException { ThrownBy = new Student { Name = "Sumanesh" } }; };
            throwsMyOwnException.ShouldThrow<MyOwnException>()
                .And.ThrownBy.Name.Should().Be("Sumanesh");

        }

        [TestMethod]
        public void Advanced_ComparingDifferentClasses()
        {
            Student student = new Student { Name = "Sumanesh" };
            Member m = new Member { Name = "Sumanesh" };

            student.ShouldBeEquivalentTo(m,opt=>opt.Including(ctx=>ctx.Name));
        }
    }
}
