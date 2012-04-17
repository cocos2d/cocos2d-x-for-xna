/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

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

using System.Diagnostics;

namespace cocos2d
{
    /// <summary>
    /// @brief CCFollow is an action that "follows" a node.
    /// Eg:
    /// layer->runAction(CCFollow::actionWithTarget(hero));
    /// Instead of using CCCamera as a "follower", use this action instead.
    /// @since v0.99.2
    /// </summary>
    public class CCFollow : CCAction
    {
        public CCFollow() { }

        ~CCFollow() { }

        public bool initWithTarget(CCNode followedNode, CCRect rect)
        {
            Debug.Assert(followedNode != null);

            m_pobFollowedNode = followedNode;
            m_bBoundarySet = true;
            m_bBoundaryFullyCovered = false;

            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            m_obFullScreenSize = new CCPoint(winSize.width, winSize.height);
            m_obHalfScreenSize = CCPointExtension.ccpMult(m_obFullScreenSize, 0.5f);

            m_fLeftBoundary = -((rect.origin.x + rect.size.width) - m_obFullScreenSize.x);
            m_fRightBoundary = -rect.origin.x;
            m_fLeftBoundary = -rect.origin.y;
            m_fBottomBoundary = -((rect.origin.y + rect.size.height) - m_obFullScreenSize.y);

            if (m_fRightBoundary < m_fLeftBoundary)
            {
                // screen width is larger than world's boundary width
                //set both in the middle of the world
                m_fRightBoundary = m_fLeftBoundary = (m_fLeftBoundary + m_fRightBoundary) / 2;
            }
            if (m_fTopBoundary < m_fBottomBoundary)
            {
                // screen width is larger than world's boundary width
                //set both in the middle of the world
                m_fTopBoundary = m_fBottomBoundary = (m_fTopBoundary + m_fBottomBoundary) / 2;
            }

            if ((m_fTopBoundary == m_fBottomBoundary) && (m_fLeftBoundary == m_fRightBoundary))
            {
                m_bBoundaryFullyCovered = true;
            }

            return true;
        }

        public bool initWithTarget(CCNode followedNode)
        {
            Debug.Assert(followedNode != null);

            m_pobFollowedNode = followedNode;
            m_bBoundarySet = false;
            m_bBoundaryFullyCovered = false;

            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            m_obFullScreenSize = new CCPoint(winSize.width, winSize.height);
            m_obHalfScreenSize = CCPointExtension.ccpMult(m_obFullScreenSize, 0.5f);

            return true;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tempZone = zone;
            CCFollow ret = null;
            if (tempZone != null && tempZone.m_pCopyObject != null)
            {
                ret = (CCFollow)tempZone.m_pCopyObject;
            }
            else
            {
                ret = new CCFollow();
                tempZone = new CCZone(ret);
            }

            base.copyWithZone(tempZone);
            ret.m_nTag = m_nTag;

            return ret;
        }

        public override void step(float dt)
        {
            if (m_bBoundarySet)
            {
                // whole map fits inside a single screen, no need to modify the position - unless map boundaries are increased
                if (m_bBoundaryFullyCovered)
                {
                    return;
                }

                CCPoint tempPos = CCPointExtension.ccpSub(m_obHalfScreenSize, m_pobFollowedNode.position);
                m_pTarget.position = CCPointExtension.ccp(CCPointExtension.clampf(tempPos.x, m_fLeftBoundary, m_fRightBoundary),
                                                          CCPointExtension.clampf(tempPos.y, m_fBottomBoundary, m_fTopBoundary));
            }
            else
            {
                m_pTarget.position = CCPointExtension.ccpSub(m_obHalfScreenSize, m_pobFollowedNode.position);
            }
        }

        public override bool isDone()
        {
            return !m_pobFollowedNode.isRunning;
        }

        public override void stop()
        {
            m_pTarget = null;
            base.stop();
        }

        /// <summary>
        /// creates the action with no boundary set
        /// </summary>
        public static CCFollow actionWithTarget(CCNode followedNode)
        {
            CCFollow ret = new CCFollow();

            if (ret != null && ret.initWithTarget(followedNode))
            {
                return ret;
            }

            return null;
        }

        /// <summary>
        /// creates the action with a set boundary
        /// </summary>
        public static CCFollow actionWithTarget(CCNode followedNode, CCRect rect)
        {
            CCFollow ret = new CCFollow();

            if (ret != null && ret.initWithTarget(followedNode, rect))
            {
                return ret;
            }

            return null;
        }

        // node to follow
        protected CCNode m_pobFollowedNode;

        protected bool m_bBoundarySet;
        /// <summary>
        /// whether camera should be limited to certain area
        /// </summary>
        public bool boundarySet
        {
            get
            {
                return m_bBoundarySet;
            }
            set
            {
                m_bBoundarySet = value;
            }
        }

        /// <summary>
        /// if screen size is bigger than the boundary - update not needed   
        /// </summary>
        protected bool m_bBoundaryFullyCovered;

        // fast access to the screen dimensions
        protected CCPoint m_obHalfScreenSize;
        protected CCPoint m_obFullScreenSize;

        // world boundaries
        protected float m_fLeftBoundary;
        protected float m_fRightBoundary;
        protected float m_fTopBoundary;
        protected float m_fBottomBoundary;
    }
}
