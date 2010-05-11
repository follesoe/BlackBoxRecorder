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
            Given.A_comparison_test_object();
            When.We_compare_true_and_true();
            Then.Nothing();
        }

        [Fact]
        public void Comparing_true_and_false_yields_an_exception()
        {
            Given.A_comparison_test_object();
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

        private void A_comparison_test_object()
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
        private Exception thrownException;
        private string exceptionMessage;
    }
}
