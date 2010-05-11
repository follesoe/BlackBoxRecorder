using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.Test.ObjectComparison;

namespace BlackBox.Testing
{
    public class Comparator<T>
    {
        //private readonly ObjectComparer<T> _comparer;
        private readonly List<PropertyInfo> _propertiesToIgnore;

        public Comparator()
        {
            //_comparer = new ObjectComparer<T>(new PublicPropertyObjectGraphFactory());
            _propertiesToIgnore = new List<PropertyInfo>();
        }


        public IEnumerable<ObjectComparisonMismatch> Compare<TProperty>(T original, T copy, Expression<Func<T, TProperty>> propertyLambda)
        {
            Debugger.Break();
            if (original == null && copy == null)
                yield break;

            IEnumerable<ObjectComparisonMismatch> mismatches;
            //_comparer.Compare(original, copy, out mismatches);

            //var w = mismatches.First().LeftObjectNode.Selector;
            //var x = propertyLambda;
            //bool b = w.Equals(x);
            //var x = w.Compile();
            //var z = x.DynamicInvoke(original);

            //foreach (var mismatch in mismatches)
            //    yield return mismatch;
        }

        public IEnumerable<ObjectComparisonMismatch> Compare(T original, T copy)
        {
            if (original == null && copy == null)
                yield break;

            IEnumerable<ObjectComparisonMismatch> mismatches;
            //_comparer.Compare(original, copy, out mismatches);

            //var w = mismatches.First().LeftObjectNode.Selector;
            //var x = w.Compile();
            //var z = x.DynamicInvoke(original);
            
            //foreach (var mismatch in mismatches)
            //    yield return mismatch;
        }

        //public void Ignore<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        //{
        //    MemberExpression exp = propertyLambda.Body as MemberExpression;
        //    StringBuilder sb = new StringBuilder();
        //    while (exp != null)
        //    {
        //        string propertyName = exp.Member.Name;
        //        Type propertyType = exp.Type;

        //        sb.AppendLine(propertyName + ": " + propertyType);

        //        exp = exp.Expression as MemberExpression;
        //    } 
        //    PropertyInfo pi = GetPropertyInfo(propertyLambda);
        //    _propertiesToIgnore.Add(pi);
        //}

        //private PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        //{
        //    Type type = typeof (T);

        //    MemberExpression member = propertyLambda.Body as MemberExpression;
        //    if (member == null)
        //        throw new ArgumentException(string.Format(
        //                                        "Expression '{0}' refers to a method, not a property.",
        //                                        propertyLambda.ToString()));

        //    PropertyInfo propInfo = member.Member as PropertyInfo;
        //    if (propInfo == null)
        //        throw new ArgumentException(string.Format(
        //                                        "Expression '{0}' refers to a field, not a property.",
        //                                        propertyLambda.ToString()));

        //    if (type != propInfo.ReflectedType &&
        //        !type.IsSubclassOf(propInfo.ReflectedType))
        //        throw new ArgumentException(string.Format(
        //                                        "Expresion '{0}' refers to a property that is not from type {1}.",
        //                                        propertyLambda.ToString(),
        //                                        type));

        //    return propInfo;
        //}

        //public void Ignore<TPropertyType>(Expression<Func<T, TPropertyType>> func)
        //{
        //    //_comparer.Ignore(func);
        //    //string propertyPath = GetPropertyPath(func);
        //    //AddPropertyToIgnore(propertyPath);
        //}

        //public void IgnoreItem<TPropertyType>(Expression<Func<T, TPropertyType>> func, int index) 
        //    where TPropertyType : IEnumerable
        //{
        //    string propertyPath = GetPropertyPath(func);
        //    propertyPath += ".IEnumerable" + index;
        //    AddPropertyToIgnore(propertyPath);
        //} 

        //private string GetPropertyPath(Expression x)
        //{
        //    var propertyPath = ROOT_OBJECT_NAME + "." +
        //                       x.ToString().Remove(0, x.ToString().IndexOf('.') + 1);
        //    return propertyPath;
        //}

        //private void AddPropertyToIgnore(string memberName)
        //{
        //    _propertiesToIgnore.Add(memberName);
        //}

        //private bool DoNotIgnore(ObjectComparisonMismatch mismatch)
        //{
        //    return mismatch.LeftObjectNode != null && !_propertiesToIgnore.Contains(mismatch.LeftObjectNode.QualifiedName);
        //}
    }
}
