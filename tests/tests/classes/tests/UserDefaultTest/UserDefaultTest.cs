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
            CCLog.Log("********************** init value ***********************");

            // set default value

            CCUserDefault.sharedUserDefault().setStringForKey("string", "value1");
            CCUserDefault.sharedUserDefault().setIntegerForKey("integer", 10);
            CCUserDefault.sharedUserDefault().setFloatForKey("float", 2.3f);
            CCUserDefault.sharedUserDefault().setDoubleForKey("double", 2.4);
            CCUserDefault.sharedUserDefault().setBoolForKey("bool", true);

            // print value

           string ret = CCUserDefault.sharedUserDefault().getStringForKey("string", null);
            CCLog.Log("string is {0}", ret);

            double d = CCUserDefault.sharedUserDefault().getDoubleForKey("double", 0.0);
            CCLog.Log("double is {0}", d);

            int i = CCUserDefault.sharedUserDefault().getIntegerForKey("integer", 0);
            CCLog.Log("integer is {0}", i);

           float f = CCUserDefault.sharedUserDefault().getFloatForKey("float", 0.0f);
            CCLog.Log("float is {0}", f);

            bool b = CCUserDefault.sharedUserDefault().getBoolForKey("bool", false);
            if (b)
            {
                CCLog.Log("bool is true");
            }
            else
            {
                CCLog.Log("bool is false");
            }

            CCLog.Log("********************** after change value ***********************");

            // change the value

            CCUserDefault.sharedUserDefault().setStringForKey("string", "value2");
            CCUserDefault.sharedUserDefault().setIntegerForKey("integer", 11);
            CCUserDefault.sharedUserDefault().setFloatForKey("float", 2.5f);
            CCUserDefault.sharedUserDefault().setDoubleForKey("double", 2.6);
            CCUserDefault.sharedUserDefault().setBoolForKey("bool", false);

            // print value

            ret = CCUserDefault.sharedUserDefault().getStringForKey("string", null);
            CCLog.Log("string is {0}, expecting 'value2'", ret);

            d = CCUserDefault.sharedUserDefault().getDoubleForKey("double", 0.0);
            CCLog.Log("double is {0} expecting 2.6", d);

            i = CCUserDefault.sharedUserDefault().getIntegerForKey("integer", 0);
           CCLog.Log("integer is {0}, expecting 11", i);

            f = CCUserDefault.sharedUserDefault().getFloatForKey("float", 0f);
            CCLog.Log("float is {0}, expecting 2.5", f);

            b = CCUserDefault.sharedUserDefault().getBoolForKey("bool", false);
            if (b)
            {
                CCLog.Log("bool is true, which is incorrect");
            }
           else
            {
                CCLog.Log("bool is false, which is correct");
            }
        }
    }
}
