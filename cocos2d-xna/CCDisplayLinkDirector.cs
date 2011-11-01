using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    /***************************************************
    * implementation of DisplayLinkDirector
    **************************************************/

    // should we afford 4 types of director ??
    // I think DisplayLinkDirector is enough
    // so we now only support DisplayLinkDirector

    public class CCDisplayLinkDirector : CCDirector
    {
        bool m_bInvalid;

        public override void stopAnimation()
        {
            m_bInvalid = true;
        }

        public override void startAnimation()
        {
            sharedDirector().animationInterval = m_dAnimationInterval;
        }

        public override void mainLoop(GameTime gameTime)
        {
            if (m_bPurgeDirecotorInNextLoop)
            {
                purgeDirector();
                m_bPurgeDirecotorInNextLoop = false;
            }
            else if (!m_bInvalid)
            {
                drawScene();
            }
        }

        public override double animationInterval
        {
            get
            {
                return base.animationInterval;
            }
            set
            {
                m_dAnimationInterval = value;
                if (!m_bInvalid)
                {
                    stopAnimation();
                    startAnimation();
                }
            }
        }
    }
}
