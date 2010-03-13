using System.Xml.Linq;
using BlackBox.Tests.Fakes;

using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests
{
    public class SerializationExtensionTest
    {
        [Fact]
        public void Can_serialize_an_object_using_extension_method()
        {
            xml = contact.ToXml();
            xml.ShouldNotBeNull();
        }

        [Fact]
        public void Can_deserialize_an_object_using_extension_method()
        {
            xml = contact.ToXml();
            contact = (Contact)xml.ToString().Deserialize(contact.GetType());
        }

        private XNode xml;
        private Contact contact;

        public SerializationExtensionTest()
        {
            contact = new Contact("Jonas Follesø", "jonas@follesoe.no");    
        }
    }
}