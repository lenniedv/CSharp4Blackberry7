using System;
using HelloWorldCSDemo;
using net.rim.device.api.ui;

namespace com.demo.HelloWorldCSDemo
{
    class HelloWorldCSDemo : UiApplication
    {
        public HelloWorldCSDemo()
        {
            HelloWorldMainScreen mainScreen = new HelloWorldMainScreen();
            pushScreen(mainScreen);
        }

        static void Main(string[] args)
        {
            HelloWorldCSDemo app = new HelloWorldCSDemo();
            app.enterEventDispatcher();
        }
    }
}
