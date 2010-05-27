using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using System.Collections.Generic;
using Microsoft.Test.ObjectComparison;

namespace BlackBox.Testing
{
    public class CharacterizationTest
    {
        private readonly RecordingXmlReader _reader;
        private readonly List<ParameterRecording> _inputParameters;
        private readonly List<ParameterRecording> _outputParameters;
        private readonly ObjectComparer _objectComparer;
        private readonly List<MemberInfo> _typePropertiesToIgnore;
        private readonly Dictionary<object, List<MemberInfo>> _instancePropertiesToIgnore;
        private readonly List<PropertyComparator> _customTypePropertyComparisons;
        private readonly Dictionary<object, List<PropertyComparator>> _customInstancePropertyComparisons;
       
        public CharacterizationTest()
        {
            _reader = new RecordingXmlReader();
            _inputParameters = new List<ParameterRecording>();
            _outputParameters = new List<ParameterRecording>();
            _objectComparer = new ObjectComparer(new PublicPropertyObjectGraphFactory());
            _typePropertiesToIgnore = new List<MemberInfo>();
            _instancePropertiesToIgnore = new Dictionary<object, List<MemberInfo>>();
            _customTypePropertyComparisons = new List<PropertyComparator>();
            _customInstancePropertyComparisons = new Dictionary<object, List<PropertyComparator>>();
        }

        public void LoadRecording(string path)
        {
            LoadRecording(XDocument.Load(path));
        }

        public void LoadRecording(XDocument recording)
        {
            _inputParameters.Clear();
            _reader.LoadRecording(recording);
            LoadDependencyReturnValues();
        }

        private void LoadDependencyReturnValues()
        {
            List<DependencyRecording> recordedDependencies = _reader.GetDependencies();

            foreach (var dependency in recordedDependencies)
            {
                foreach (var returnValue in dependency.ReturnValues)
                {
                    RecordingServices.DependencyPlayback.RegisterExpectedReturnValue(dependency.Method, returnValue);
                }
            }
        }

        protected virtual void ConfigureComparison(string filename)
        {
        }

        public object GetInputParameterValue(string parameterName)
        {
            if (_inputParameters.Count == 0)
            {
                _inputParameters.AddRange(_reader.GetInputParameters());
            }
            return _inputParameters.Where(p => p.Name == parameterName).SingleOrDefault().Value;
        }

        public object GetOutputParameterValue(string parameterName)
        {
            if(_outputParameters.Count == 0)
            {
                _outputParameters.AddRange(_reader.GetOutputParameters());
            }

            return _outputParameters.Where(p => p.Name == parameterName).SingleOrDefault().Value;
        }

        public object GetReturnValue()
        {
            return _reader.GetReturnValue();   
        }

        public void CompareObjects(object expected, object actual)
        {
            IEnumerable<ObjectComparisonMismatch> mismatches;
            _objectComparer.Compare(expected,
                                    actual,
                                    _typePropertiesToIgnore,
                                    _instancePropertiesToIgnore,
                                    _customTypePropertyComparisons,
                                    _customInstancePropertyComparisons,
                                    out mismatches);
            if (mismatches.Any())
                throw new ObjectMismatchException(mismatches);
        }

        public void Initialize()
        {
            Configuration.RecordingMode = RecordingMode.Playback;
        }

        public void Ignore<TType, TPropertyType>(TType anObject, Expression<Func<TType, TPropertyType>> propertySelector)
        {
            MemberExpression memberExpression = GetMemberExpression(propertySelector);
            if(!_instancePropertiesToIgnore.ContainsKey(anObject))
                _instancePropertiesToIgnore.Add(anObject, new List<MemberInfo>());
            _instancePropertiesToIgnore[anObject].Add(memberExpression.Member);
        }

        public void Ignore<TType, TPropertyType>(IEnumerable<TType> aSetOfObjects, Expression<Func<TType, TPropertyType>> propertySelector)
        {
            aSetOfObjects.ToList().ForEach(o => Ignore(o, propertySelector));
        }

        public void IgnoreOnType<TType, TPropertyType>(Expression<Func<TType, TPropertyType>> propertySelector)
        {
            MemberExpression memberExpression = GetMemberExpression(propertySelector);
           _typePropertiesToIgnore.Add(memberExpression.Member); 
        }

        public void AllowOnType<TType, TPropertyType>(Expression<Func<TType, TPropertyType>> propertySelector, Predicate<TPropertyType> propertyComparison)
        {
            MemberExpression memberExpression = GetMemberExpression(propertySelector);
            _customTypePropertyComparisons.Add(new PropertyComparator
                                               {
                                                   Property = memberExpression.Member,
                                                   Comparator = propertyComparison
                                               });
        }

        public void Allow<TType, TPropertyType>(TType anObject, Expression<Func<TType, TPropertyType>> propertySelector, Predicate<TPropertyType> propertyComparison)
        {
            MemberExpression memberExpression = GetMemberExpression(propertySelector);
            if(!_customInstancePropertyComparisons.ContainsKey(anObject))
                _customInstancePropertyComparisons.Add(anObject, new List<PropertyComparator>());
            _customInstancePropertyComparisons[anObject].Add(new PropertyComparator
                                                                 {
                                                                     Property = memberExpression.Member,
                                                                     Comparator = propertyComparison
                                                                 });
        }

        private static MemberExpression GetMemberExpression<TType, TPropertyType>(Expression<Func<TType, TPropertyType>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException(string.Format("{0} is not a valid member expression.", expression));

            return memberExpression;            
        }
    }
}