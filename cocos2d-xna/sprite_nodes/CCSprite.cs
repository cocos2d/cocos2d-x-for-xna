/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.

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

using System;
namespace cocos2d
{
    /** CCSprite is a 2d image ( http://en.wikipedia.org/wiki/Sprite_(computer_graphics) )
    *
    * CCSprite can be created with an image, or with a sub-rectangle of an image.
    *
    * If the parent or any of its ancestors is a CCSpriteBatchNode then the following features/limitations are valid
    *	- Features when the parent is a CCBatchNode:
    *		- MUCH faster rendering, specially if the CCSpriteBatchNode has many children. All the children will be drawn in a single batch.
    *
    *	- Limitations
    *		- Camera is not supported yet (eg: CCOrbitCamera action doesn't work)
    *		- GridBase actions are not supported (eg: CCLens, CCRipple, CCTwirl)
    *		- The Alias/Antialias property belongs to CCSpriteBatchNode, so you can't individually set the aliased property.
    *		- The Blending function property belongs to CCSpriteBatchNode, so you can't individually set the blending function property.
    *		- Parallax scroller is not supported, but can be simulated with a "proxy" sprite.
    *
    *  If the parent is an standard CCNode, then CCSprite behaves like any other CCNode:
    *    - It supports blending functions
    *    - It supports aliasing / antialiasing
    *    - But the rendering will be slower: 1 draw per children.
    *
    * The default anchorPoint in CCSprite is (0.5, 0.5).
    */
    public class CCSprite : CCNode, CCTextureProtocol, CCRGBAProtocol
    {
        /**
         Whether or not an CCSprite will rotate, scale or translate with it's parent.
         Useful in health bars, when you want that the health bar translates with it's parent but you don't
         want it to rotate with its parent.
         @since v0.99.0
         */
        public enum ccHonorParentTransform
        {
            //! Translate with it's parent
            CC_HONOR_PARENT_TRANSFORM_TRANSLATE = 1 << 0,
            //! Rotate with it's parent
            CC_HONOR_PARENT_TRANSFORM_ROTATE = 1 << 1,
            //! Scale with it's parent
            CC_HONOR_PARENT_TRANSFORM_SCALE = 1 << 2,
            //! Skew with it's parent
            CC_HONOR_PARENT_TRANSFORM_SKEW = 1 << 3,

            //! All possible transformation enabled. Default value.
            CC_HONOR_PARENT_TRANSFORM_ALL = CC_HONOR_PARENT_TRANSFORM_TRANSLATE | CC_HONOR_PARENT_TRANSFORM_ROTATE | CC_HONOR_PARENT_TRANSFORM_SCALE | CC_HONOR_PARENT_TRANSFORM_SKEW,
        }

        // Properties

        /** Opacity: conforms to CCRGBAProtocol protocol */
        private byte m_nOpacity;
        public byte getOpacity()
        {
            throw new NotImplementedException();
        }
        public void setOpacity(byte value)
        {
            throw new NotImplementedException();
        }

        /** Color: conforms with CCRGBAProtocol protocol */
        private ccColor3B m_sColor;
        public ccColor3B getColor()
        {
            throw new NotImplementedException();
        }
        public void setColor(ccColor3B value)
        {
            throw new NotImplementedException();
        }

        /** whether or not the Sprite needs to be updated in the Atlas */
        private bool m_bDirty;
        public bool dirty
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

        /** get the quad (tex coords, vertex coords and color) information */
        private ccV3F_C4B_T2F_Quad m_sQuad;
        public ccV3F_C4B_T2F_Quad quad
        {
            // read only
            get
            {
                return m_sQuad;
            }
        }

        /** returns whether or not the texture rectangle is rotated */
        private bool m_bRectRotated;
        public bool rectRotated
        {
            // read only
            get
            {
                return m_bRectRotated;
            }
        }

        /** The index used on the TextureAtlas. Don't modify this value unless you know what you are doing */
        private uint m_uAtlasIndex;
        public uint atlasIndex
        {
            get
            {
                return m_uAtlasIndex;
            }
            set
            {
                m_uAtlasIndex = value;
            }
        }

