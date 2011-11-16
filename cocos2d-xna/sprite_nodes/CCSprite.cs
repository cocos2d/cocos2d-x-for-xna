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
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace cocos2d
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
        #region Properties

        private byte m_nOpacity;

        /// <summary>
        /// Opacity: conforms to CCRGBAProtocol protocol
        /// </summary>
        public byte Opacity 
        {
            get
            { 
                return m_nOpacity;
            }

            set
            {
                m_nOpacity = value;

                // special opacity for premultiplied textures
                if (m_bOpacityModifyRGB)
                {
                    Color = m_sColorUnmodified;
                }

                updateColor();            
            }
        }

        private ccColor3B m_sColor;

        /// <summary>
        /// Color: conforms with CCRGBAProtocol protocol
        /// </summary>
        public ccColor3B Color 
        {
            get
            {
                if (m_bOpacityModifyRGB)
                {
                    return m_sColorUnmodified;
                }

                return m_sColor;
            }

            set
            {
                m_sColor = m_sColorUnmodified = value;

                if (m_bOpacityModifyRGB)
                {
                    m_sColor.r = (byte)(value.r * m_nOpacity / 255);
                    m_sColor.g = (byte)(value.g * m_nOpacity / 255);
                    m_sColor.b = (byte)(value.b * m_nOpacity / 255);
                }

                updateColor();
            }
        }

        /// <summary>
        /// opacity: conforms to CCRGBAProtocol protocol
        /// </summary>
        public virtual bool IsOpacityModifyRGB
        {
            get
            {
                return m_bOpacityModifyRGB;
            }

            set
            {
                ccColor3B oldColor = m_sColor;
                m_bOpacityModifyRGB = value;
                m_sColor = oldColor;
            }
        }

        /** whether or not the Sprite needs to be updated in the Atlas */
        private bool m_bDirty;
        public bool dirty
        {
            get
            {
                return m_bDirty;
            }
            set
            {
                m_bDirty = value;
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
        public bool IsUseBatchNode
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

        #endregion

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

        public override void draw()
        {
            if (null == m_pobTexture)
            {
                return;
            }

            m_pobTexture.drawAtPoint(new CCPoint(position.x - anchorPointInPixels.x, position.y - anchorPointInPixels.y));
        }

        /** Creates an sprite with a texture.
	     The rect used will be the size of the texture.
	     The offset will be (0,0).
	     */
        public static CCSprite spriteWithTexture(CCTexture2D texture)
        {
            CCSprite sprite = new CCSprite();
            if (sprite != null && sprite.initWithTexture(texture))
            {
                return sprite;
            }

            sprite = null;
            return null;
        }

        /** Creates an sprite with a texture and a rect.
	     The offset will be (0,0).
	     */
        public static CCSprite spriteWithTexture(CCTexture2D texture, CCRect rect)
        {
            CCSprite sprite = new CCSprite();
            if (sprite != null && sprite.initWithTexture(texture, rect))
            {
                return sprite;
            }

            sprite = null;
            return null;
        }

        /** Creates an sprite with a texture, a rect and offset. */
        public static CCSprite spriteWithTexture(CCTexture2D texture, CCRect rect, CCPoint offset)
        {
            // not implement
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
            CCSprite sprite = new CCSprite();
            if (sprite != null && sprite.initWithFile(fileName))
            {
                return sprite;
            }

            sprite = null;
            return sprite;
        }

        /** Creates an sprite with an image filename and a rect.
	     The offset will be (0,0).
	     */
        public static CCSprite spriteWithFile(string fileName, CCRect rect)
        {
            CCSprite sprite = new CCSprite();

            if (sprite != null && sprite.initWithFile(fileName, rect))
            {
                return sprite;
            }

            sprite = null;
            return sprite;
        }

        /** Creates an sprite with an CCBatchNode and a rect */
        ///@todo
        // static CCSprite* spriteWithBatchNode(CCSpriteBatchNode *batchNode, const CCRect& rect);

        public virtual bool init()
        {
            m_bDirty = m_bRecursiveDirty = false;

            // by default use "Self Render".
            // if the sprite is added to an batchnode, then it will automatically switch to "SpriteSheet Render"
            useSelfRender();

            m_bOpacityModifyRGB = true;
            m_nOpacity = 255;
            m_sColor = m_sColorUnmodified = new ccColor3B(255, 255, 255);

            /*
             * @todo, they are designed for opengl es, how to replace them?
             * 
            m_sBlendFunc.src = CC_BLEND_SRC;
            m_sBlendFunc.dst = CC_BLEND_DST;
             */

            // update texture (calls updateBlendFunc)
            setTexture(null);

            // clean the Quad
            //memset(&m_sQuad, 0, sizeof(m_sQuad));
            m_sQuad = new ccV3F_C4B_T2F_Quad();

            m_bFlipX = m_bFlipY = false;

            // default transform anchor: center
            anchorPoint = (CCPointExtension.ccp(0.5f, 0.5f));

            // zwoptex default values
            m_obOffsetPositionInPixels = new CCPoint();

            m_eHonorParentTransform = ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_ALL;
            m_bHasChildren = false;

            // Atlas: Color
            m_sQuad.bl.colors = new ccColor4B(255, 255, 255, 255);
            m_sQuad.br.colors = new ccColor4B(255, 255, 255, 255);
            m_sQuad.tl.colors = new ccColor4B(255, 255, 255, 255);
            m_sQuad.tr.colors = new ccColor4B(255, 255, 255, 255);

            // Atlas: Vertex

            // updated in "useSelfRender"

            // Atlas: TexCoords

            setTextureRectInPixels(new CCRect(), false, new CCSize());

            return true;
        }

        public CCSprite() 
        { 
            m_obOffsetPositionInPixels = new CCPoint();
            m_obRectInPixels = new CCRect();
            m_obUnflippedOffsetPositionFromCenter = new CCPoint();
        }
        ~CCSprite() { }

        public override void removeChild(CCNode child, bool cleanup)
        {
            if (m_bUseBatchNode)
            {
                ///@todo
                throw new NotImplementedException();
            }

            base.removeChild(child, cleanup);
        }

        public override void removeAllChildrenWithCleanup(bool cleanup)
        {
            if (m_bUseBatchNode)
            {
                foreach (CCNode node in m_pChildren)
                {
                    ///@todo
                    throw new NotImplementedException();
                }
            }

            base.removeAllChildrenWithCleanup(cleanup);

            m_bHasChildren = false;
        }

        public override void reorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null);
            Debug.Assert(m_pChildren.Contains(child));

            if (zOrder == child.zOrder)
            {
                return;
            }

            if (m_bUseBatchNode)
            {
                // XXX: Instead of removing/adding, it is more efficient to reorder manually
                removeChild(child, false);
                addChild(child, zOrder);
            }
            else
            {
                base.reorderChild(child, zOrder);
            }
        }

        public override void addChild(CCNode child)
        {
            base.addChild(child);
        }

        public override void addChild(CCNode child, int zOrder)
        {
            base.addChild(child, zOrder);
        }

        public override void addChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null);

            base.addChild(child, zOrder, tag);

            if (m_bUseBatchNode)
            {
                ///@todo
                //assert(((CCSprite*)pChild)->getTexture()->getName() == m_pobTextureAtlas->getTexture()->getName());
                //unsigned int index = m_pobBatchNode->atlasIndexForChild((CCSprite*)(pChild), zOrder);
                //m_pobBatchNode->insertChild((CCSprite*)(pChild), index);
                throw new NotImplementedException();
            }

            m_bHasChildren = true;
        }

        public virtual void setDirtyRecursively(bool bValue)
        {
            m_bDirty = m_bRecursiveDirty = bValue;
            // recursively set dirty
            if (m_bHasChildren)
            {
                foreach (CCNode child in m_pChildren)
                {
                    ((CCSprite)child).setDirtyRecursively(true);
                }
            }
        }

        private void SET_DIRTY_RECURSIVELY()
        {
            if (m_bUseBatchNode)
            {
                m_bDirty = m_bRecursiveDirty = true;
                if (m_bHasChildren)
                {
                    setDirtyRecursively(true);
                }
            }
        }

        public override CCPoint position
        {
            get
            {
                return base.position;
            }
            set
            {
                base.position = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override CCPoint positionInPixels
        {
            get
            {
                return base.positionInPixels;
            }
            set
            {
                base.positionInPixels = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float rotation
        {
            get
            {
                return base.rotation;
            }
            set
            {
                base.rotation = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float skewX
        {
            get
            {
                return base.skewX;
            }
            set
            {
                base.skewX = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float skewY
        {
            get
            {
                return base.skewY;
            }
            set
            {
                base.skewY = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float scaleX
        {
            get
            {
                return base.scaleX;
            }
            set
            {
                base.scaleX = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float scaleY
        {
            get
            {
                return base.scaleY;
            }
            set
            {
                base.scaleY = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float scale
        {
            get
            {
                return base.scale;
            }
            set
            {
                base.scale = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float vertexZ
        {
            get
            {
                return base.vertexZ;
            }
            set
            {
                base.vertexZ = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override CCPoint anchorPoint
        {
            get
            {
                return base.anchorPoint;
            }
            set
            {
                base.anchorPoint = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override bool isRelativeAnchorPoint
        {
            get
            {
                return base.isRelativeAnchorPoint;
            }
            set
            {
                base.isRelativeAnchorPoint = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override bool visible
        {
            get
            {
                return base.visible;
            }
            set
            {
                base.visible = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public void setFlipX(bool bFlipX)
        {
            if (m_bFlipX != bFlipX)
            {
                m_bFlipX = bFlipX;
                setTextureRectInPixels(m_obRectInPixels, m_bRectRotated, m_tContentSizeInPixels);
            }
        }

        public void setFlipY(bool bFlipY)
        {
            if (m_bFlipY != bFlipY)
            {
                m_bFlipY = bFlipY;
                setTextureRectInPixels(m_obRectInPixels, m_bRectRotated, m_tContentSizeInPixels);
            }
        }
        /** whether or not the sprite is flipped horizontally. 
	    It only flips the texture of the sprite, and not the texture of the sprite's children.
	    Also, flipping the texture doesn't alter the anchorPoint.
	    If you want to flip the anchorPoint too, and/or to flip the children too use:

	    sprite->setScaleX(sprite->getScaleX() * -1);
	    */

        public bool isFlipX()
        {
            return m_bFlipX;
        }

        /** whether or not the sprite is flipped vertically.
	    It only flips the texture of the sprite, and not the texture of the sprite's children.
	    Also, flipping the texture doesn't alter the anchorPoint.
	    If you want to flip the anchorPoint too, and/or to flip the children too use:

	    sprite->setScaleY(sprite->getScaleY() * -1);
	    */
        public bool isFlipY()
        {
            return m_bFlipY;
        }

        void updateColor()
        {
            m_sQuad.bl.colors = new ccColor4B(m_sColor.r, m_sColor.g, m_sColor.b, m_nOpacity);
            m_sQuad.br.colors = new ccColor4B(m_sColor.r, m_sColor.g, m_sColor.b, m_nOpacity);
            m_sQuad.tl.colors = new ccColor4B(m_sColor.r, m_sColor.g, m_sColor.b, m_nOpacity);
            m_sQuad.tr.colors = new ccColor4B(m_sColor.r, m_sColor.g, m_sColor.b, m_nOpacity);

            // renders using Sprite Manager
            if (m_bUseBatchNode)
            {
                if (m_uAtlasIndex != ccMacros.CCSpriteIndexNotInitialized)
                {
                    //@todo
                    throw new NotImplementedException();
                }
                else
                {
                    // no need to set it recursively
                    // update dirty_, don't update recursiveDirty_
                    m_bDirty = true;
                }
            }

            // self render
            // do nothing
        }

        // CCTextureProtocol
        public virtual void setTexture(CCTexture2D texture)
        {
            Debug.Assert(!m_bUseBatchNode, "CCSprite: setTexture doesn't work when the sprite is rendered using a CCSpriteBatchNode");

            m_pobTexture = texture;

            updateBlendFunc();
        }
        public virtual CCTexture2D getTexture()
        {
            return m_pobTexture;
        }

        /** Initializes an sprite with a texture.
	     The rect used will be the size of the texture.
	     The offset will be (0,0).
	     */
        public bool initWithTexture(CCTexture2D texture)
        {
            Debug.Assert(texture != null);

            CCRect rect = new CCRect();
            rect.size = texture.getContentSize();

            return initWithTexture(texture, rect);
        }

        /** Initializes an sprite with a texture and a rect.
	     The offset will be (0,0).
	     */
        public bool initWithTexture(CCTexture2D texture, CCRect rect)
        {
            Debug.Assert(texture != null);
            init();
            setTexture(texture);
            setTextureRect(rect);

            return true;
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
            Debug.Assert(null != fileName, "fileName is null");

            CCTexture2D textureFromFile = CCTextureCache.sharedTextureCache().addImage(fileName);

            if (null != textureFromFile)
            {
                CCRect rect = new CCRect();
                rect.origin.x = 0.0f;
                rect.origin.y = 0.0f;
                rect.size = textureFromFile.getContentSize();
                return initWithTexture(textureFromFile, rect);
            }

            return false;
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
            m_uAtlasIndex = ccMacros.CCSpriteIndexNotInitialized;
            m_bUseBatchNode = false;

            /*
             * ///@todo
            m_pobTextureAtlas = NULL;
            m_pobBatchNode = NULL;
             */
            // throw new NotImplementedException();
            m_bDirty = m_bRecursiveDirty = false;

            float x1 = 0 + m_obOffsetPositionInPixels.x;
            float y1 = 0 + m_obOffsetPositionInPixels.y;
            float x2 = x1 + m_obRectInPixels.size.width;
            float y2 = y1 + m_obRectInPixels.size.height;
            m_sQuad.bl.vertices = ccTypes.vertex3(x1, y1, 0);
            m_sQuad.br.vertices = ccTypes.vertex3(x2, y1, 0);
            m_sQuad.tl.vertices = ccTypes.vertex3(x1, y2, 0);
            m_sQuad.tr.vertices = ccTypes.vertex3(x2, y2, 0);
        }

        /** updates the texture rect of the CCSprite in points. */
        public void setTextureRect(CCRect rect)
        {
            CCRect rectInPixels = ccMacros.CC_RECT_POINTS_TO_PIXELS(rect);
            setTextureRectInPixels(rectInPixels, false, rectInPixels.size);
        }

        /** updates the texture rect, rectRotated and untrimmed size of the CCSprite in pixels */
        public void setTextureRectInPixels(CCRect rect, bool rotated, CCSize size)
        {
            m_obRectInPixels = rect;
            m_obRect = ccMacros.CC_RECT_PIXELS_TO_POINTS(rect);
            m_bRectRotated = rotated;

            contentSizeInPixels = size;
            updateTextureCoords(m_obRectInPixels);

            CCPoint relativeOffsetInPixels = m_obUnflippedOffsetPositionFromCenter;

            if (m_bFlipX)
            {
                relativeOffsetInPixels.x = -relativeOffsetInPixels.x;
            }
            if (m_bFlipY)
            {
                relativeOffsetInPixels.y = -relativeOffsetInPixels.y;
            }

            m_obOffsetPositionInPixels.x = relativeOffsetInPixels.x + (m_tContentSizeInPixels.width - m_obRectInPixels.size.width) / 2;
            m_obOffsetPositionInPixels.y = relativeOffsetInPixels.y + (m_tContentSizeInPixels.height - m_obRectInPixels.size.height) / 2;

            // rendering using batch node
            if (m_bUseBatchNode)
            {
                // update dirty_, don't update recursiveDirty_
                m_bDirty = true;
            }
            else
            {
                // self rendering

                // Atlas: Vertex
                float x1 = 0 + m_obOffsetPositionInPixels.x;
                float y1 = 0 + m_obOffsetPositionInPixels.y;
                float x2 = x1 + m_obRectInPixels.size.width;
                float y2 = y1 + m_obRectInPixels.size.height;

                // Don't update Z.
                m_sQuad.bl.vertices = ccTypes.vertex3(x1, y1, 0);
                m_sQuad.br.vertices = ccTypes.vertex3(x2, y1, 0);
                m_sQuad.tl.vertices = ccTypes.vertex3(x1, y2, 0);
                m_sQuad.tr.vertices = ccTypes.vertex3(x2, y2, 0);
            }
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
            // throw new NotImplementedException();
        }

        protected void updateBlendFunc()
        {
            // throw new NotImplementedException();
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

        // image is flipped
        protected bool m_bFlipX;
        protected bool m_bFlipY;
    }

    public struct transformValues_
    {
        CCPoint pos;    // position  x and y
        CCPoint scale;  // scale x and y
        float rotation;
        CCPoint skew;   // skew x and y
        CCPoint ap;     // anchor point in pixels
        bool visible;
    }
}
