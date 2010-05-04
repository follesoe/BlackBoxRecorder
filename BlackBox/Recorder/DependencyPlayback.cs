using System;
using System.Reflection;
using System.Collections.Generic;

namespace BlackBox.Recorder
{
    public class DependencyPlayback
    {
        [ThreadStatic]
        private static Dictionary<MethodInfo, Stack<object>> _playbackValues;

        public void RegisterExpectedReturnValue(MethodInfo method, object returnValue)
        {
            if (_playbackValues == null)
            {
                _playbackValues = new Dictionary<MethodInfo, Stack<object>>();
            }

            if(!_playbackValues.ContainsKey(method))
            {
                _playbackValues.Add(method, new Stack<object>());
            }

            _playbackValues[method].Push(returnValue);
        }

        public bool HasReturnValue(MethodInfo method)
        {
            return _playbackValues.ContainsKey(method) && _playbackValues[method].Count > 0;
        }

        public object GetReturnValue(MethodInfo method)
        {
            return _playbackValues[method].Pop();
        }
    }
}