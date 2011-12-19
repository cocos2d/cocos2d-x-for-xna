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

namespace cocos2d
{
    /// <summary>
    /// An abstract class for "label" CCMenuItemLabel items 
    /// Any CCNode that supports the CCLabelProtocol protocol can be added.
    /// Supported nodes:
    /// - CCBitmapFontAtlas
    /// - CCLabelAtlas
    /// - CCLabelTTF
    /// </summary>
    public class CCMenuItemLabel : CCMenuItem, ICCRGBAProtocol
    {
        const uint kCurrentItem = 0xc0c05001;
        const uint kZoomActionTag = 0xc0c05002;

        protected ccColor3B m_tColorBackup;
        protected float m_fOriginalScale;

        public ccColor3B DisabledColor
        {
            get;
            set;
        }

        protected CCNode m_pLabel;

        public CCNode Label
        {
            get
            {
                return m_pLabel;
            }
            set
            {
                if (value != null)
                {
                    addChild(value);
                    value.anchorPoint = new CCPoint(0, 0);
                    contentSize = value.contentSize;
                }

                if (m_pLabel != null)
                {
                    removeChild(m_pLabel, true);
                }

                m_pLabel = value;
            }
        }

        //public override CCPoint anchorPoint
        //{
        //    get
        //    {
        //        return m_pLabel.anchorPoint;
        //    }
        //    set
        //    {
        //        m_pLabel.anchorPoint = value;
        //    }
        //}

        /// <summary>
        /// creates a CCMenuItemLabel with a Label, target and selector
        /// </summary>
        public static CCMenuItemLabel itemWithLabel(CCNode label, SelectorProtocol target, SEL_MenuHandler selector)
        {
            CCMenuItemLabel pRet = new CCMenuItemLabel();
            pRet.initWithLabel(label, target, selector);

            return pRet;
        }

        /// <summary>
        /// creates a CCMenuItemLabel with a Label. Target and selector will be nill 
        /// </summary>
        public static CCMenuItemLabel itemWithLabel(CCNode label)
        {
            CCMenuItemLabel pRet = new CCMenuItemLabel();
            pRet.initWithLabel(label, null, null);

            return pRet;
        }

        /// <summary>
        /// initializes a CCMenuItemLabel with a Label, target and selector
        /// </summary>
        protected bool initWithLabel(CCNode label, SelectorProtocol target, SEL_MenuHandler selector)
        {
            base.initWithTarget(target, selector);

            m_fOriginalScale = 1.0f;
            m_tColorBackup = new ccColor3B(Microsoft.Xna.Framework.Color.White); // ccWHITE;
            DisabledColor = new ccColor3B(126, 126, 126);
            this.Label = label;

            return true;
        }

        /** sets a new string to the inner label */
        public void setString(string label)
        {
            ((ICCLabelProtocol)m_pLabel).setString(label);
            this.contentSize = m_pLabel.contentSize;
        }

        // super methods
        public override void activate()
        {
            if (this.Enabled)
            {
                this.stopAllActions();
                this.scale = m_fOriginalScale;
                base.activate();
            }
        }

        public override void selected()
        {
            // subclass to change the default action
            if (this.Enabled)
            {
                base.selected();

                //CCAction action = getActionByTag(kZoomActionTag);
                //if (action != null)
                //{
                //    this.stopAction(action);
                //}
                //else
                //{
                //    m_fOriginalScale = this.scale;
                //}

                //CCAction zoomAction = CCScaleTo.actionWithDuration(0.1f, m_fOriginalScale * 1.2f);
                ////zoomAction.tag = (int)kZoomActionTag;
                //this.runAction(zoomAction);
            }
        }

        public override void unselected()
        {
            // subclass to change the default action
            if (m_bIsEnabled)
            {
                base.unselected();
                //this.stopActionByTag((int)kZoomActionTag);
                CCAction zoomAction = CCScaleTo.actionWithDuration(0.1f, m_fOriginalScale);
                //zoomAction.tag=kZoomActionTag;
                this.runAction(zoomAction);
            }
        }

        /** Enable or disabled the CCMenuItemFont
        @warning setIsEnabled changes the RGB color of the font
        */
        public override bool Enabled
        {
            set
            {
                if (m_bIsEnabled != value)
                {
                    if (!value)
                    {
                        //m_tColorBackup = m_pLabel.convertToRGBAProtocol().getColor();
                        //m_pLabel->convertToRGBAProtocol()->setColor(m_tDisabledColor);
                    }
                    else
                    {
                        //m_pLabel->convertToRGBAProtocol()->setColor(m_tColorBackup);
                    }
                }
                base.Enabled = value;
            }

            get
            {
                return base.Enabled;
            }
        }


        public virtual ICCRGBAProtocol convertToRGBAProtocol()
        {
            return (ICCRGBAProtocol)this;
        }


        #region CCRGBAProtocol interface

        public ccColor3B Color
        {
            get
            {
                return (m_pLabel as ICCRGBAProtocol).Color;
                
            }
            set
            {
                (m_pLabel as ICCRGBAProtocol).Color = value;
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
