using System;
using BlackBox.Tests.Testing;
using Xunit;
using Xunit.Extensions;

using BlackBox.Tests.Fakes;
using System.Collections.Generic;

namespace BlackBox.Tests
{
    public class TypeExtensionTest
    {
        [Fact]
        public void Can_get_code_definition_of_a_type()
        {
            var contact = new Contact();
            contact.GetType().GetCodeDefinition().ShouldEqual("BlackBox.Tests.Fakes.Contact");
        }

        [Fact]
        public void Can_get_code_definition_of_a_generic_type()
        {
            var list = new List<Contact>();
            list.GetType().GetCodeDefinition().ShouldEqual("System.Collections.Generic.List<BlackBox.Tests.Fakes.Contact>");
        }
    }
}
