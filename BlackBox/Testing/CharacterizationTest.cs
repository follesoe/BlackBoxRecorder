using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Collections.Generic;

namespace BlackBox.Testing
{
    public class CharacterizationTest
    {
        private readonly RecordingXmlReader _reader;
        private readonly List<ParameterRecording> _parameters;
       
        public CharacterizationTest()
        {
            _reader = new RecordingXmlReader();
            _parameters = new List<ParameterRecording>();
        }

        public void LoadRecording(string path)
        {
            LoadRecording(XDocument.Load(path));
        }

        public void LoadRecording(XDocument recording)
        {
            _parameters.Clear();
            _reader.LoadRecording(recording);
        }

        public object GetInputParameterValue(string parameterName)
        {
            if (_parameters.Count == 0)
            {
                _parameters.AddRange(_reader.GetInputParameters());
            }
            return _parameters.Where(p => p.Name == parameterName).SingleOrDefault().Value;
        }

        public object GetReturnValue()
        {
            return _reader.GetReturnValue();   
        }

        public void CompareObjects(object expected, object actuall)
        {
            var differences = new List<PropertyDifference>();
            CompareObjects(expected, actuall, differences, 0);

            if(differences.Count > 0)
            {
                Console.WriteLine("---Objects not identical!---");
            }
            foreach(var difference in differences)
            {
                Console.WriteLine("\tProperty: {0}\tExpected: {1}\tActuall: {2}", difference.Navn, difference.VerdiA, difference.VerdiB);
            }

            if(differences.Count > 0) throw new Exception("Objects not identical!");
        }

        private void CompareObjects(object a, object b, ICollection<PropertyDifference> forskjeller, int innrykk)
        {
            if (a == null && b == null) return;

            innrykk++;
            string innrykkPrefix = CreateIndentation(innrykk);

            var properties = a.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.PropertyType.GetInterfaces().Contains(typeof(IList)))
                {
                    var listA = (IList)property.GetValue(a, null);
                    var listB = (IList)property.GetValue(b, null);



                    for (int i = 0; i < listA.Count; ++i)
                    {
                        CompareObjects(listA[i], listB[i], forskjeller, innrykk);
                    }
                }
                else if (property.PropertyType.IsClass)
                {
                    CompareObjects(property.GetValue(a, null), property.GetValue(b, null), forskjeller, innrykk);
                }
                else
                {
                    object valueA;
                    object valueB;
                    if (a is string)
                    {
                        valueA = a.ToString();
                        valueB = b.ToString();
                    }
                    else
                    {
                        valueA = property.GetValue(a, null);
                        valueB = property.GetValue(b, null);
                    }
                    if (valueA is double)
                    {
                        var doubleA = (double)valueA;
                        var doubleB = (double)valueB;

                        if (Math.Abs(doubleA - doubleB) > 0.0001)
                        {
                            forskjeller.Add(new PropertyDifference(property.DeclaringType.Name + "." + property.Name, doubleA, doubleB));
                        }
                    }
                    else
                    {
                        if (!valueA.Equals(valueB))
                        {
                            forskjeller.Add(new PropertyDifference(property.Name, valueA, valueB));
                        }
                    }
                }
            }
        }

        private string CreateIndentation(int innrykk)
        {
            string innrykkString = string.Empty;
            for (int i = 0; i < innrykk; ++i)
                innrykkString += "\t";
            return innrykkString;
        }

        private class PropertyDifference
        {
            public string Navn { get; set; }
            public string VerdiA { get; set; }
            public string VerdiB { get; set; }

            public PropertyDifference(string navn, object a, object b)
            {
                Navn = navn;
                VerdiA = a.ToString();
                VerdiB = b.ToString();
            }
        }
    }
}