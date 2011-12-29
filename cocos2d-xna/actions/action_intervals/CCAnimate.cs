/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

http://www.cocos2d-x.org

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
using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
{
    /** @brief Animates a sprite given the name of an Animation */
    public class CCAnimate : CCActionInterval
    {
        CCAnimation m_pAnimation;
        CCSpriteFrame m_pOrigFrame;
        bool m_bRestoreOriginalFrame;

        public static CCAnimate actionWithAnimation(CCAnimation pAnimation)
        {
            CCAnimate pAnimate = new CCAnimate();
            pAnimate.initWithAnimation(pAnimation, true);

            return pAnimate;
        }

        public bool initWithAnimation(CCAnimation pAnimation)
        {
            Debug.Assert(pAnimation != null);

            return initWithAnimation(pAnimation, true);
        }

        public static CCAnimate actionWithAnimation(CCAnimation pAnimation, bool bRestoreOriginalFrame)
        {
            CCAnimate pAnimate = new CCAnimate();
            pAnimate.initWithAnimation(pAnimation, bRestoreOriginalFrame);

            return pAnimate;
        }

        public bool initWithAnimation(CCAnimation pAnimation, bool bRestoreOriginalFrame)
        {
            Debug.Assert(pAnimation != null);

            if (base.initWithDuration(pAnimation.getFrames().Count * pAnimation.getDelay()))
            {
                m_bRestoreOriginalFrame = bRestoreOriginalFrame;
                m_pAnimation = pAnimation;
                m_pOrigFrame = null;

                return true;
            }

            return false;
        }

        public static CCAnimate actionWithDuration(float duration, CCAnimation pAnimation, bool bRestoreOriginalFrame)
        {
            CCAnimate pAnimate = new CCAnimate();
            pAnimate.initWithDuration(duration, pAnimation, bRestoreOriginalFrame);

            return pAnimate;
        }

        public bool initWithDuration(float duration, CCAnimation pAnimation, bool bRestoreOriginalFrame)
        {
            Debug.Assert(pAnimation != null);

            if (base.initWithDuration(duration))
            {
                m_bRestoreOriginalFrame = bRestoreOriginalFrame;
                m_pAnimation = pAnimation;
                m_pOrigFrame = null;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCAnimate pCopy = null;
            if(pZone !=null && pZone.m_pCopyObject != null) 
            {
                //in case of being called at sub class
                pCopy = (CCAnimate)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCAnimate();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(m_fDuration, m_pAnimation, m_bRestoreOriginalFrame);

            return pCopy;
        }

        ~CCAnimate()
        {
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            CCSprite pSprite = (CCSprite)(pTarget);


            if (m_bRestoreOriginalFrame)
            {
                // original code : m_pOrigFrame = pSprite->displayedFrame();
                m_pOrigFrame = pSprite.displayedFrame();
            }
        }

        public override void stop()
        {
            if (m_bRestoreOriginalFrame && m_pTarget != null)
            {
                // original code: ((CCSprite*)(m_pTarget))->setDisplayFrame(m_pOrigFrame);
                ((CCSprite)(m_pTarget)).setDisplayFrame(m_pOrigFrame);
            }

            base.stop();
        }

        public override void update(float time)
        {
            List<CCSpriteFrame> pFrames = m_pAnimation.getFrames();
            int numberOfFrames = pFrames.Count;

            int idx = (int)(time * numberOfFrames);

            if (idx >= numberOfFrames)
            {
                idx = numberOfFrames - 1;
            }

            CCSprite pSprite = (CCSprite)(m_pTarget);
            if (! pSprite.isFrameDisplayed(pFrames[idx]))
            {
                pSprite.setDisplayFrame(pFrames[idx]);
            }
        }

        public override CCFiniteTimeAction reverse()
        {
            List<CCSpriteFrame> pOldArray = m_pAnimation.getFrames();
            List<CCSpriteFrame> pNewArray = new List<CCSpriteFrame>(pOldArray.Count);
   
            if (pOldArray.Count > 0)
            {
                CCSpriteFrame pElement;
                for (int nIndex = pOldArray.Count - 1; nIndex >= 0; nIndex--)
                {
                    pElement = pOldArray[nIndex];
                    if (null == pElement)
                    {
                        break;
                    }

                    pNewArray.Insert(pOldArray.Count - 1 - nIndex, (CCSpriteFrame)(pElement.copy()));
                }

            }

            CCAnimation pNewAnim = CCAnimation.animationWithFrames(pNewArray, m_pAnimation.getDelay());

            return CCAnimate.actionWithDuration(m_fDuration, pNewAnim, m_bRestoreOriginalFrame);
        }


    }
}
