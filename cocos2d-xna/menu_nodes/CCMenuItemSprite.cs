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
    /// CCMenuItemSprite accepts CCNode<CCRGBAProtocol> objects as items.
    /// The images has 3 different states:
    /// - unselected image
    /// - selected image
    /// - disabled image
    /// @since v0.8.0
    /// </summary>
    public class CCMenuItemSprite : CCMenuItem, ICCRGBAProtocol
    {
        #region contructor

        public CCMenuItemSprite()
        {
            m_pNormalImage = null;
            m_pSelectedImage = null;
            m_pDisabledImage = null;
        }

        /// <summary>
        /// creates a menu item with a normal and selected image
        /// </summary>
        /// <param name="normalSprite"></param>
        /// <param name="selectedSprite"></param>
        /// <returns></returns>
        public static CCMenuItemSprite itemFromNormalSprite(CCNode normalSprite, CCNode selectedSprite)
        {
            return itemFromNormalSprite(normalSprite, selectedSprite, null, null, null);
        }

        /// <summary>
        /// creates a menu item with a normal and selected image with target/selector
        /// </summary>
        /// <param name="normalSprite"></param>
        /// <param name="selectedSprite"></param>
        /// <param name="target"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static CCMenuItemSprite itemFromNormalSprite(CCNode normalSprite, CCNode selectedSprite,
                                                        SelectorProtocol target, SEL_MenuHandler selector)
        {
            return itemFromNormalSprite(normalSprite, selectedSprite, null, target, selector);
        }

        /// <summary>
        /// creates a menu item with a normal,selected  and disabled image with target/selector
        /// </summary>
        /// <param name="normalSprite"></param>
        /// <param name="selectedSprite"></param>
        /// <param name="disabledSprite"></param>
        /// <param name="target"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static CCMenuItemSprite itemFromNormalSprite(CCNode normalSprite, CCNode selectedSprite, CCNode disabledSprite,
                                                        SelectorProtocol target, SEL_MenuHandler selector)
        {
            CCMenuItemSprite pRet = new CCMenuItemSprite();
            pRet.initFromNormalSprite(normalSprite, selectedSprite, disabledSprite, target, selector);
            //pRet->autorelease();
            return pRet;
        }

        /// <summary>
        /// initializes a menu item with a normal, selected  and disabled image with target/selector
        /// </summary>
        public bool initFromNormalSprite(CCNode normalSprite, CCNode selectedSprite, CCNode disabledSprite,
                                    SelectorProtocol target, SEL_MenuHandler selector)
        {
            if (normalSprite == null)
            {
                throw new NullReferenceException();
            }

            initWithTarget(target, selector);

            NormalImage = normalSprite;
            SelectedImage = selectedSprite;
            DisabledImage = disabledSprite;

            contentSize = m_pNormalImage.contentSize;

            return true;
        }

        #endregion

        #region Images

        CCNode m_pNormalImage;
        CCNode NormalImage
        {
            get
            {
                return m_pNormalImage;
            }
            set
            {
                if (value != null)
                {
                    addChild(value);
                    value.anchorPoint = new CCPoint(0, 0);
                    value.visible = true;
                }

                if (m_pNormalImage != null)
                {
                    removeChild(m_pNormalImage, true);
                }

                m_pNormalImage = value;
            }
        }

        CCNode m_pSelectedImage;
        CCNode SelectedImage
        {
            get
            {
                return m_pSelectedImage;
            }
            set
            {
                if (value != null)
                {
                    addChild(value);
                    value.anchorPoint = new CCPoint(0, 0);
                    value.visible = true;
                }

                if (m_pSelectedImage != null)
                {
                    removeChild(m_pSelectedImage, true);
                }

                m_pSelectedImage = value;
            }
        }

        CCNode m_pDisabledImage;
        CCNode DisabledImage
        {
            get
            {
                return m_pDisabledImage;
            }
            set
            {
                if (value != null)
                {
                    addChild(value);
                    value.anchorPoint = new CCPoint(0, 0);
                    value.visible = true;
                }

                if (m_pDisabledImage != null)
                {
                    removeChild(m_pDisabledImage, true);
                }

                m_pDisabledImage = value;
            }
        }

        #endregion

        //// super methods
        public ccColor3B Color { get; set; }

        public byte Opacity
        {
            get
            {
                return (m_pNormalImage as ICCRGBAProtocol).Opacity;
            }
            set
            {
                (m_pNormalImage as ICCRGBAProtocol).Opacity = value;

                if (m_pSelectedImage != null)
                {
                    (m_pSelectedImage as ICCRGBAProtocol).Opacity = value;
                }

                if (m_pDisabledImage != null)
                {
                    (m_pDisabledImage as ICCRGBAProtocol).Opacity = value;
                }
            }
        } // typedef khronos_uint8_t  GLubyte;

        #region override

        ///**
        //@since v0.99.5
        //*/
        public override void selected()
        {
            base.selected();

            if (m_pDisabledImage != null)
            {
                m_pDisabledImage.visible = false;
            }

            if (m_pSelectedImage != null)
            {
                m_pNormalImage.visible = false;
                m_pSelectedImage.visible = true;
            }
            else
            {
                m_pNormalImage.visible = true;
            }
        }

        public override void unselected()
        {
            base.unselected();

            m_pNormalImage.visible = true;

            if (m_pSelectedImage != null)
            {
                m_pSelectedImage.visible = false;
            }

            if (m_pDisabledImage != null)
            {
                m_pDisabledImage.visible = false;
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;

                if (m_pSelectedImage != null)
                {
                    m_pSelectedImage.visible = false;
                }

                if (value)
                {
                    m_pNormalImage.visible = true;

                    if (m_pDisabledImage != null)
                    {
                        m_pDisabledImage.visible = false;
                    }
                }
                else
                {
                    if (m_pDisabledImage != null)
                    {
                        m_pDisabledImage.visible = true;
                        m_pNormalImage.visible = false;
                    }
                    else
                    {
                        m_pNormalImage.visible = true;
                    }
                }
            }
        }

        #endregion

        public virtual ICCRGBAProtocol convertToRGBAProtocol()
        {
            return (this as ICCRGBAProtocol);
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
    }
}
