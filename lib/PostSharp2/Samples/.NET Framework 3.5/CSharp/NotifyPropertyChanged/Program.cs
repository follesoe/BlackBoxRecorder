using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using PostSharp;

namespace NotifyPropertyChanged
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDerivedClass c = new TestDerivedClass();
            Post.Cast<TestDerivedClass, INotifyPropertyChanged>( c ).PropertyChanged += OnPropertyChanged;
            c.PropertyA = "Hello";
            c.PropertyB = 5;
        }

        private static void OnPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            Console.WriteLine("Property changed: {0}", e.PropertyName);
        }
    }
}
