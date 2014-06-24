using System;
using net.rim.device.api.ui;

namespace InterfacesDemo
{
    class InterfacesDemo : UiApplication, IAddCalculate, ISubtractCalculate
    {
        public InterfacesDemo()
        {
            
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

        static void Main(string[] args)
        {
            InterfacesDemo demo = new InterfacesDemo();
            demo.Add(1, 2);
            demo.Subtract(10, 5);
        }
    }
}
