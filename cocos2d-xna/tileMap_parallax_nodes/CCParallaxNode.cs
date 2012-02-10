/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
Copyright (c) 2011-2012 Fulcrum Mobile Network, Inc

http://www.cocos2d-x.org
http://www.openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace cocos2d
{

    public class CCParallaxNode : CCNode
    {
        /// <summary>
        /// array that holds the offset / ratio of the children
        /// </summary>
        protected List<CCPointObject> m_pParallaxArray;
        public List<CCPointObject> ParallaxArray
        {
            get { return m_pParallaxArray; }
            set { m_pParallaxArray = value; }
        }

        /// <summary>
        ///  Adds a child to the container with a z-order, a parallax ratio and a position offset
        /// It returns self, so you can chain several addChilds.
        /// @since v0.8
        /// </summary>
        public CCParallaxNode()
        {
            m_pParallaxArray = new List<CCPointObject>();
            m_tLastPosition = new CCPoint(-100, -100);
        }
        public static CCParallaxNode node()
        {
            CCParallaxNode pRet = new CCParallaxNode();
            //pRet->autorelease();
            return pRet;
        }

        // super methods
        public virtual void addChild(CCNode child, UInt32 zOrder, int tag)
        {
            //    CC_UNUSED_PARAM(zOrder);
            //    CC_UNUSED_PARAM(child);
            //    CC_UNUSED_PARAM(tag);
            Debug.Assert(false, "ParallaxNode: use addChild:z:parallaxRatio:positionOffset instead");
        }

        public virtual void addChild(CCNode child, UInt32 z, CCPoint parallaxRatio, CCPoint positionOffset)
        {
            Debug.Assert(child != null, "Argument must be non-nil");
            CCPointObject obj = CCPointObject.pointWithCCPoint(parallaxRatio, positionOffset);
            obj.Child = child;
            //ccCArray.ccArrayAppendObjectWithResize(m_pParallaxArray, (CCObject)obj);

            CCPoint pos = m_tPosition;
            pos.x = pos.x * parallaxRatio.x + positionOffset.x;
            pos.y = pos.y * parallaxRatio.y + positionOffset.y;
            child.position = pos;

            base.addChild(child, (int)z, child.tag);
        }

        public virtual void removeChild(CCNode child, bool cleanup)
        {
            for (int i = 0; i < m_pParallaxArray.Count; i++)
            {
                CCPointObject point = (CCPointObject)m_pParallaxArray[i];
                if (point.Child == child)
                {
                    //ccCArray.ccArrayRemoveObjectAtIndex(m_pParallaxArray, i);
                    break;
                }
            }
            base.removeChild(child, cleanup);
        }
        public virtual void removeAllChildrenWithCleanup(bool cleanup)
        {
            //ccCArray.ccArrayRemoveAllObjects(m_pParallaxArray);
            base.removeAllChildrenWithCleanup(cleanup);
        }
        public virtual void visit()
        {
            //	CCPoint pos = position_;
            //	CCPoint	pos = [self convertToWorldSpace:CCPointZero];
            CCPoint pos = this.absolutePosition();
            if (!CCPoint.CCPointEqualToPoint(pos, m_tLastPosition))
            {
                for (int i = 0; i < m_pParallaxArray.Count; i++)
                {
                    CCPointObject point = (CCPointObject)(m_pParallaxArray[i]);
                    float x = -pos.x + pos.x * point.Ratio.x + point.Offset.x;
                    float y = -pos.y + pos.y * point.Ratio.y + point.Offset.y;
                    point.Child.position = new CCPoint(x, y);
                }
                m_tLastPosition = pos;
            }
            base.visit();
        }
        private CCPoint absolutePosition()
        {
            CCPoint ret = m_tPosition;
            CCNode cn = this;
            while (cn.parent != null)
            {
                cn = cn.parent;
                ret = new CCPoint(ret.x + position.x, ret.y + position.y);
            }
            return ret;
        }

        protected CCPoint m_tLastPosition;
    }
}
