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
    /// CCMenuItemImage accepts images as items.
    /// The images has 3 different states:
    /// - unselected image
    /// - selected image
    /// - disabled image
    /// For best results try that all images are of the same size
    /// </summary>
    public class CCMenuItemImage : CCMenuItemSprite
    {
        /// <summary>
        /// creates a menu item with a normal and selected image
        /// </summary>
        public static CCMenuItemImage itemFromNormalImage(string normalImage, string selectedImage)
        {
            return itemFromNormalImage(normalImage, selectedImage, null, null, null);
        }

        /// <summary>
        /// creates a menu item with a normal,selected  and disabled image
        /// </summary>
        public static CCMenuItemImage itemFromNormalImage(string normalImage, string selectedImage, SelectorProtocol target, SEL_MenuHandler selector)
        {
            return itemFromNormalImage(normalImage, selectedImage, null, target, selector);
        }

        /// <summary>
        /// creates a menu item with a normal and selected image with target/selector
        /// </summary>
        public static CCMenuItemImage itemFromNormalImage(string normalImage, string selectedImage, string disabledImage, SelectorProtocol target, SEL_MenuHandler selector)
        {
            CCMenuItemImage pRet = new CCMenuItemImage();

            if (pRet != null && pRet.initFromNormalImage(normalImage, selectedImage, disabledImage, target, selector))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// creates a menu item with a normal,selected  and disabled image with target/selector
        /// </summary>
        public static CCMenuItemImage itemFromNormalImage(string normalImage, string selectedImage, string disabledImage)
        {
            CCMenuItemImage pRet = new CCMenuItemImage();

            if (pRet != null && pRet.initFromNormalImage(normalImage, selectedImage, disabledImage, null, null))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// initializes a menu item with a normal, selected  and disabled image with target/selector
        /// </summary>
        bool initFromNormalImage(string normalImage, string selectedImage, string disabledImage, SelectorProtocol target, SEL_MenuHandler selector)
        {
            CCNode normalSprite = CCSprite.spriteWithFile(normalImage);
            CCNode selectedSprite = null;
            CCNode disabledSprite = null;

            if (selectedImage != null && selectedImage.Trim() != "")
            {
                selectedSprite = CCSprite.spriteWithFile(selectedImage);
            }

            if (disabledImage != null && disabledImage.Trim() != "")
            {
                disabledSprite = CCSprite.spriteWithFile(disabledImage);
            }

            return initFromNormalSprite(normalSprite, selectedSprite, disabledSprite, target, selector);
        }
    }
}
