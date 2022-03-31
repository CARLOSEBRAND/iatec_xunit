using System;

namespace EmprestimoBancarioTests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestPriorityAttribute : Attribute
    {
        public TestPriorityAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; private set; }
    }
}