        /** returns the rect of the CCSprite in points */
        private CCRect m_obTextureRect;
        public CCRect textureRect
        {
            // read only
            get
            {
                return m_obTextureRect;
            }
        }

        /** whether or not the Sprite is rendered using a CCSpriteBatchNode */
        private bool m_bUseBatchNode;
        public bool useBatchNode
        {
            get
            {
                return m_bUseBatchNode;
            }
            set
            {
                m_bUseBatchNode = value;
            }
        }

        /** weak reference of the CCTextureAtlas used when the sprite is rendered using a CCSpriteBatchNode */
        ///@todo add m_pobTextureAtlas
        ///

        /** weak reference to the CCSpriteBatchNode that renders the CCSprite */
        ///@todo add m_pobBatchNode
        ///

        /** whether or not to transform according to its parent transfomrations.
         Useful for health bars. eg: Don't rotate the health bar, even if the parent rotates.
         IMPORTANT: Only valid if it is rendered using an CCSpriteBatchNode.
         @since v0.99.0
         */
        private ccHonorParentTransform m_eHonorParentTransform;
        public ccHonorParentTransform honorParentTransform
        {
            get
            {
                return m_eHonorParentTransform;
            }
            set
            {
                m_eHonorParentTransform = value;
            }
        }

        /** offset position in pixels of the sprite in points. Calculated automatically by editors like Zwoptex.
             @since v0.99.0
             */
        private CCPoint m_obOffsetPositionInPixels;
        public CCPoint offsetPositionInPixels
        {
            // read only
            get
            {
                return m_obOffsetPositionInPixels;
            }
        }

        /** conforms to CCTextureProtocol protocol */
        private ccBlendFunc m_sBlendFunc;
        public ccBlendFunc getBlendFunc()
        {
            return m_sBlendFunc;
        }
        public void setBlendFunc(ccBlendFunc value)
        {
            m_sBlendFunc = value;
        }

        /** Creates an sprite with a texture.
	     The rect used will be the size of the texture.
	     The offset will be (0,0).
	     */
        public static CCSprite spriteWithTexture(CCTexture2D texture)
        {
            throw new NotImplementedException();
        }

        /** Creates an sprite with a texture and a rect.
	     The offset will be (0,0).
	     */
        public static CCSprite spriteWithTexture(CCTexture2D texture, CCRect rect)
        {
            throw new NotImplementedException();
        }

        /** Creates an sprite with a texture, a rect and offset. */
        public static CCSprite spriteWithTexture(CCTexture2D texture, CCRect rect, CCPoint offset)
        {
            throw new NotImplementedException();
        }

        ///@todo
        /** Creates an sprite with an sprite frame. */
        //static CCSprite* spriteWithSpriteFrame(CCSpriteFrame *pSpriteFrame);

        ///@todo
        /** Creates an sprite with an sprite frame name.
         An CCSpriteFrame will be fetched from the CCSpriteFrameCache by name.
         If the CCSpriteFrame doesn't exist it will raise an exception.
         @since v0.9
         */
        //static CCSprite* spriteWithSpriteFrameName(const char *pszSpriteFrameName);

        /** Creates an sprite with an image filename.
	     The rect used will be the size of the image.
	     The offset will be (0,0).
	     */
        public static CCSprite spriteWithFile(string fileName)
        {
            throw new NotImplementedException();
        }

        /** Creates an sprite with an image filename and a rect.
	     The offset will be (0,0).
	     */
        public static CCSprite spriteWithFile(string fileName, CCRect rect)
        {
            throw new NotImplementedException();
        }

        /** Creates an sprite with an CCBatchNode and a rect */
        ///@todo
        // static CCSprite* spriteWithBatchNode(CCSpriteBatchNode *batchNode, const CCRect& rect);

        public virtual bool init()
        {
            throw new NotImplementedException();
        }

        public CCSprite()
        {
            throw new NotImplementedException();
        }
        ~CCSprite()
        {
            throw new NotImplementedException();
        }

