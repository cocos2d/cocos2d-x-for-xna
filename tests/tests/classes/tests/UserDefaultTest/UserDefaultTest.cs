using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class UserDefaultTest : CCLayer
    {
        public UserDefaultTest() { }
        private void doTest()
        {
            Debug.WriteLine("********************** init value ***********************");

            // set default value

            //CCUserDefault.sharedUserDefault().setStringForKey("string", "value1");
            //CCUserDefault.sharedUserDefault().setIntegerForKey("integer", 10);
            //CCUserDefault.sharedUserDefault().setFloatForKey("float", 2.3f);
            //CCUserDefault.sharedUserDefault().setDoubleForKey("double", 2.4);
            //CCUserDefault.sharedUserDefault().setBoolForKey("bool", true);

            // print value

        //    string ret = CCUserDefault.sharedUserDefault()->getStringForKey("string");
        //    Debug.WriteLine("string is {0}", ret);

        //    double d = CCUserDefault.sharedUserDefault()->getDoubleForKey("double");
        //    Debug.WriteLine("double is {0}", d);

        //    int i = CCUserDefault.sharedUserDefault()->getIntegerForKey("integer");
        //    Debug.WriteLine("integer is {0}", i);

        //    float f = CCUserDefault.sharedUserDefault()->getFloatForKey("float");
        //    Debug.WriteLine("float is {0}", f);

        //    bool b = CCUserDefault.sharedUserDefault()->getBoolForKey("bool");
        //    if (b)
        //    {
        //        Debug.WriteLine("bool is true");
        //    }
        //    else
        //    {
        //        Debug.WriteLine("bool is false");
        //    }

        //    Debug.WriteLine("********************** after change value ***********************");

        //    // change the value

        //    CCUserDefault.sharedUserDefault().setStringForKey("string", "value2");
        //    CCUserDefault.sharedUserDefault().setIntegerForKey("integer", 11);
        //    CCUserDefault.sharedUserDefault().setFloatForKey("float", 2.5f);
        //    CCUserDefault.sharedUserDefault().setDoubleForKey("double", 2.6);
        //    CCUserDefault.sharedUserDefault().setBoolForKey("bool", false);

        //    // print value

        //    ret = CCUserDefault.sharedUserDefault().getStringForKey("string");
        //    Debug.WriteLine("string is %s", ret);

        //    d = CCUserDefault.sharedUserDefault().getDoubleForKey("double");
        //    Debug.WriteLine("double is %f", d);

        //    i = CCUserDefault.sharedUserDefault().getIntegerForKey("integer");
        //    Debug.WriteLine("integer is %d", i);

        //    f = CCUserDefault.sharedUserDefault().getFloatForKey("float");
        //    Debug.WriteLine("float is %f", f);

        //    b = CCUserDefault.sharedUserDefault().getBoolForKey("bool");
        //    if (b)
        //    {
        //        Debug.WriteLine("bool is true");
        //    }
        //    else
        //    {
        //        Debug.WriteLine("bool is false");
        //    }
        }
    }
}
