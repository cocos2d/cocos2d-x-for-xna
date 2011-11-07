using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCMenuItemImage : CCMenuItemSprite
    {
        /// <summary>
        /// creates a menu item with a normal and selected image
        /// </summary>
        /// <param name="normalImage"></param>
        /// <param name="selectedImage"></param>
        /// <returns></returns>
        public static CCMenuItemImage itemFromNormalImage(string normalImage, string selectedImage)
        {
            return itemFromNormalImage(normalImage, selectedImage, null, null, null);
        }

        /// <summary>
        /// creates a menu item with a normal,selected  and disabled image
        /// </summary>
        /// <param name="normalImage"></param>
        /// <param name="selectedImage"></param>
        /// <param name="target"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static CCMenuItemImage itemFromNormalImage(string normalImage, string selectedImage,SelectorProtocol target, SEL_MenuHandler selector)
        {
            return itemFromNormalImage(normalImage, selectedImage, null, target, selector);
        }

        /// <summary>
        /// creates a menu item with a normal and selected image with target/selector
        /// </summary>
        /// <param name="normalImage"></param>
        /// <param name="selectedImage"></param>
        /// <param name="disabledImage"></param>
        /// <param name="target"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
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
        /// <param name="normalImage"></param>
        /// <param name="selectedImage"></param>
        /// <param name="disabledImage"></param>
        /// <returns></returns>
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
        /// <param name="normalImage"></param>
        /// <param name="selectedImage"></param>
        /// <param name="disabledImage"></param>
        /// <param name="target"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        bool initFromNormalImage(string normalImage, string selectedImage, string disabledImage, SelectorProtocol target, SEL_MenuHandler selector)
	    {
            CCNode normalSprite = CCSprite.spriteWithFile(normalImage);
            CCNode selectedSprite = null;
            CCNode disabledSprite = null;

            if (selectedImage != null && selectedImage.Trim() != "")
            {
                selectedSprite = CCSprite.spriteWithFile(selectedImage);
            }

            if(disabledImage != null && disabledImage.Trim() != "")
            {
                disabledSprite = CCSprite.spriteWithFile(disabledImage);
            }

            return initFromNormalSprite(normalSprite, selectedSprite, disabledSprite, target, selector);
	    }
    }
}
