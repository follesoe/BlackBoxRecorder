using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BlackBox.Tests.Testing
{
    public enum OneOrTwoOrThree
    {
        One,
        Two,
        Three
    }

    public class ObjectWithSelfReference
    {
        public ObjectWithSelfReference Self { get { return this; } }
    }

    public class ObjectWithEnumProperty
    {
        public OneOrTwoOrThree MyEnumeration { get; set; }
    }

    public class ObjectWithNoProperties
    {
    }

    public class ObjectWithDateTimeProperty
    {
        public DateTime MyDay { get; set; }
    }

    public class ObjectWithNullableProperty
    {
        public int? MyMaybeAnInt { get; set; }
    }

    public class ObjectWithValueTypeProperties
    {
        public bool MyBoolean { get; set; }
        public byte MyByte { get; set; }
        public char MyChar { get; set; }
        public decimal MyDecimal { get; set; }
        public double MyDouble { get; set; }
        public float MyFloat { get; set; }
        public int MyInteger { get; set; }
        public long MyLong { get; set; }
        public sbyte MySignedByte { get; set; }
        public short MyShort { get; set; }
        public ulong MyUnsignedLong { get; set; }
        public uint MyUnsignedInt { get; set; }
        public ushort MyUnsignedShort { get; set; }
    }

    public class ObjectWithMixedTypeProperties
    {
        public string MyString { get; set; }
        public int MyInteger { get; set; }
        public ObjectWithValueTypeProperties MySimpleObject { get; set; }
    }

    public class ObjectWithListTypeProperty
    {
        public List<object> MyList { get; set; }
    }

    public class PrettyLargeAndComplexTestObject
    {
        public ObjectWithValueTypeProperties MyObjectWithValues { get; set; }
        public Dictionary<string, int> MyDictionary { get; set; }
        public XDocument MyXmlDocument { get; set; }
        public int? MyNullableInt { get; set; }
    }

    public class ObjectWithListOfObjectsWithValueTypeProperties
    {
        public List<ObjectWithValueTypeProperties> MyList { get; set; }
    }
}