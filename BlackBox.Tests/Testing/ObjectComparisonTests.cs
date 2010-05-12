using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BlackBox.Testing;
using Microsoft.Test.ObjectComparison;
using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.Testing
{
    public class ObjectComparisonTests : BDD<ObjectComparisonTests>
    {
        [Fact]
        public void Comparing_true_and_true_does_not_yield_any_exception()
        {
            Given.A_comparator();
            When.We_compare_true_and_true();
            Then.Nothing();
        }

        [Fact]
        public void Comparing_true_and_false_yields_an_exception()
        {
            Given.A_comparator();
            When.We_compare_true_and_false();
            Then.We_get_the_appropriate_exception();
        }

        [Fact]
        public void The_mismatch_exception_object_message_contains_information_about_the_mismatch()
        {
            Given.A_mismatch_exception_object_for_a_specific_comparison();
            When.We_get_the_message_of_the_exception();
            Then.It_tells_us_what_the_difference_is();
        }

        [Fact]
        public void Do_not_follow_circular_references()
        {
            Given.A_comparator();
            When.We_compare_two_objects_with_circular_references();
            Then.Nothing();
        }

        [Fact]
        public void We_do_not_want_to_compare_the_Capacity_or_other_properties_when_comparing_IEnumerables()
        {
            Given.A_comparator();
            When.We_compare_two_lists_with_equal_objects_but_different_capacity();
            Then.Nothing();
        }

        [Fact]
        public void Comparing_two_lists_of_different_objects_yields_all_nested_differences()
        {
            Given.A_comparator();
            When.We_compare_two_lists_with_two_objects_each_that_differ_in_two_properties_each();
            Then.The_exception_message_contains_all_four_differences();
        }

        [Fact]
        public void Can_exclude_a_certain_property_in_a_simple_comparison()
        {
            Given.A_property_we_wish_to_ignore();
            When.We_compare_two_objects_that_differ_on_that_property();
            Then.Nothing();
        }

        private void A_property_we_wish_to_ignore()
        {
            propertyToIgnore = "RootObject.MyBoolean";
        }

        private void We_compare_two_objects_that_differ_on_that_property()
        {
            var someObject = new ObjectWithValueTypeProperties();
            var anotherObject = new ObjectWithValueTypeProperties { MyBoolean = true };
            var test = new CharacterizationTest();
            test.Ignore(propertyToIgnore);
            test.CompareObjects(someObject, anotherObject);
        }

        private void We_compare_two_lists_with_two_objects_each_that_differ_in_two_properties_each()
        {
            thrownException = Record.Exception(() => Compare_two_lists_with_two_objects_each_that_differ_in_two_properties_each());
        }

        private void Compare_two_lists_with_two_objects_each_that_differ_in_two_properties_each()
        {
            var aListElement = new ObjectWithValueTypeProperties();
            var anotherListElement = new ObjectWithValueTypeProperties { MyBoolean = true, MyInteger = 1 };
            var aSecondListElement = new ObjectWithValueTypeProperties();
            var another2ListElement = new ObjectWithValueTypeProperties { MyBoolean = true, MyInteger = 1 };

            var aList = new List<ObjectWithValueTypeProperties>
                            {
                                new ObjectWithValueTypeProperties(),
                                new ObjectWithValueTypeProperties {MyBoolean = true, MyInteger = 1}
                            };
            var anotherList = new List<ObjectWithValueTypeProperties>
                                  {
                                      new ObjectWithValueTypeProperties {MyByte = 1, MyChar = 'a'},
                                      new ObjectWithValueTypeProperties()
                                  };

            new CharacterizationTest().CompareObjects(aList, anotherList);
        }

        private void The_exception_message_contains_all_four_differences()
        {
            exceptionMessage = thrownException.Message;
            Assert.Contains("ObjectValuesDoNotMatch: LeftNodeName=RootObject.IEnumerable0.MyByte", exceptionMessage);
            Assert.Contains("ObjectValuesDoNotMatch: LeftNodeName=RootObject.IEnumerable0.MyChar", exceptionMessage);
            Assert.Contains("ObjectValuesDoNotMatch: LeftNodeName=RootObject.IEnumerable1.MyBoolean", exceptionMessage);
            Assert.Contains("ObjectValuesDoNotMatch: LeftNodeName=RootObject.IEnumerable1.MyInteger", exceptionMessage);
        }

        private void We_compare_two_lists_with_equal_objects_but_different_capacity()
        {
            var aList = new List<int>(1) {1};
            var anotherList = new List<int>(2) {1};
            new CharacterizationTest().CompareObjects(aList, anotherList);
        }

        private void We_compare_two_objects_with_circular_references()
        {
            var someObject = new ObjectWithSelfReference();
            new CharacterizationTest().CompareObjects(someObject, someObject);
        }

        private void A_mismatch_exception_object_for_a_specific_comparison()
        {
            thrownException = Record.Exception(() => Compare_two_objects_with_value_type_properties());
        }

        private void Compare_two_objects_with_value_type_properties()
        {
            var someObject = new ObjectWithValueTypeProperties();
            var anotherObject = new ObjectWithValueTypeProperties { MyBoolean = true };
            new CharacterizationTest().CompareObjects(someObject, anotherObject);
        }

        private void We_get_the_message_of_the_exception()
        {
            exceptionMessage = thrownException.Message;
        }

        private void It_tells_us_what_the_difference_is()
        {
            Assert.Contains("ObjectValuesDoNotMatch: LeftNodeName=RootObject.MyBoolean", exceptionMessage);
        }

        private void A_comparator()
        {
            // WTF? If I instantiate CharacterizationTest here, 
            // I *sometimes* get a null reference when I reference it later.
            //
            //test = new CharacterizationTest();
        }

        private void We_compare_true_and_true()
        {
            new CharacterizationTest().CompareObjects(true, true);
        }

        private void Nothing()
        {
        }

        private void Compare_true_and_false()
        {
            new CharacterizationTest().CompareObjects(true, false);
        }

        private void We_compare_true_and_false()
        {
            thrownException = Record.Exception(() => Compare_true_and_false());
        }

        private void We_get_the_appropriate_exception()
        {
            Assert.IsType(typeof(ObjectMismatchException), thrownException);
        }

        //private CharacterizationTest test;
        private string propertyToIgnore;
        private Exception thrownException;
        private string exceptionMessage;
    }
}
