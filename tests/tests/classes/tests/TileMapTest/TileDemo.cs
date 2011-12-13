using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TileDemo : CCLayer
    {
        public TileDemo()
        {
        }

        public virtual string title()
        {
            return "No tile";
        }

        public virtual string subtitle()
        {
            return "drag the screen";
        }

        public virtual void onEnter()
        {
            base.onEnter();
        }
    }
}
