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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace cocos2d
{
    /// <summary>
    /// CCSpriteBatchNode is like a batch node: if it contains children, it will draw them in 1 single OpenGL call
    /// (often known as "batch draw").
    /// A CCSpriteBatchNode can reference one and only one texture (one image file, one texture atlas).
    /// Only the CCSprites that are contained in that texture can be added to the CCSpriteBatchNode.
    /// All CCSprites added to a CCSpriteBatchNode are drawn in one OpenGL ES draw call.
    ///  If the CCSprites are not added to a CCSpriteBatchNode then an OpenGL ES draw call will be needed for each one, which is less efficient.
    ///  Limitations:
    ///  - The only object that is accepted as child (or grandchild, grand-grandchild, etc...) is CCSprite or any subclass of CCSprite. eg: particles, labels and layer can't be added to a CCSpriteBatchNode.
    ///  - Either all its children are Aliased or Antialiased. It can't be a mix. This is because "alias" is a property of the texture, and all the sprites share the same texture.
    ///  @since v0.7.1
    /// </summary>
    public class CCSpriteBatchNode : CCNode, CCTextureProtocol
    {
        const int defaultCapacity = 29;

        public CCSpriteBatchNode()
        {

        }

        // property

        // retain
        public CCTextureAtlas TextureAtlas
        {
            get
            {
                return m_pobTextureAtlas;
            }
            set
            {
                if (value != m_pobTextureAtlas)
                {
                    m_pobTextureAtlas = value;
                }
            }
        }

        public List<CCSprite> getDescendants()
        {
            return m_pobDescendants;
        }

        /// <summary>
        /// creates a CCSpriteBatchNode with a texture2d and a default capacity of 29 children.
        /// The capacity will be increased in 33% in runtime if it run out of space.
        /// </summary>
        public static CCSpriteBatchNode batchNodeWithTexture(CCTexture2D tex)
        {
            CCSpriteBatchNode batchNode = new CCSpriteBatchNode();
            batchNode.initWithTexture(tex, defaultCapacity);

            return batchNode;
        }

        /// <summary>
        /// creates a CCSpriteBatchNode with a texture2d and capacity of children.
        /// The capacity will be increased in 33% in runtime if it run out of space.
        /// </summary>
        public static CCSpriteBatchNode batchNodeWithTexture(CCTexture2D tex, uint capacity)
        {
            CCSpriteBatchNode batchNode = new CCSpriteBatchNode();
            batchNode.initWithTexture(tex, capacity);

            return batchNode;
        }

        /// <summary>
        ///  creates a CCSpriteBatchNode with a file image (.png, .jpeg, .pvr, etc) with a default capacity of 29 children.
        ///  The capacity will be increased in 33% in runtime if it run out of space.
        ///  The file will be loaded using the TextureMgr.
        /// </summary>
        public static CCSpriteBatchNode batchNodeWithFile(string fileImage)
        {
            CCSpriteBatchNode batchNode = new CCSpriteBatchNode();
            batchNode.initWithFile(fileImage, defaultCapacity);

            return batchNode;
        }

        /// <summary>
        /// creates a CCSpriteBatchNode with a file image (.png, .jpeg, .pvr, etc) and capacity of children.
        /// The capacity will be increased in 33% in runtime if it run out of space.
        /// The file will be loaded using the TextureMgr.
        /// </summary>
        public static CCSpriteBatchNode batchNodeWithFile(string fileImage, uint capacity)
        {
            CCSpriteBatchNode batchNode = new CCSpriteBatchNode();
            batchNode.initWithFile(fileImage, capacity);

            return batchNode;
        }

        /// <summary>
        ///  initializes a CCSpriteBatchNode with a texture2d and capacity of children.
        ///  The capacity will be increased in 33% in runtime if it run out of space.
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public bool initWithTexture(CCTexture2D tex, uint capacity)
        {
            m_blendFunc.src = 1; // CC_BLEND_SRC = 1
            m_blendFunc.dst = 0x0303; // CC_BLEND_DST = 0x0303

            m_pobTextureAtlas = new CCTextureAtlas();
            m_pobTextureAtlas.initWithTexture(tex, capacity);

            updateBlendFunc();

            // no lazy alloc in this node
            m_pChildren = new List<CCNode>();
            m_pobDescendants = new List<CCSprite>();

            return true;
        }

        /// <summary>
        /// initializes a CCSpriteBatchNode with a file image (.png, .jpeg, .pvr, etc) and a capacity of children.
        /// The capacity will be increased in 33% in runtime if it run out of space.
        /// The file will be loaded using the TextureMgr.
        /// </summary>
        /// <param name="fileImage"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public bool initWithFile(string fileImage, uint capacity)
        {
            CCTexture2D pTexture2D = CCTextureCache.sharedTextureCache().addImage(fileImage);
            return initWithTexture(pTexture2D, capacity);
        }

        public void increaseAtlasCapacity()
        {
            // if we're going beyond the current TextureAtlas's capacity,
            // all the previously initialized sprites will need to redo their texture coords
            // this is likely computationally expensive
            uint quantity = (m_pobTextureAtlas.getCapacity() + 1) * 4 / 3;

            Debug.WriteLine
                (
                    string.Format(
                            "cocos2d: CCSpriteBatchNode: resizing TextureAtlas capacity from [{0}] to [{1}].",
                            (long)m_pobTextureAtlas.getCapacity(),
                            (long)m_pobTextureAtlas.getCapacity()
                            )
                 );

            if ( !m_pobTextureAtlas.resizeCapacity(quantity) )
            {
                // serious problems
                Debug.WriteLine("cocos2d: WARNING: Not enough memory to resize the atlas");
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// removes a child given a certain index. It will also cleanup the running actions depending on the cleanup parameter.
        /// @warning Removing a child from a CCSpriteBatchNode is very slow
        /// </summary>
        /// <param name="index"></param>
        /// <param name="doCleanup"></param>
        public void removeChildAtIndex( int index , bool doCleanup)
        {
            if ( index >= 0 && index < m_pChildren.Count)
            {
                removeChild((CCSprite)(m_pChildren[index]) , doCleanup);
            }
        }

        public void insertChild(CCSprite pobSprite, uint uIndex)
        {
            pobSprite.useBatchNode(this);
            pobSprite.atlasIndex = uIndex;
            pobSprite.dirty = true;

            if (m_pobTextureAtlas.getTotalQuads() == m_pobTextureAtlas.getCapacity())
            {
                increaseAtlasCapacity();
            }

            ccV3F_C4B_T2F_Quad quad = pobSprite.quad;
            m_pobTextureAtlas.insertQuad(quad, uIndex);
            m_pobDescendants.Insert((int) uIndex, pobSprite );

            // update indices
            uint i = 0;

            if (m_pobDescendants != null && m_pobDescendants.Count > 0)
            {
                CCObject pObject = null;

                for (int j = 0; j < m_pobDescendants.Count; j++)
                {
                    pObject = m_pobDescendants[j];
                    CCSprite pChild = pObject as CCSprite;

                    if (pChild != null)
                    {
                        if (i > uIndex)
                        {
                            pChild.atlasIndex = pChild.atlasIndex + 1;
                        }

                        ++i;
                    }
                }
            }

            // add children recursively
            List<CCNode> pChildren = pobSprite.children;

            if (pChildren != null && pChildren.Count > 0)
            {
                CCObject pObject = null;

                for (int j = 0; j < pChildren.Count; j++)
                {
                    pObject = pChildren[j];
                    CCSprite pChild = pObject as CCSprite;

                    if (pChild != null)
                    {
                        uIndex = atlasIndexForChild(pChild, pChild.zOrder);
                        insertChild(pChild, uIndex);
                    }
                }
            }
        }

        public void removeSpriteFromAtlas(CCSprite pobSprite)
        {
            // remove from TextureAtlas
            m_pobTextureAtlas.removeQuadAtIndex(pobSprite.atlasIndex);

            // Cleanup sprite. It might be reused (issue #569)
            pobSprite.useSelfRender();

            uint uIndex = (uint)m_pobDescendants.IndexOf(pobSprite);

            if (uIndex != 0xffffffff)
            {
                m_pobDescendants.RemoveAt((int) uIndex);

                // update all sprites beyond this one
                int count = m_pobDescendants.Count;

                for (; uIndex < count; ++uIndex)
                {
                    CCSprite s = (m_pobDescendants[(int)uIndex]) as CCSprite;
                    s.atlasIndex = s.atlasIndex - 1;
                }
            }

            // remove children recursively
            List<CCNode> pChildren = pobSprite.children;

            if (pChildren != null && pChildren.Count > 0)
            {
                CCObject pObject = null;

                for (int i = 0; i < pChildren.Count; i++)
                {
                    pObject = pChildren[i];
                    CCSprite pChild = pObject as CCSprite;

                    if (pChild != null)
                    {
                        removeSpriteFromAtlas(pChild);
                    }
                }
            }
        }

        public uint rebuildIndexInOrder(CCSprite pobParent, uint uIndex)
        {
            List<CCNode> pChildren = pobParent.children;

            if (pChildren != null && pChildren.Count > 0)
            {
                CCObject pObject = null;

                for (int i = 0; i < pChildren.Count; i++)
                {
                    pObject = pChildren[i];
                    CCSprite pChild = pObject as CCSprite;

                    if (pChild != null && (pChild.zOrder < 0))
                    {
                        uIndex = rebuildIndexInOrder(pChild, uIndex);
                    }
                }
            }

            // ignore self (batch node)
            if ( !pobParent.Equals(this))
            {
                pobParent.atlasIndex = uIndex;
                uIndex++;
            }

            if ( pChildren != null && pChildren.Count > 0)
            {
                CCObject pObject = null;

                for (int i = 0; i < pChildren.Count; i++)
                {
                    pObject = pChildren[i];
                    CCSprite pChild = pObject as CCSprite;

                    if ( pChild != null && (pChild.zOrder >= 0))
                    {
                        uIndex = rebuildIndexInOrder(pChild, uIndex);
                    }
                }
            }

            return uIndex;
        }

        public uint highestAtlasIndexInChild(CCSprite pSprite)
        {
            List<CCNode> pChildren = pSprite.children;

            if ( pChildren != null || pChildren.Count == 0)
            {
                return pSprite.atlasIndex;
            }
            else
            {
                return highestAtlasIndexInChild((pChildren.Last()) as CCSprite);
            }
        }

        public uint lowestAtlasIndexInChild(CCSprite pSprite)
        {
            List<CCNode> pChildren = pSprite.children;

            if (pChildren != null || pChildren.Count == 0)
            {
                return pSprite.atlasIndex;
            }
            else
            {
                return lowestAtlasIndexInChild((pChildren[0]) as CCSprite);
            }
        }

        public uint atlasIndexForChild(CCSprite pobSprite, int nZ)
        {
            List<CCNode> pBrothers = pobSprite.parent.children;

            uint uChildIndex =(uint) pBrothers.IndexOf(pobSprite);

            // ignore parent Z if parent is spriteSheet
            bool bIgnoreParent = (CCSpriteBatchNode)(pobSprite.parent) == this;
            CCSprite pPrevious = null;

            if (uChildIndex > 0 &&
                uChildIndex < 0xffffffff)
            {
                pPrevious = pBrothers[(int)(uChildIndex - 1)] as CCSprite;
            }

            // first child of the sprite sheet
            if (bIgnoreParent)
            {
                if (uChildIndex == 0)
                {
                    return 0;
                }

                return highestAtlasIndexInChild(pPrevious) + 1;
            }

            // parent is a CCSprite, so, it must be taken into account

            // first child of an CCSprite ?
            if (uChildIndex == 0)
            {
                CCSprite p = pobSprite.parent as CCSprite;

                if (p == null)
                {
                    return 0;
                }

                // less than parent and brothers
                if (nZ < 0)
                {
                    return p.atlasIndex;
                }
                else
                {
                    return p.atlasIndex + 1;
                }
            }
            else
            {
                // previous & sprite belong to the same branch
                if ((pPrevious.zOrder < 0 && nZ < 0) || (pPrevious.zOrder >= 0 && nZ >= 0))
                {
                    return highestAtlasIndexInChild(pPrevious) + 1;
                }

                // else (previous < 0 and sprite >= 0 )
                CCSprite p = pobSprite.parent as CCSprite;
                return p.atlasIndex + 1;
            }

            // Should not happen. Error  calculating Z on SpriteSheet
            Debug.Assert(false);
            return 0;
        }

        //public virtual CCTexture2D gsTexture
        //{
        //    get
        //    {
        //        return m_pobTextureAtlas.getTexture();
        //    }
        //    set
        //    {
        //        m_pobTextureAtlas.setTexture(value);
        //        updateBlendFunc();
        //    }
        //}

        public virtual CCTexture2D getTexture()
        {
            return m_pobTextureAtlas.getTexture();
        }

        public virtual void setTexture(CCTexture2D texture)
        {
            m_pobTextureAtlas.setTexture(texture);
            updateBlendFunc();
        }

        //public virtual ccBlendFunc BlendFunc
        //{
        //    get
        //    {
        //        return m_blendFunc;
        //    }
        //    set
        //    {
        //        m_blendFunc = value;
        //    }
        //}

        public virtual void setBlendFunc(ccBlendFunc blendFunc)
        {
            m_blendFunc = blendFunc;
        }

        public virtual ccBlendFunc getBlendFunc()
        {
            return m_blendFunc;
        }

        public override void visit()
        {
            // CAREFUL:
            // This visit is almost identical to CocosNode#visit
            // with the exception that it doesn't call visit on it's children
            //
            // The alternative is to have a void CCSprite#visit, but
            // although this is less mantainable, is faster
            //
            throw new NotImplementedException();

            if (!m_bIsVisible)
            {
                return;
            }

            CCNode node;
            int i = 0;
#warning In C# glPushMatrix() is not exist
            //glPushMatrix();

            //if (m_pGrid && m_pGrid->isActive())
            //{
            //    m_pGrid->beforeDraw();
            //    transformAncestors();
            //}

            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                for (; i < m_pChildren.Count; ++i)
                {
                    node = m_pChildren[i];
                    if (node != null && node.zOrder < 0)
                    {
                        node.visit();
                    }
                    else
                    {
                        break;
                    }
                }
                transformAncestors();
            }

            transform();

            draw();

            //if (m_pGrid && m_pGrid->isActive())
            //{
            //    m_pGrid->afterDraw(this);
            //}
            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                for (; i < m_pChildren.Count; ++i)
                {
                    node = m_pChildren[i];

                    if (node != null)
                    {
                        node.visit();
                    }
                }
            }
#warning In C# glPopMatrix is not exist
            //glPopMatrix();
        }

        public override void addChild(CCNode child)
        {
            addChild(child);
        }

        public override void addChild(CCNode child, int zOrder)
        {
            addChild(child, zOrder);
        }

        public override void addChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null);

            CCSprite pSprite = child as CCSprite;

            if (pSprite == null)
            {
                return;
            }
            // check CCSprite is using the same texture id
            Debug.Assert(pSprite.getTexture() == m_pobTextureAtlas.getTexture());

            addChild(child, zOrder, tag);

            uint uIndex = atlasIndexForChild(pSprite, zOrder);
            insertChild(pSprite, uIndex);
        }

        public override void reorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null);
            Debug.Assert(m_pChildren.Contains(child));

            if (zOrder == child.zOrder)
            {
                return;
            }

            // xxx: instead of removing/adding, it is more efficient ot reorder manually
            removeChild((CCSprite)child, false);
            addChild(child, zOrder);
        }

        public override void removeChild(CCNode child, bool cleanup)
        {
            CCSprite pSprite = child as CCSprite;

            // explicit null handling
            if (pSprite == null)
            {
                return;
            }

            Debug.Assert(m_pChildren.Contains(pSprite));

            // cleanup before removing
            removeSpriteFromAtlas(pSprite);

            removeChild(pSprite, cleanup);
        }

        public override void removeAllChildrenWithCleanup(bool cleanup)
        {
            // Invalidate atlas index. issue #569
            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                CCObject pObject = null;

                for (int i = 0; i < m_pChildren.Count; i++)
                {
                    pObject = m_pChildren[i];
                    CCSprite pChild = pObject as CCSprite;

                    if (pChild != null)
                    {
                        removeSpriteFromAtlas(pChild);
                    }
                }
            }

            removeAllChildrenWithCleanup(cleanup);

            m_pobDescendants.Clear();
            m_pobTextureAtlas.removeAllQuads();
        }

        public override void draw()
        {
            base.draw();

            // Optimization: Fast Dispatch	
            if (m_pobTextureAtlas.getTotalQuads() == 0)
            {
                return;
            }

            if (m_pobDescendants != null && m_pobDescendants.Count > 0)
            {
                CCObject pObject = null;

                for (int i = 0; i < m_pobDescendants.Count; i++)
                {
                    pObject = m_pobDescendants[i];
                    CCSprite pChild = pObject as CCSprite;

                    if (pChild != null)
                    {
                        // fast dispatch
                        pChild.updateTransform();

//#if CC_SPRITEBATCHNODE_DEBUG_DRAW
//                    // issue #528
//                    CCRect rect = pChild->boundingBox();
//                    CCPoint vertices[4]={
//                        ccp(rect.origin.x,rect.origin.y),
//                        ccp(rect.origin.x+rect.size.width,rect.origin.y),
//                        ccp(rect.origin.x+rect.size.width,rect.origin.y+rect.size.height),
//                        ccp(rect.origin.x,rect.origin.y+rect.size.height),
//                    };
//                    ccDrawPoly(vertices, 4, true);
//#endif // CC_SPRITEBATCHNODE_DEBUG_DRAW
                    }
                }
            }

             //Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
             //Needed states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
             //Unneeded states: -
            bool newBlend = m_blendFunc.src != 1 || m_blendFunc.dst != 0x0303;

            if (newBlend)
            {
#warning In C# glBlendFunc is not exist
                //glBlendFunc(m_blendFunc.src, m_blendFunc.dst);
            }

            m_pobTextureAtlas.drawQuads();

            if (newBlend)
            {
#warning In C# glBlendFunc is not exist
                //glBlendFunc(1, 0x0303);
            }
        }

        /// <summary>
        /// IMPORTANT XXX IMPORTNAT:
        /// These 2 methods can't be part of CCTMXLayer since they call [super add...], and CCSpriteSheet#add SHALL not be called
        /// Adds a quad into the texture atlas but it won't be added into the children array.
        /// This method should be called only when you are dealing with very big AtlasSrite and when most of the CCSprite won't be updated.
        /// For example: a tile map (CCTMXMap) or a label with lots of characters (BitmapFontAtlas)
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="index"></param>
        protected void addQuadFromSprite(CCSprite sprite, uint index)
        {
            Debug.Assert(sprite != null, "Argument must be non-nil");
            /// @todo CCAssert( [sprite isKindOfClass:[CCSprite class]], @"CCSpriteSheet only supports CCSprites as children");

            while (index >= m_pobTextureAtlas.getCapacity() || m_pobTextureAtlas.getCapacity() == m_pobTextureAtlas.getTotalQuads())
            {
                this.increaseAtlasCapacity();
            }
            //
            // update the quad directly. Don't add the sprite to the scene graph
            //
            sprite.useBatchNode(this);
            sprite.atlasIndex = index;

            ccV3F_C4B_T2F_Quad quad = sprite.quad;
            m_pobTextureAtlas.insertQuad(quad, index);

            // XXX: updateTransform will update the textureAtlas too using updateQuad.
            // XXX: so, it should be AFTER the insertQuad
            sprite.dirty = true;
            sprite.updateTransform();
        }

        /// <summary>
        /// This is the opposite of "addQuadFromSprite.
        /// It add the sprite to the children and descendants array, but it doesn't update add it to the texture atlas
        /// </summary>
        /// <param name="child"></param>
        /// <param name="z"></param>
        /// <param name="aTag"></param>
        /// <returns></returns>
        protected CCSpriteBatchNode addSpriteWithoutQuad(CCSprite child, uint z, int aTag)
        {
            Debug.Assert(child != null, "Argument must be non-nil");
            /// @todo CCAssert( [child isKindOfClass:[CCSprite class]], @"CCSpriteSheet only supports CCSprites as children");

            // quad index is Z
            child.atlasIndex = z;

            // XXX: optimize with a binary search
            int i = 0;

            if (m_pobDescendants != null && m_pobDescendants.Count > 0)
            {
                CCObject pObject = null;

                for (int j = 0; j < m_pobDescendants.Count; j++)
                {
                    pObject = m_pobDescendants[i];
                    CCSprite pChild = pObject as CCSprite;

                    if (pChild != null && (pChild.atlasIndex >= z))
                    {
                        ++i;
                    }
                }
            }
            m_pobDescendants.Insert(i, child);

            // I  MPORTANT: Call super, and not self. Avoid adding it to the texture atlas array
            CCNode ccNode = new CCNode();
            ccNode.addChild(child, (int)z, aTag);
            return this;
        }

        private void updateBlendFunc()
        {
            if (!m_pobTextureAtlas.getTexture().getHasPremultipliedAlpha())
            {
                m_blendFunc.src = 0x0302;
                m_blendFunc.dst = 0x0303;
            }
        }

        protected CCTextureAtlas m_pobTextureAtlas;
        protected ccBlendFunc m_blendFunc;

        // all descendants: chlidren, gran children, etc...
        protected List<CCSprite> m_pobDescendants;

    }
}