        public override void removeChild(CCNode child, bool cleanup)
        {
            throw new NotImplementedException();
        }

        public override void removeAllChildrenWithCleanup(bool cleanup)
        {
            throw new NotImplementedException();
        }

        public override void reorderChild(CCNode child, int zOrder)
        {
            throw new NotImplementedException();
        }

        public override void addChild(CCNode child)
        {
            throw new NotImplementedException();
        }

        public override void addChild(CCNode child, int zOrder)
        {
            throw new NotImplementedException();
        }

        public override void addChild(CCNode child, int zOrder, int tag)
        {
            throw new NotImplementedException();
        }

        public virtual void setDirtyRecursively(bool bValue)
        {
            throw new NotImplementedException();
        }

        public virtual void setPosition(CCPoint pos)
        {
            throw new NotImplementedException();
        }

        public virtual void setPositionInPixels(CCPoint pos)
        {
            throw new NotImplementedException();
        }

        public virtual void setRotation(float fRotation)
        {
            throw new NotImplementedException();
        }

        public virtual void setSkewX(float sx)
        {
            throw new NotImplementedException();
        }

        public virtual void setSkewY(float sy)
        {
            throw new NotImplementedException();
        }

        public virtual void setScaleX(float fScaleX)
        {
            throw new NotImplementedException();
        }

        public virtual void setScaleY(float fScaleY)
        {
            throw new NotImplementedException();
        }

        public virtual void setScale(float fScale)
        {
            throw new NotImplementedException();
        }

        public virtual void setVetexZ(float fVetexZ)
        {
            throw new NotImplementedException();
        }

        public virtual void setAnchorPoint(CCPoint anchor)
        {
            throw new NotImplementedException();
        }

        public virtual void setIsRelativeAnchorPoint(bool bRelative)
        {
            throw new NotImplementedException();
        }

        public virtual void setIsVisible(bool bVisible)
        {
            throw new NotImplementedException();
        }

        public void setFilpX(bool bFlipX)
        {
            throw new NotImplementedException();
        }

        public void setFlipY(bool bFlipY)
        {
            throw new NotImplementedException();

        }
        /** whether or not the sprite is flipped horizontally. 
	    It only flips the texture of the sprite, and not the texture of the sprite's children.
	    Also, flipping the texture doesn't alter the anchorPoint.
	    If you want to flip the anchorPoint too, and/or to flip the children too use:

	    sprite->setScaleX(sprite->getScaleX() * -1);
	    */

        public bool isFlipX()
        {
            throw new NotImplementedException();
        }

        /** whether or not the sprite is flipped vertically.
	    It only flips the texture of the sprite, and not the texture of the sprite's children.
	    Also, flipping the texture doesn't alter the anchorPoint.
	    If you want to flip the anchorPoint too, and/or to flip the children too use:

	    sprite->setScaleY(sprite->getScaleY() * -1);
	    */
        public bool isFlipY()
        {
            throw new NotImplementedException();
        }

        void updateColor()
        {
            throw new NotImplementedException();
        }

        /** opacity: conforms to CCRGBAProtocol protocol */
	    public virtual void setIsOpacityModifyRGB(bool bValue)
        {
            throw new NotImplementedException();
        }

	    public virtual bool getIsOpacityModifyRGB()
        {
            throw new NotImplementedException();
        }

        // CCTextureProtocol
        public virtual void setTexture(CCTexture2D texture)
        {
            throw new NotImplementedException();
        }
        public virtual CCTexture2D getTexture()
        {
            throw new NotImplementedException();
        }

        /** Initializes an sprite with a texture.
	     The rect used will be the size of the texture.
	     The offset will be (0,0).
	     */
        public bool initWithTexture(CCTexture2D texture)
        {
            throw new NotImplementedException();
        }

        /** Initializes an sprite with a texture and a rect.
	     The offset will be (0,0).
	     */
        public bool initWithTexture(CCTexture2D texture, CCRect rect)
        {
            throw new NotImplementedException();
        }

