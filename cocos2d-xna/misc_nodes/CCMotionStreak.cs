using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCMotionStreak : CCNode, ICCTextureProtocol
    {
        //        CC_PROPERTY_READONLY(CCRibbon*, m_pRibbon, Ribbon)
        ////CCTextureProtocol methods
        //CC_PROPERTY(CCTexture2D*, m_pTexture, Texture)
        //CC_PROPERTY(ccBlendFunc, m_tBlendFunc, BlendFunc)
        public CCTexture2D Texture
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ccBlendFunc BlendFunc
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
