/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011      Fulcrum Mobile Network, Inc.

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
    public enum tCCMenuState
    {
        kCCMenuStateWaiting,
        kCCMenuStateTrackingTouch
    };

    public class CCMenu : CCLayer, CCRGBAProtocol, ICCTouchDelegate
    {
        const float kDefaultPadding = 5;
        const int kCCMenuTouchPriority = -128;

        protected tCCMenuState m_eState;
        protected CCMenuItem m_pSelectedItem;

        public CCMenu()
        {
            m_cOpacity = 0;
            m_pSelectedItem = null;
        }

        /// <summary>
        /// creates an empty CCMenu
        /// </summary>
        public static CCMenu node()
        {
            CCMenu menu = new CCMenu();

            if (menu != null && menu.init())
            {
                return menu;
            }

            return null;
        }

        /// <summary>
        /// creates a CCMenu with it's items
        /// </summary>
        public static CCMenu menuWithItems(params CCMenuItem[] item)
        {
            CCMenu pRet = new CCMenu();

            if (pRet != null && pRet.initWithItems(item))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// creates a CCMenu with it's item, then use addChild() to add 
        /// other items. It is used for script, it can't init with undetermined
        /// number of variables.
        /// </summary>
        public static CCMenu menuWithItem(CCMenuItem item)
        {
            return menuWithItems(item, null);
        }

        /// <summary>
        /// initializes an empty CCMenu
        /// </summary>
        public bool init()
        {
            return initWithItems(null);
        }

        /// <summary>
        /// initializes a CCMenu with it's items 
        /// </summary>
        bool initWithItems(params CCMenuItem[] item)
        {
            if (base.init())
            {
                this.m_bIsTouchEnabled = true;

                // menu in the center of the screen
                CCSize s = CCDirector.sharedDirector().getWinSize();

                this.m_bIsRelativeAnchorPoint = true;
                anchorPoint = new CCPoint(0.0f, 0.0f);
                this.contentSize = s;

                // XXX: in v0.7, winSize should return the visible size
                // XXX: so the bar calculation should be done there
                CCRect r;
                CCApplication.sharedApplication().statusBarFrame(out r);

                ccDeviceOrientation orientation = CCDirector.sharedDirector().deviceOrientation;
                if (orientation == ccDeviceOrientation.CCDeviceOrientationLandscapeLeft
                    ||
                    orientation == ccDeviceOrientation.CCDeviceOrientationLandscapeRight)
                {
                    s.height -= r.size.width;
                }
                else
                {
                    s.height -= r.size.height;
                }

                position = new CCPoint(s.width / 2, s.height / 2);

                if (item != null)
                {
                    foreach (var menuItem in item)
                    {
                        this.addChild(menuItem);
                    }
                }
                //	[self alignItemsVertically];

                m_pSelectedItem = null;
                m_eState = tCCMenuState.kCCMenuStateWaiting;
                return true;
            }

            return false;
        }

        /// <summary>
        /// align items vertically
        /// </summary>
        public void alignItemsVertically()
        {
            this.alignItemsVerticallyWithPadding(kDefaultPadding);
        }

        /// <summary>
        /// align items vertically with padding
        /// @since v0.7.2
        /// </summary>
        public void alignItemsVerticallyWithPadding(float padding)
        {
            float height = -padding;

            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                foreach (var pChild in m_pChildren)
                {
                    if (pChild != null)
                    {
                        height += pChild.contentSize.height * pChild.scaleY + padding;
                    }
                }
            }

            float y = height / 2.0f;
            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                foreach (var pChild in m_pChildren)
                {
                    if (pChild != null)
                    {
                        pChild.position = new CCPoint(0, y - pChild.contentSize.height * pChild.scaleY / 2.0f);
                        y -= pChild.contentSize.height * pChild.scaleY + padding;
                    }
                }
            }
        }

        /// <summary>
        /// align items horizontally
        /// </summary>
        public void alignItemsHorizontally()
        {
            this.alignItemsHorizontallyWithPadding(kDefaultPadding);
        }

        /// <summary>
        /// align items horizontally with padding
        /// @since v0.7.2
        /// </summary>
        public void alignItemsHorizontallyWithPadding(float padding)
        {
            float width = -padding;

            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                foreach (var pChild in m_pChildren)
                {
                    if (pChild != null)
                    {
                        width += pChild.contentSize.width * pChild.scaleX + padding;
                    }
                }
            }

            float x = -width / 2.0f;
            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                foreach (var pChild in m_pChildren)
                {
                    if (pChild != null)
                    {
                        pChild.position = new CCPoint(x + pChild.contentSize.width * pChild.scaleX / 2.0f, 0);
                        x += pChild.contentSize.width * pChild.scaleX + padding;
                    }
                }
            }
        }

        /** align items in rows of columns */
        //void alignItemsInColumns(unsigned int columns, ...);
        //void alignItemsInColumns(unsigned int columns, va_list args);

        /** align items in columns of rows */
        //void alignItemsInRows(unsigned int rows, ...);
        //void alignItemsInRows(unsigned int rows, va_list args);

        public override void registerWithTouchDispatcher()
        {
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, kCCMenuTouchPriority, true);
        }

        /// <summary>
        /// For phone event handle functions
        /// </summary>
        public override bool ccTouchBegan(CCTouch touch, CCEvent ccevent)
        {
            if (m_eState != tCCMenuState.kCCMenuStateWaiting || !m_bIsVisible)
            {
                return false;
            }

            for (CCNode c = this.m_pParent; c != null; c = c.parent)
            {
                if (c.visible == false)
                {
                    return false;
                }
            }

            m_pSelectedItem = this.itemForTouch(touch);

            if (m_pSelectedItem != null)
            {
                m_eState = tCCMenuState.kCCMenuStateTrackingTouch;
                m_pSelectedItem.selected();

                return true;
            }

            return false;
        }

        public override void ccTouchEnded(CCTouch touch, CCEvent ccevent)
        {
            Debug.Assert(m_eState == tCCMenuState.kCCMenuStateTrackingTouch, "[Menu ccTouchEnded] -- invalid state");

            if (m_pSelectedItem != null)
            {
                m_pSelectedItem.unselected();
                m_pSelectedItem.activate();
            }

            m_eState = tCCMenuState.kCCMenuStateWaiting;
        }

        public override void ccTouchCancelled(CCTouch touch, CCEvent ccevent)
        {
            Debug.Assert(m_eState == tCCMenuState.kCCMenuStateTrackingTouch, "[Menu ccTouchCancelled] -- invalid state");

            if (m_pSelectedItem != null)
            {
                m_pSelectedItem.unselected();
            }

            m_eState = tCCMenuState.kCCMenuStateWaiting;
        }

        public override void ccTouchMoved(CCTouch touch, CCEvent ccevent)
        {
            Debug.Assert(m_eState == tCCMenuState.kCCMenuStateTrackingTouch, "[Menu ccTouchMoved] -- invalid state");

            CCMenuItem currentItem = this.itemForTouch(touch);

            if (currentItem != m_pSelectedItem)
            {
                if (m_pSelectedItem != null)
                {
                    m_pSelectedItem.unselected();
                }

                m_pSelectedItem = currentItem;

                if (m_pSelectedItem != null)
                {
                    m_pSelectedItem.selected();
                }
            }
        }

        public virtual void destroy()
        {
            //release();            
        }

        public virtual void keep()
        {
            //throw new NotImplementedException();
        }

        public override void onExit()
        {
            if (m_eState == tCCMenuState.kCCMenuStateTrackingTouch)
            {
                m_pSelectedItem.unselected();
                m_eState = tCCMenuState.kCCMenuStateWaiting;
                m_pSelectedItem = null;
            }

            base.onExit();
        }

        public virtual CCRGBAProtocol convertToRGBAProtocol()
        {
            return (CCRGBAProtocol)this;
        }

        protected CCMenuItem itemForTouch(CCTouch touch)
        {
            //XNA point
            CCPoint touchLocation = touch.locationInView(touch.view());
            //cocos2d point
            touchLocation = CCDirector.sharedDirector().convertToGL(touchLocation);

            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                foreach (var pChild in m_pChildren)
                {
                    if (pChild != null && pChild.visible && ((CCMenuItem)pChild).Enabled)
                    {
                        CCPoint local = pChild.convertToNodeSpace(touchLocation);
                        CCRect r = ((CCMenuItem)pChild).rect();
                        r.origin = CCPoint.Zero;

                        if (CCRect.CCRectContainsPoint(r, local))
                        {
                            return (CCMenuItem)pChild;
                        }
                    }
                }
            }

            return null;
        }

        #region CCRGBAProtocol Interface

        protected ccColor3B m_tColor;
        protected byte m_cOpacity;

        public ccColor3B Color
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

        public byte Opacity
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

        public bool IsOpacityModifyRGB
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

        #endregion
    }
}
