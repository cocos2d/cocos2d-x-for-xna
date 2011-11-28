/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
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

        private bool m_bDirty;
        /// <summary>
        /// whether or not the Sprite needs to be updated in the Atlas
        /// </summary>
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

        private ccV3F_C4B_T2F_Quad m_sQuad;
        /// <summary>
        /// get the quad (tex coords, vertex coords and color) information
        /// </summary>
        public ccV3F_C4B_T2F_Quad quad
        {
            // read only
            get
            {
                return m_sQuad;
            }
        }

        private bool m_bRectRotated;
        /// <summary>
        /// returns whether or not the texture rectangle is rotated
        /// </summary>
        public bool rectRotated
        {
            // read only
            get
            {
                return m_bRectRotated;
            }
        }

        private uint m_uAtlasIndex;
        /// <summary>
        /// The index used on the TextureAtlas. Don't modify this value unless you know what you are doing
        /// </summary>
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

        private CCRect m_obTextureRect;
        /// <summary>
        /// returns the rect of the CCSprite in points
        /// </summary>
        public CCRect textureRect
        {
            // read only
            get
            {
                return m_obTextureRect;
            }
        }

        /// <summary>
        /// whether or not the Sprite is rendered using a CCSpriteBatchNode
        /// </summary>
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

        private ccHonorParentTransform m_eHonorParentTransform;
        /// <summary>
        /// whether or not to transform according to its parent transfomrations.
        /// Useful for health bars. eg: Don't rotate the health bar, even if the parent rotates.
        /// IMPORTANT: Only valid if it is rendered using an CCSpriteBatchNode.
        /// @since v0.99.0
        /// </summary>
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

        /// <summary>
        /// offset position in pixels of the sprite in points. Calculated automatically by editors like Zwoptex.
        /// @since v0.99.0
        /// </summary>
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

        protected CCTextureAtlas m_pobTextureAtlas;
        protected CCSpriteBatchNode m_pobBatchNode;

        public override void draw()
        {
            base.draw();

            if (m_pobTexture == null)
            {
                return;
            }

            //get the point of cocos2d
            CCPoint cocos2dPoint = new CCPoint(position.x - contentSizeInPixels.width * anchorPoint.x,
                 position.y - contentSizeInPixels.height * anchorPoint.y);

            //uiPoint = this.convertToWorldSpace(uiPoint);
            CCPoint uiPoint = CCAffineTransform.CCPointApplyAffineTransform(new CCPoint(0, 0), m_tNodeToWorldTransform);

            m_pobTexture.drawAtPoint(uiPoint);
        }

        /// <summary>
        /// Creates an sprite with a texture.
        /// The rect used will be the size of the texture.
        /// The offset will be (0,0).
        /// </summary>
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

        /// <summary>
        /// Creates an sprite with a texture and a rect.
        /// The offset will be (0,0).
        /// </summary>
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

        /// <summary>
        /// Creates an sprite with a texture, a rect and offset. 
        /// </summary>
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

        /// <summary>
        /// Creates an sprite with an image filename.
        /// The rect used will be the size of the image.
        /// The offset will be (0,0).
        /// </summary>
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

        /// <summary>
        /// Creates an sprite with an image filename and a rect.
        /// The offset will be (0,0).
        /// </summary>
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

        public override void removeChild(CCNode child, bool cleanup)
        {
            if (m_bUseBatchNode)
            {
                m_pobBatchNode.removeSpriteFromAtlas((CCSprite)(child));
            }

            base.removeChild(child, cleanup);
        }

        public override void removeAllChildrenWithCleanup(bool cleanup)
        {
            if (m_bUseBatchNode)
            {
                foreach (CCNode node in m_pChildren)
                {
                    m_pobBatchNode.removeSpriteFromAtlas((CCSprite)(node));
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

        /// <summary>
        /// Initializes an sprite with a texture.
        /// The rect used will be the size of the texture.
        /// The offset will be (0,0).
        /// </summary>
        public bool initWithTexture(CCTexture2D texture)
        {
            Debug.Assert(texture != null);

            CCRect rect = new CCRect();
            rect.size = texture.getContentSize();

            return initWithTexture(texture, rect);
        }

        /// <summary>
        /// Initializes an sprite with a texture and a rect.
        /// The offset will be (0,0).
        /// </summary>
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

        /// <summary>
        /// Initializes an sprite with an image filename.
        /// The rect used will be the size of the image.
        /// The offset will be (0,0).
        /// </summary>
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

        /// <summary>
        /// Initializes an sprite with an image filename, and a rect.
        /// The offset will be (0,0).
        /// </summary>
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

        /// <summary>
        /// updates the quad according the the rotation, position, scale values. 
        /// </summary>
        public void updateTransform()
        {
            //assert(m_bUsesBatchNode);

            // optimization. Quick return if not dirty
            if (!m_bDirty)
            {
                return;
            }

            CCAffineTransform matrix;

            // Optimization: if it is not visible, then do nothing
            if (!m_bIsVisible)
            {
                m_sQuad.br.vertices = m_sQuad.tl.vertices = m_sQuad.tr.vertices = m_sQuad.bl.vertices = new ccVertex3F(0, 0, 0);
                m_pobTextureAtlas.updateQuad(m_sQuad, m_uAtlasIndex);
                //m_bDirty = m_bRecursiveDirty = false;
                return;
            }

            // Optimization: If parent is batchnode, or parent is nil
            // build Affine transform manually
            if (m_pParent == null || m_pParent == m_pobBatchNode)
            {
                float radians = -ccMacros.CC_DEGREES_TO_RADIANS(m_fRotation);
                float c = (float)Math.Cos(radians);
                float s = (float)Math.Sin(radians);

                matrix = CCAffineTransform.CCAffineTransformMake(c * m_fScaleX, s * m_fScaleX,
                    -s * m_fScaleY, c * m_fScaleY,
                    m_tPositionInPixels.x, m_tPositionInPixels.y);
                if (m_fSkewX > 0 || m_fSkewY > 0)
                {
                    CCAffineTransform skewMatrix = CCAffineTransform.CCAffineTransformMake(1.0f, (float)Math.Tan(ccMacros.CC_DEGREES_TO_RADIANS(m_fSkewY)),
                        (float)Math.Tan(ccMacros.CC_DEGREES_TO_RADIANS(m_fSkewX)), 1.0f,
                        0.0f, 0.0f);
                    matrix = CCAffineTransform.CCAffineTransformConcat(skewMatrix, matrix);
                }
                matrix = CCAffineTransform.CCAffineTransformTranslate(matrix, -m_tAnchorPointInPixels.x, -m_tAnchorPointInPixels.y);
            }
            else // parent_ != batchNode_ 
            {
                // else do affine transformation according to the HonorParentTransform
                matrix = CCAffineTransform.CCAffineTransformMakeIdentity();
                ccHonorParentTransform prevHonor = ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_ALL;

                CCNode p = this;
                while (p != null && p != m_pobBatchNode)
                {
                    // Might happen. Issue #1053
                    // how to implement, we can not use dynamic
                    // CCAssert( [p isKindOfClass:[CCSprite class]], @"CCSprite should be a CCSprite subclass. Probably you initialized an sprite with a batchnode, but you didn't add it to the batch node." );
                    transformValues_ tv = new transformValues_();
                    ((CCSprite)p).getTransformValues(tv);

                    // If any of the parents are not visible, then don't draw this node
                    if (!tv.visible)
                    {
                        m_sQuad.br.vertices = m_sQuad.tl.vertices = m_sQuad.tr.vertices = m_sQuad.bl.vertices = new ccVertex3F(0, 0, 0);
                        m_pobTextureAtlas.updateQuad(m_sQuad, m_uAtlasIndex);
                        m_bDirty = m_bRecursiveDirty = false;

                        return;
                    }

                    CCAffineTransform newMatrix = CCAffineTransform.CCAffineTransformMakeIdentity();

                    // 2nd: Translate, Skew, Rotate, Scale
                    if ((int)prevHonor != 0 & (int)ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_TRANSLATE != 0)
                    {
                        newMatrix = CCAffineTransform.CCAffineTransformTranslate(newMatrix, tv.pos.x, tv.pos.y);
                    }

                    if ((int)prevHonor != 0 & (int)ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_ROTATE != 0)
                    {
                        newMatrix = CCAffineTransform.CCAffineTransformRotate(newMatrix, -ccMacros.CC_DEGREES_TO_RADIANS(tv.rotation));
                    }

                    if ((int)prevHonor != 0 & (int)ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_SKEW != 0)
                    {
                        CCAffineTransform skew = CCAffineTransform.CCAffineTransformMake(1.0f,
                            (float)Math.Tan(ccMacros.CC_DEGREES_TO_RADIANS(tv.skew.y)),
                            (float)Math.Tan(ccMacros.CC_DEGREES_TO_RADIANS(tv.skew.x)), 1.0f, 0.0f, 0.0f);
                        // apply the skew to the transform
                        newMatrix = CCAffineTransform.CCAffineTransformConcat(skew, newMatrix);
                    }

                    if ((int)prevHonor != 0 & (int)ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_SCALE != 0)
                    {
                        newMatrix = CCAffineTransform.CCAffineTransformScale(newMatrix, tv.scale.x, tv.scale.y);
                    }

                    // 3rd: Translate anchor point
                    newMatrix = CCAffineTransform.CCAffineTransformTranslate(newMatrix, -tv.ap.x, -tv.ap.y);

                    // 4th: Matrix multiplication
                    matrix = CCAffineTransform.CCAffineTransformConcat(matrix, newMatrix);

                    prevHonor = ((CCSprite)p).honorParentTransform;

                    p = p.parent;
                }
            }

            //
            // calculate the Quad based on the Affine Matrix
            //
            CCSize size = m_obRectInPixels.size;

            float x1 = m_obOffsetPositionInPixels.x;
            float y1 = m_obOffsetPositionInPixels.y;

            float x2 = x1 + size.width;
            float y2 = y1 + size.height;
            float x = matrix.tx;
            float y = matrix.ty;

            float cr = matrix.a;
            float sr = matrix.b;
            float cr2 = matrix.d;
            float sr2 = -matrix.c;
            float ax = x1 * cr - y1 * sr2 + x;
            float ay = x1 * sr + y1 * cr2 + y;

            float bx = x2 * cr - y1 * sr2 + x;
            float by = x2 * sr + y1 * cr2 + y;

            float cx = x2 * cr - y2 * sr2 + x;
            float cy = x2 * sr + y2 * cr2 + y;

            float dx = x1 * cr - y2 * sr2 + x;
            float dy = x1 * sr + y2 * cr2 + y;

            m_sQuad.bl.vertices = new ccVertex3F((float)ax, (float)ay, m_fVertexZ);
            m_sQuad.br.vertices = new ccVertex3F((float)bx, (float)by, m_fVertexZ);
            m_sQuad.tl.vertices = new ccVertex3F((float)dx, (float)dy, m_fVertexZ);
            m_sQuad.tr.vertices = new ccVertex3F((float)cx, (float)cy, m_fVertexZ);

            m_pobTextureAtlas.updateQuad(m_sQuad, m_uAtlasIndex);
            m_bDirty = m_bRecursiveDirty = false;
        }

        /// <summary>
        /// tell the sprite to use self-render.
        /// @since v0.99.0
        /// </summary>
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

        /// <summary>
        /// updates the texture rect of the CCSprite in points.
        /// </summary>
        public void setTextureRect(CCRect rect)
        {
            CCRect rectInPixels = ccMacros.CC_RECT_POINTS_TO_PIXELS(rect);
            setTextureRectInPixels(rectInPixels, false, rectInPixels.size);
        }

        /// <summary>
        /// updates the texture rect, rectRotated and untrimmed size of the CCSprite in pixels
        /// </summary>
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

        /// <summary>
        /// tell the sprite to use batch node render.
        /// @since v0.99.0
        /// </summary>
        public void useBatchNode(CCSpriteBatchNode batchNode)
        {
            m_bUseBatchNode = true;
            m_pobTextureAtlas = batchNode.TextureAtlas; // weak ref
            m_pobBatchNode = batchNode;
        }

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
            tv.pos = m_tPositionInPixels;
            tv.scale.x = m_fScaleX;
            tv.scale.y = m_fScaleY;
            tv.rotation = m_fRotation;
            tv.skew.x = m_fSkewX;
            tv.skew.y = m_fSkewY;
            tv.ap = m_tAnchorPointInPixels;
            tv.visible = m_bIsVisible;
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

    public class transformValues_
    {
        public CCPoint pos;  // position  x and y
        public CCPoint scale;  // scale x and y
        public float rotation;
        public CCPoint skew;   // skew x and y
        public CCPoint ap;     // anchor point in pixels
        public bool visible;
    }
}
