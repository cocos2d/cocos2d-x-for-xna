using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SplitRowsDemo : CCSplitRows
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            return CCSplitRows.actionWithRows(9, t);
        }
    }
}