        ///@todo
        // Initializes an sprite with an sprite frame.
        //bool initWithSpriteFrame(CCSpriteFrame* pSpriteFrame);

        /** Initializes an sprite with an sprite frame name.
	     An CCSpriteFrame will be fetched from the CCSpriteFrameCache by name.
	     If the CCSpriteFrame doesn't exist it will raise an exception.
	     @since v0.9
	     */
        public bool initWithSpriteFrameName(string spriteFrameName)
        {
            throw new NotImplementedException();
        }

        /** Initializes an sprite with an image filename.
	     The rect used will be the size of the image.
	     The offset will be (0,0).
	     */
        public bool initWithFile(string fileName)
        {
            throw new NotImplementedException();
        }

        /** Initializes an sprite with an image filename, and a rect.
	     The offset will be (0,0).
	     */
        public bool initWithFile(string fileName, CCRect rect)
        {
            throw new NotImplementedException();
        }

        ///@todo
        /** Initializes an sprite with an CCSpriteBatchNode and a rect in points */
        //bool initWithBatchNode(CCSpriteBatchNode *batchNode, const CCRect& rect);

        ///@todo
        /** Initializes an sprite with an CCSpriteBatchNode and a rect in pixels
	    @since v0.99.5
	    */
	    //bool initWithBatchNodeRectInPixels(CCSpriteBatchNode *batchNode, const CCRect& rect);

        // BatchNode methods

        /** updates the quad according the the rotation, position, scale values. */
        public void updateTransform()
        {
            throw new NotImplementedException();
        }

        /** tell the sprite to use self-render.
	     @since v0.99.0
	     */
        public void useSelfRender()
        {
            throw new NotImplementedException();
        }

        /** updates the texture rect of the CCSprite in points. */
        public void setTextureRect(CCRect rect)
        {
            throw new NotImplementedException();
        }

        /** updates the texture rect, rectRotated and untrimmed size of the CCSprite in pixels */
        public void setTextureRectInPixels(CCRect rect, bool rotated, CCSize size)
        {
            throw new NotImplementedException();
        }

        ///@todo
        /** tell the sprite to use batch node render.
	     @since v0.99.0
	     */
        //void useBatchNode(CCSpriteBatchNode* batchNode);

        // Frames

        ///@todo
        /** sets a new display frame to the CCSprite. */
        //void setDisplayFrame(CCSpriteFrame* pNewFrame);

        ///@todo
        /** returns whether or not a CCSpriteFrame is being displayed */
        //bool isFrameDisplayed(CCSpriteFrame* pFrame);

        ///@todo
        /** returns the current displayed frame. */
	    //CCSpriteFrame* displayedFrame(void);

        // Animation

        /** changes the display frame with animation name and index.
	    The animation name will be get from the CCAnimationCache
	    @since v0.99.5
	    */
        public void setDisplayFrameWithAnimationName(string animationName, int frameIndex)
        {
            throw new NotImplementedException();
        }

        protected void updateTextureCoords(CCRect rect)
        {
            throw new NotImplementedException();
        }

        protected void updateBlendFunc()
        {
            throw new NotImplementedException();
        }

        protected void getTransformValues(transformValues_ tv)
        {
            throw new NotImplementedException();
        }

        // Subchildren needs to be updated
        protected bool m_bRecursiveDirty;

        // optimization to check if it contain children
        protected bool m_bHasChildren;

        //
        // Data used when the sprite is self-rendered
        //
        protected CCTexture2D m_pobTexture;

        // texture
        protected CCRect m_obRect;
        protected CCRect m_obRectInPixels;

        // Offset Position (used by Zwoptex)
        protected CCPoint m_obUnflippedOffsetPositionFromCenter;

        // opacity and RGB protocol
        protected ccColor3B m_sColorUnmodified;
        protected bool m_bOpacityModifyRGB;
    }

    public struct transformValues_
    {
        CCPoint pos;    // position  x and y
        CCPoint scale;  // scale x and y
        float   rotation;
        CCPoint skew;   // skew x and y
        CCPoint ap;     // anchor point in pixels
        bool    visible;
    }
}
