using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackBox.Tests.Testing;
using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests
{
    public class TypeToolsTests
    {
        [Fact]
        public void Can_construct_a_type_from_a_string()
        {
            Type type = TypeTools.UnwrapType("System.DateTime");
            type.ShouldEqual(typeof(DateTime));
        }

        [Fact]
        public void Can_retrieve_the_generic_type_parameter_from_a_list_of_DateTimes()
        {
            var collection = new List<DateTime>();
            Type type = TypeTools.UnwrapType(collection.GetType().ToString());
            type.ShouldEqual(typeof(DateTime));
        }

        [Fact]
        public void Can_get_some_property_name_from_a_type()
        {
            Type type = typeof (ObjectWithDateTimeProperty);
            string propertyName = TypeTools.GetSomePublicPropertyName(type);
            propertyName.ShouldEqual("MyDay");
        }

        [Fact]
        public void Will_produce_a_nonexistant_property_name_if_no_public_instance_props_are_available()
        {
            Type type = typeof(ObjectWithNoProperties);
            string propertyName = TypeTools.GetSomePublicPropertyName(type);
            propertyName.ShouldEqual("NoPublicInstancePropertiesAvailable");
        }
    }
}
