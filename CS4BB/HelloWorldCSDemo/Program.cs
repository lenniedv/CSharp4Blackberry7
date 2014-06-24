using System;
using net.rim.device.api.ui;

namespace HelloWorldCSDemo
{
    class HelloWorldCSDemo : UiApplication
    {
        public HelloWorldCSDemo()
        {
            HelloWorldMainScreen mainScreen = new HelloWorldMainScreen();
            base.pushScreen(mainScreen);
        }

        static void Main(string[] args)
        {
            HelloWorldCSDemo app = new HelloWorldCSDemo();
            app.enterEventDispatcher();
        }
    }
}
