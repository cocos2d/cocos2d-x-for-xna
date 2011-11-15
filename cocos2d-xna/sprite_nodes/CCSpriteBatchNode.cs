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
        { }

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
                    //CC_SAFE_RETAIN(textureAtlas);
                    //CC_SAFE_RELEASE(m_pobTextureAtlas);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// creates a CCSpriteBatchNode with a texture2d and capacity of children.
        /// The capacity will be increased in 33% in runtime if it run out of space.
        /// </summary>
        public static CCSpriteBatchNode batchNodeWithTexture(CCTexture2D tex, uint capacity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  creates a CCSpriteBatchNode with a file image (.png, .jpeg, .pvr, etc) with a default capacity of 29 children.
        ///  The capacity will be increased in 33% in runtime if it run out of space.
        ///  The file will be loaded using the TextureMgr.
        /// </summary>
        public static CCSpriteBatchNode batchNodeWithFile(string fileImage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// creates a CCSpriteBatchNode with a file image (.png, .jpeg, .pvr, etc) and capacity of children.
        /// The capacity will be increased in 33% in runtime if it run out of space.
        /// The file will be loaded using the TextureMgr.
        /// </summary>
        public static CCSpriteBatchNode batchNodeWithFile(string fileImage, uint capacity)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// initializes a CCSpriteBatchNode with a file image (.png, .jpeg, .pvr, etc) and a capacity of children.
        /// The capacity will be increased in 33% in runtime if it run out of space.
        /// The file will be loaded using the TextureMgr.
        /// </summary>
        /// <param name="fileImage"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public static bool initWithFile(string fileImage, uint capacity)
        {
            throw new NotImplementedException();
        }

        public void increaseAtlasCapacity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// removes a child given a certain index. It will also cleanup the running actions depending on the cleanup parameter.
        /// @warning Removing a child from a CCSpriteBatchNode is very slow
        /// </summary>
        /// <param name="index"></param>
        /// <param name="doCleanup"></param>
        public void removeChildAtIndex(uint index, bool doCleanup)
        {
            throw new NotImplementedException();
        }

        public void insertChild(CCSprite child, uint index)
        {
            throw new NotImplementedException();
        }

        public void removeSpriteFromAtlas(CCSprite sprite)
        {
            throw new NotImplementedException();
        }

        public uint rebuildIndexInOrder(CCSprite parent, uint index)
        {
            throw new NotImplementedException();
        }

        public uint highestAtlasIndexInChild(CCSprite sprite)
        {
            throw new NotImplementedException();
        }

        public uint lowestAtlasIndexInChild(CCSprite sprite)
        {
            throw new NotImplementedException();
        }

        public uint atlasIndexForChild(CCSprite sprite, int z)
        {
            throw new NotImplementedException();
        }

        // CCTextureProtocol
        public virtual CCTexture2D getTexture()
        {
            throw new NotImplementedException();
        }

        public virtual void setTexture(CCTexture2D texture)
        {
            throw new NotImplementedException();
        }

        public virtual void setBlendFunc(ccBlendFunc blendFunc)
        {
            throw new NotImplementedException();
        }

        public virtual ccBlendFunc getBlendFunc()
        {
            throw new NotImplementedException();
        }

        public override void visit()
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

        public override void reorderChild(CCNode child, int zOrder)
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

        public override void draw()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private void updateBlendFunc()
        {
            throw new NotImplementedException();
        }

        protected CCTextureAtlas m_pobTextureAtlas;
        protected ccBlendFunc m_blendFunc;

        // all descendants: chlidren, gran children, etc...
        protected List<CCSprite> m_pobDescendants;

    }
}
