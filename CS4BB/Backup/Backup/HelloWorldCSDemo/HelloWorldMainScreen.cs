using System;
using System.Text;
using net.rim.device.api.ui.container;
using net.rim.device.api.ui.component;

namespace HelloWorldCSDemo
{
    public class HelloWorldMainScreen: MainScreen
    {
        public HelloWorldMainScreen()
        {
            LabelField labelField = new LabelField("Hello World");
            add(labelField);
        }
    }
}
