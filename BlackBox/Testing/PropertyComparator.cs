using System;
using System.Reflection;

namespace BlackBox.Testing
{
	public class PropertyComparator
	{
		public MemberInfo Property { get; set; }

		public MulticastDelegate Comparator { get; set; }
	}
}