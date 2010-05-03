using System;
using System.Collections.Generic;

namespace BlackBox.Recorder
{
    public static class RecordingStack
    {
        [ThreadStatic] private static Stack<Guid> MethodExecutionTag;

        public static int Count
        {
            get
            {
                CreateIfNull();
                return MethodExecutionTag.Count;
            }
        }

        public static void Push(Guid callGuid)
        {
            CreateIfNull();
            MethodExecutionTag.Push(callGuid);
        }

        public static Guid Pop()
        {
            return MethodExecutionTag.Pop();
        }

        public static Guid Peek()
        {
            return MethodExecutionTag.Peek();
        }

        private static void CreateIfNull()
        {
            if(MethodExecutionTag == null)
                MethodExecutionTag = new Stack<Guid>();
        }
    }
}
