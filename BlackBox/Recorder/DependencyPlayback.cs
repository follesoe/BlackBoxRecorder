using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace BlackBox.Recorder
{
    public class DependencyPlayback
    {
        [ThreadStatic]
        private static Dictionary<MethodInfo, Queue<object>> _playbackValues;

        public void RegisterExpectedReturnValue(MethodInfo method, object returnValue)
        {
            InitializeStore();
            var interceptedMethod = GetInterceptedMethod(method);

            if (!_playbackValues.ContainsKey(interceptedMethod))
            {
                _playbackValues.Add(method, new Queue<object>());
            }

            _playbackValues[interceptedMethod].Enqueue(returnValue);         
        }

        public bool HasReturnValue(MethodInfo method)
        {
            InitializeStore();
            var interceptedMethod = GetInterceptedMethod(method);
            return _playbackValues.ContainsKey(interceptedMethod) && _playbackValues[interceptedMethod].Count > 0;
        }

        public object GetReturnValue(MethodInfo method)
        {
            InitializeStore();
            var interceptedMethod = GetInterceptedMethod(method);
            return _playbackValues[interceptedMethod].Dequeue();
        }

        private static void InitializeStore()
        {
            if (_playbackValues == null)
            {
                _playbackValues = new Dictionary<MethodInfo, Queue<object>>();
            }
        }

        private static MethodInfo GetInterceptedMethod(MethodInfo interceptionMethod)
        {
            var parameterTypes = from parameter in interceptionMethod.GetParameters()
                                 select parameter.ParameterType;

            return interceptionMethod.DeclaringType.GetMethod(interceptionMethod.GetMethodNameWithoutTilde(), parameterTypes.ToArray());
        }

        public void Clear()
        {
            if(_playbackValues != null)
                _playbackValues.Clear();
        }
    }
}