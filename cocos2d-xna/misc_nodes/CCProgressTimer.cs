/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

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
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    /// <summary>
    /// CCProgresstimer is a subclass of CCNode.
    /// It renders the inner sprite according to the percentage.
    /// The progress can be Radial, Horizontal or vertical.
    /// @since v0.99.1
    /// </summary>
    public class CCProgressTimer : CCNode
    {
        private const int kProgressTextureCoordsCount = 4;
        const int kProgressTextureCoords = 0x1e;

        #region properties

        /// <summary>
        /// Change the percentage to change progress.
        /// </summary>
        public CCProgressTimerType Type
        {
            get { return m_eType; }
            set
            {
                if (value != m_eType)
                {
                    //	release all previous information
                    if (m_pVertexData == null)
                    {
                        // delete[] m_pVertexData;
                        m_pVertexData = null;
                        m_nVertexDataCount = 0;
                    }

                    m_eType = value;
                }
            }
        }

        /// <summary>
        /// Percentages are from 0 to 100
        /// </summary>
        public float Percentage
        {
            get { return m_fPercentage; }
            set
            {
                if (m_fPercentage != value)
                {
                    m_fPercentage = CCPointExtension.clampf(value, 0, 100);
                    updateProgress();
                }
            }
        }

        /// <summary>
        /// The image to show the progress percentage, retain
        /// </summary>
        public CCSprite Sprite
        {
            get { return m_pSprite; }
            set
            {
                if (m_pSprite != value)
                {
                    m_pSprite = value;
                    contentSize = m_pSprite.contentSize;

                    //	Everytime we set a new sprite, we free the current vertex data
                    if (m_pVertexData != null)
                    {
                        m_pVertexData = null;
                        m_nVertexDataCount = 0;
                    }
                }
            }
        }

        #endregion

        #region init

        public static CCProgressTimer progressWithFile(string pszFileName)
        {
            CCProgressTimer pProgressTimer = new CCProgressTimer();
            if (pProgressTimer.initWithFile(pszFileName))
            {
                return pProgressTimer;
            }

            return null;
        }

        public static CCProgressTimer progressWithTexture(CCTexture2D pTexture)
        {
            CCProgressTimer pProgressTimer = new CCProgressTimer();
            if (pProgressTimer.initWithTexture(pTexture))
            {
                return pProgressTimer;
            }

            return null;
        }

        public bool initWithFile(string pszFileName)
        {
            return this.initWithTexture(CCTextureCache.sharedTextureCache().addImage(pszFileName));
        }

        public bool initWithTexture(CCTexture2D pTexture)
        {
            m_pSprite = CCSprite.spriteWithTexture(pTexture);
            m_fPercentage = 0;
            m_pVertexData = null;
            m_nVertexDataCount = 0;
            anchorPoint = new CCPoint(0.5f, 0.5f);
            contentSize = m_pSprite.contentSize;
            m_eType = CCProgressTimerType.kCCProgressTimerTypeRadialCCW;

            return true;
        }

        #endregion

        private void getIndexes()
        {
            if (indexes == null)
            {
                switch (m_eType)
                {
                    case CCProgressTimerType.kCCProgressTimerTypeRadialCCW:
                        indexes = new short[15];

                        indexes[0] = 1;
                        indexes[1] = 0;
                        indexes[2] = 2;

                        indexes[3] = 2;
                        indexes[4] = 0;
                        indexes[5] = 3;

                        indexes[6] = 3;
                        indexes[7] = 0;
                        indexes[8] = 4;

                        indexes[9] = 4;
                        indexes[10] = 0;
                        indexes[11] = 5;

                        indexes[12] = 5;
                        indexes[13] = 0;
                        indexes[14] = 6;

                        break;
                    case CCProgressTimerType.kCCProgressTimerTypeRadialCW:
                        indexes = new short[15];
                        indexes[0] = 0;
                        indexes[1] = 1;
                        indexes[2] = 2;

                        indexes[3] = 0;
                        indexes[4] = 2;
                        indexes[5] = 3;

                        indexes[6] = 0;
                        indexes[7] = 3;
                        indexes[8] = 4;

                        indexes[9] = 0;
                        indexes[10] = 4;
                        indexes[11] = 5;

                        indexes[12] = 0;
                        indexes[13] = 5;
                        indexes[14] = 6;

                        break;
                    case CCProgressTimerType.kCCProgressTimerTypeHorizontalBarRL:
                        indexes = new short[6];
                        indexes[0] = 0;
                        indexes[1] = 1;
                        indexes[2] = 2;
                        indexes[3] = 2;
                        indexes[4] = 1;
                        indexes[5] = 3;
                        break;

                    case CCProgressTimerType.kCCProgressTimerTypeHorizontalBarLR:
                        indexes = new short[6];
                        indexes[0] = 1;
                        indexes[1] = 0;
                        indexes[2] = 2;
                        indexes[3] = 1;
                        indexes[4] = 2;
                        indexes[5] = 3;

                        break;

                    case CCProgressTimerType.kCCProgressTimerTypeVerticalBarBT:
                        indexes = new short[6];
                        indexes[0] = 1;
                        indexes[1] = 0;
                        indexes[2] = 2;
                        indexes[3] = 1;
                        indexes[4] = 2;
                        indexes[5] = 3;

                        break;

                    case CCProgressTimerType.kCCProgressTimerTypeVerticalBarTB:
                        indexes = new short[6];
                        indexes[0] = 1;
                        indexes[1] = 0;
                        indexes[2] = 2;
                        indexes[3] = 1;
                        indexes[4] = 2;
                        indexes[5] = 3;
                        break;
                }
            }
        }

        public override void draw()
        {
            base.draw();

            if (m_pVertexData == null)
            {
                return;
            }

            if (m_pSprite == null)
            {
                return;
            }

            vertices = new VertexPositionColorTexture[m_pVertexData.Length];
            getIndexes();

            for (int i = 0; i < m_pVertexData.Length; i++)
            {
                ccV2F_C4B_T2F temp = m_pVertexData[i];

                vertices[i] = new VertexPositionColorTexture(
                    new Microsoft.Xna.Framework.Vector3(temp.vertices.x, temp.vertices.y, 0),
                    new Microsoft.Xna.Framework.Color(temp.colors.r, temp.colors.g, temp.colors.b, temp.colors.a),
                    new Microsoft.Xna.Framework.Vector2(temp.texCoords.u, temp.texCoords.v));
            }

            CCApplication app = CCApplication.sharedApplication();

            app.basicEffect.Texture = m_pSprite.Texture.getTexture2D();
            app.basicEffect.TextureEnabled = true;

            VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Color, 0),
                    new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                });

            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(
                    PrimitiveType.TriangleList,
                    vertices, 0, m_nVertexDataCount,
                    indexes, 0, m_nVertexDataCount - 2);
            }
        }

        /// <summary>
        /// the vertex position from the texture coordinate
        /// </summary>
        protected ccVertex2F vertexFromTexCoord(CCPoint texCoord)
        {
            CCPoint tmp;
            ccVertex2F ret = new ccVertex2F();

            CCTexture2D pTexture = m_pSprite.Texture;
            if (pTexture != null)
            {
                float fXMax = Math.Max(m_pSprite.quad.br.texCoords.u, m_pSprite.quad.bl.texCoords.u);
                float fXMin = Math.Min(m_pSprite.quad.br.texCoords.u, m_pSprite.quad.bl.texCoords.u);
                float fYMax = Math.Max(m_pSprite.quad.tl.texCoords.v, m_pSprite.quad.bl.texCoords.v);
                float fYMin = Math.Min(m_pSprite.quad.tl.texCoords.v, m_pSprite.quad.bl.texCoords.v);
                CCPoint tMax = new CCPoint(fXMax, fYMax);
                CCPoint tMin = new CCPoint(fXMin, fYMin);

                CCSize texSize = new CCSize(m_pSprite.quad.br.vertices.x - m_pSprite.quad.bl.vertices.x,
                                            m_pSprite.quad.tl.vertices.y - m_pSprite.quad.bl.vertices.y);
                tmp = new CCPoint(texSize.width * (texCoord.x - tMin.x) / (tMax.x - tMin.x),
                            texSize.height * (1 - (texCoord.y - tMin.y) / (tMax.y - tMin.y)));
            }
            else
            {
                tmp = new CCPoint(0, 0);
            }

            ret.x = tmp.x;
            ret.y = tmp.y;
            return ret;
        }

        protected void updateProgress()
        {
            switch (m_eType)
            {
                case CCProgressTimerType.kCCProgressTimerTypeRadialCW:
                case CCProgressTimerType.kCCProgressTimerTypeRadialCCW:
                    updateRadial();
                    break;
                case CCProgressTimerType.kCCProgressTimerTypeHorizontalBarLR:
                case CCProgressTimerType.kCCProgressTimerTypeHorizontalBarRL:
                case CCProgressTimerType.kCCProgressTimerTypeVerticalBarBT:
                case CCProgressTimerType.kCCProgressTimerTypeVerticalBarTB:
                    updateBar();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Update does the work of mapping the texture onto the triangles for the bar
        //	It now doesn't occur the cost of free/alloc data every update cycle.
        //	It also only changes the percentage point but no other points if they have not
        //	been modified.
        //	
        //	It now deals with flipped texture. If you run into this problem, just use the
        //	sprite property and enable the methods flipX, flipY.
        /// </summary>
        protected void updateBar()
        {
            float alpha = m_fPercentage / 100.0f;

            float fXMax = Math.Max(m_pSprite.quad.br.texCoords.u, m_pSprite.quad.bl.texCoords.u);
            float fXMin = Math.Min(m_pSprite.quad.br.texCoords.u, m_pSprite.quad.bl.texCoords.u);
            float fYMax = Math.Max(m_pSprite.quad.tl.texCoords.v, m_pSprite.quad.bl.texCoords.v);
            float fYMin = Math.Min(m_pSprite.quad.tl.texCoords.v, m_pSprite.quad.bl.texCoords.v);
            CCPoint tMax = new CCPoint(fXMax, fYMax);
            CCPoint tMin = new CCPoint(fXMin, fYMin);

            int[] vIndexes = new int[2] { 0, 0 };
            int index = 0;

            //	We know vertex data is always equal to the 4 corners
            //	If we don't have vertex data then we create it here and populate
            //	the side of the bar vertices that won't ever change.
            if (m_pVertexData == null)
            {
                m_nVertexDataCount = kProgressTextureCoordsCount;
                m_pVertexData = new ccV2F_C4B_T2F[m_nVertexDataCount];
                for (int i = 0; i < m_nVertexDataCount; i++)
                {
                    m_pVertexData[i] = new ccV2F_C4B_T2F();
                }


                Debug.Assert(m_pVertexData != null);

                if (m_eType == CCProgressTimerType.kCCProgressTimerTypeHorizontalBarLR)
                {
                    m_pVertexData[vIndexes[0] = 0].texCoords = new ccTex2F(tMin.x, tMin.y);
                    m_pVertexData[vIndexes[1] = 1].texCoords = new ccTex2F(tMin.x, tMax.y);
                }
                else
                    if (m_eType == CCProgressTimerType.kCCProgressTimerTypeHorizontalBarRL)
                    {
                        m_pVertexData[vIndexes[0] = 2].texCoords = new ccTex2F(tMax.x, tMax.y);
                        m_pVertexData[vIndexes[1] = 3].texCoords = new ccTex2F(tMax.x, tMin.y);
                    }
                    else
                        if (m_eType == CCProgressTimerType.kCCProgressTimerTypeVerticalBarBT)
                        {
                            m_pVertexData[vIndexes[0] = 1].texCoords = new ccTex2F(tMin.x, tMax.y);
                            m_pVertexData[vIndexes[1] = 3].texCoords = new ccTex2F(tMax.x, tMax.y);
                        }
                        else
                            if (m_eType == CCProgressTimerType.kCCProgressTimerTypeVerticalBarTB)
                            {
                                m_pVertexData[vIndexes[0] = 0].texCoords = new ccTex2F(tMin.x, tMin.y);
                                m_pVertexData[vIndexes[1] = 2].texCoords = new ccTex2F(tMax.x, tMin.y);
                            }

                index = vIndexes[0];
                m_pVertexData[index].vertices = vertexFromTexCoord(new CCPoint(m_pVertexData[index].texCoords.u,
                                                                       m_pVertexData[index].texCoords.v));

                index = vIndexes[1];
                m_pVertexData[index].vertices = vertexFromTexCoord(new CCPoint(m_pVertexData[index].texCoords.u,
                                                                       m_pVertexData[index].texCoords.v));

                if (m_pSprite.IsFlipY || m_pSprite.IsFlipX)
                {
                    if (m_pSprite.IsFlipX)
                    {
                        index = vIndexes[0];
                        m_pVertexData[index].texCoords.u = tMin.x + tMax.x - m_pVertexData[index].texCoords.u;
                        index = vIndexes[1];
                        m_pVertexData[index].texCoords.u = tMin.x + tMax.x - m_pVertexData[index].texCoords.u;
                    }

                    if (m_pSprite.IsFlipY)
                    {
                        index = vIndexes[0];
                        m_pVertexData[index].texCoords.v = tMin.y + tMax.y - m_pVertexData[index].texCoords.v;
                        index = vIndexes[1];
                        m_pVertexData[index].texCoords.v = tMin.y + tMax.y - m_pVertexData[index].texCoords.v;
                    }
                }

                updateColor();
            }

            if (m_eType == CCProgressTimerType.kCCProgressTimerTypeHorizontalBarLR)
            {
                m_pVertexData[vIndexes[0] = 3].texCoords = new ccTex2F(tMin.x + (tMax.x - tMin.x) * alpha, tMax.y);
                m_pVertexData[vIndexes[1] = 2].texCoords = new ccTex2F(tMin.x + (tMax.x - tMin.x) * alpha, tMin.y);
            }
            else
                if (m_eType == CCProgressTimerType.kCCProgressTimerTypeHorizontalBarRL)
                {
                    m_pVertexData[vIndexes[0] = 1].texCoords = new ccTex2F(tMin.x + (tMax.x - tMin.x) * (1.0f - alpha), tMin.y);
                    m_pVertexData[vIndexes[1] = 0].texCoords = new ccTex2F(tMin.x + (tMax.x - tMin.x) * (1.0f - alpha), tMax.y);
                }
                else
                    if (m_eType == CCProgressTimerType.kCCProgressTimerTypeVerticalBarBT)
                    {
                        m_pVertexData[vIndexes[0] = 0].texCoords = new ccTex2F(tMin.x, tMin.y + (tMax.y - tMin.y) * (1.0f - alpha));
                        m_pVertexData[vIndexes[1] = 2].texCoords = new ccTex2F(tMax.x, tMin.y + (tMax.y - tMin.y) * (1.0f - alpha));
                    }
                    else
                        if (m_eType == CCProgressTimerType.kCCProgressTimerTypeVerticalBarTB)
                        {
                            m_pVertexData[vIndexes[0] = 1].texCoords = new ccTex2F(tMin.x, tMin.y + (tMax.y - tMin.y) * alpha);
                            m_pVertexData[vIndexes[1] = 3].texCoords = new ccTex2F(tMax.x, tMin.y + (tMax.y - tMin.y) * alpha);
                        }

            index = vIndexes[0];
            m_pVertexData[index].vertices = vertexFromTexCoord(new CCPoint(m_pVertexData[index].texCoords.u,
                                                                   m_pVertexData[index].texCoords.v));
            index = vIndexes[1];
            m_pVertexData[index].vertices = vertexFromTexCoord(new CCPoint(m_pVertexData[index].texCoords.u,
                                                                   m_pVertexData[index].texCoords.v));

            if (m_pSprite.IsFlipY || m_pSprite.IsFlipX)
            {
                if (m_pSprite.IsFlipX)
                {
                    index = vIndexes[0];
                    m_pVertexData[index].texCoords.u = tMin.x + tMax.x - m_pVertexData[index].texCoords.u;
                    index = vIndexes[1];
                    m_pVertexData[index].texCoords.u = tMin.x + tMax.x - m_pVertexData[index].texCoords.u;
                }

                if (m_pSprite.IsFlipY)
                {
                    index = vIndexes[0];
                    m_pVertexData[index].texCoords.v = tMin.y + tMax.y - m_pVertexData[index].texCoords.v;
                    index = vIndexes[1];
                    m_pVertexData[index].texCoords.v = tMin.y + tMax.y - m_pVertexData[index].texCoords.v;
                }
            }
        }

        /// <summary>
        /// Update does the work of mapping the texture onto the triangles
        //	It now doesn't occur the cost of free/alloc data every update cycle.
        //	It also only changes the percentage point but no other points if they have not
        //	been modified.
        //	
        //	It now deals with flipped texture. If you run into this problem, just use the
        //	sprite property and enable the methods flipX, flipY.
        /// </summary>
        protected void updateRadial()
        {
            //	Texture Max is the actual max coordinates to deal with non-power of 2 textures
            float fXMax = Math.Max(m_pSprite.quad.br.texCoords.u, m_pSprite.quad.bl.texCoords.u);
            float fXMin = Math.Min(m_pSprite.quad.br.texCoords.u, m_pSprite.quad.bl.texCoords.u);
            float fYMax = Math.Max(m_pSprite.quad.tl.texCoords.v, m_pSprite.quad.bl.texCoords.v);
            float fYMin = Math.Min(m_pSprite.quad.tl.texCoords.v, m_pSprite.quad.bl.texCoords.v);
            CCPoint tMax = new CCPoint(fXMax, fYMax);
            CCPoint tMin = new CCPoint(fXMin, fYMin);

            //	Grab the midpoint
            CCPoint midpoint = CCPointExtension.ccpAdd(tMin, CCPointExtension.ccpCompMult(m_tAnchorPoint, CCPointExtension.ccpSub(tMax, tMin)));

            float alpha = m_fPercentage / 100.0f;

            //	Otherwise we can get the angle from the alpha
            float angle = 2.0f * ((float)Math.PI) * (m_eType == CCProgressTimerType.kCCProgressTimerTypeRadialCW ? alpha : 1.0f - alpha);

            //	We find the vector to do a hit detection based on the percentage
            //	We know the first vector is the one @ 12 o'clock (top,mid) so we rotate 
            //	from that by the progress angle around the midpoint pivot
            CCPoint topMid = new CCPoint(midpoint.x, tMin.y);
            CCPoint percentagePt = CCPointExtension.ccpRotateByAngle(topMid, midpoint, angle);

            int index = 0;
            CCPoint hit = new CCPoint();

            if (alpha == 0.0f)
            {
                //	More efficient since we don't always need to check intersection
                //	If the alpha is zero then the hit point is top mid and the index is 0.
                hit = topMid;
                index = 0;
            }
            else
                if (alpha == 1.0f)
                {
                    //	More efficient since we don't always need to check intersection
                    //	If the alpha is one then the hit point is top mid and the index is 4.
                    hit = topMid;
                    index = 4;
                }
                else
                {
                    //	We run a for loop checking the edges of the texture to find the
                    //	intersection point
                    //	We loop through five points since the top is split in half

                    float min_t = float.MaxValue;

                    for (int i = 0; i <= kProgressTextureCoordsCount; ++i)
                    {
                        int pIndex = (i + (kProgressTextureCoordsCount - 1)) % kProgressTextureCoordsCount;

                        CCPoint edgePtA = CCPointExtension.ccpAdd(tMin, CCPointExtension.ccpCompMult(boundaryTexCoord(i % kProgressTextureCoordsCount), CCPointExtension.ccpSub(tMax, tMin)));
                        CCPoint edgePtB = CCPointExtension.ccpAdd(tMin, CCPointExtension.ccpCompMult(boundaryTexCoord(pIndex), CCPointExtension.ccpSub(tMax, tMin)));

                        //	Remember that the top edge is split in half for the 12 o'clock position
                        //	Let's deal with that here by finding the correct endpoints
                        if (i == 0)
                        {
                            edgePtB = CCPointExtension.ccpLerp(edgePtA, edgePtB, 0.5f);
                        }
                        else
                            if (i == 4)
                            {
                                edgePtA = CCPointExtension.ccpLerp(edgePtA, edgePtB, 0.5f);
                            }

                        //	s and t are returned by ccpLineIntersect
                        float s = 0;
                        float t = 0;
                        if (CCPointExtension.ccpLineIntersect(edgePtA, edgePtB, midpoint, percentagePt, ref s, ref t))
                        {
                            //	Since our hit test is on rays we have to deal with the top edge
                            //	being in split in half so we have to test as a segment
                            if (i == 0 || i == 4)
                            {
                                //	s represents the point between edgePtA--edgePtB
                                if (!(0.0f <= s && s <= 1.0f))
                                {
                                    continue;
                                }
                            }

                            //	As long as our t isn't negative we are at least finding a 
                            //	correct hitpoint from midpoint to percentagePt.
                            if (t >= 0.0f)
                            {
                                //	Because the percentage line and all the texture edges are
                                //	rays we should only account for the shortest intersection
                                if (t < min_t)
                                {
                                    min_t = t;
                                    index = i;
                                }
                            }
                        }

                    }

                    //	Now that we have the minimum magnitude we can use that to find our intersection
                    hit = CCPointExtension.ccpAdd(midpoint, CCPointExtension.ccpMult(CCPointExtension.ccpSub(percentagePt, midpoint), min_t));
                }

            //	The size of the vertex data is the index from the hitpoint
            //	the 3 is for the midpoint, 12 o'clock point and hitpoint position.

            bool sameIndexCount = true;
            if (m_nVertexDataCount != index + 3)
            {
                sameIndexCount = false;
                if (m_pVertexData != null)
                {
                    m_pVertexData = null;
                    m_nVertexDataCount = 0;
                }
            }

            if (m_pVertexData == null)
            {
                m_nVertexDataCount = index + 3;
                m_pVertexData = new ccV2F_C4B_T2F[m_nVertexDataCount];
                for (int i = 0; i < m_nVertexDataCount; i++)
                {
                    m_pVertexData[i] = new ccV2F_C4B_T2F();
                }


                Debug.Assert(m_pVertexData != null);

                updateColor();
            }

            if (!sameIndexCount)
            {
                //	First we populate the array with the midpoint, then all 
                //	vertices/texcoords/colors of the 12 'o clock start and edges and the hitpoint
                m_pVertexData[0].texCoords = new ccTex2F(midpoint.x, midpoint.y);
                m_pVertexData[0].vertices = vertexFromTexCoord(midpoint);

                m_pVertexData[1].texCoords = new ccTex2F(midpoint.x, tMin.y);
                m_pVertexData[1].vertices = vertexFromTexCoord(new CCPoint(midpoint.x, tMin.y));

                for (int i = 0; i < index; ++i)
                {
                    CCPoint texCoords = CCPointExtension.ccpAdd(tMin, CCPointExtension.ccpCompMult(boundaryTexCoord(i), CCPointExtension.ccpSub(tMax, tMin)));

                    m_pVertexData[i + 2].texCoords = new ccTex2F(texCoords.x, texCoords.y);
                    m_pVertexData[i + 2].vertices = vertexFromTexCoord(texCoords);
                }

                //	Flip the texture coordinates if set
                if (m_pSprite.IsFlipX || m_pSprite.IsFlipY)
                {
                    for (int i = 0; i < m_nVertexDataCount - 1; ++i)
                    {
                        if (m_pSprite.IsFlipX)
                        {
                            m_pVertexData[i].texCoords.u = tMin.x + tMax.x - m_pVertexData[i].texCoords.u;
                        }

                        if (m_pSprite.IsFlipY)
                        {
                            m_pVertexData[i].texCoords.v = tMin.y + tMax.y - m_pVertexData[i].texCoords.v;
                        }
                    }
                }
            }

            //	hitpoint will go last
            m_pVertexData[m_nVertexDataCount - 1].texCoords = new ccTex2F(hit.x, hit.y);
            m_pVertexData[m_nVertexDataCount - 1].vertices = vertexFromTexCoord(hit);

            if (m_pSprite.IsFlipX || m_pSprite.IsFlipY)
            {
                if (m_pSprite.IsFlipX)
                {
                    m_pVertexData[m_nVertexDataCount - 1].texCoords.u = tMin.x + tMax.x - m_pVertexData[m_nVertexDataCount - 1].texCoords.u;
                }

                if (m_pSprite.IsFlipY)
                {
                    m_pVertexData[m_nVertexDataCount - 1].texCoords.v = tMin.y + tMax.y - m_pVertexData[m_nVertexDataCount - 1].texCoords.v;
                }
            }
        }

        protected void updateColor()
        {
            byte op = m_pSprite.Opacity;
            ccColor3B c3b = m_pSprite.Color;

            ccColor4B color = new ccColor4B { r = c3b.r, g = c3b.g, b = c3b.b, a = op };
            if (m_pSprite.Texture.HasPremultipliedAlpha)
            {
                color.r *= (byte)(op / 255);
                color.g *= (byte)(op / 255);
                color.b *= (byte)(op / 255);
            }

            if (m_pVertexData != null)
            {
                for (int i = 0; i < m_nVertexDataCount; ++i)
                {
                    m_pVertexData[i].colors = color;
                }
            }
        }

        protected CCPoint boundaryTexCoord(int index)
        {
            if (index < kProgressTextureCoordsCount)
            {
                switch (m_eType)
                {
                    case CCProgressTimerType.kCCProgressTimerTypeRadialCW:
                        return new CCPoint((float)((kProgressTextureCoords >> ((index << 1) + 1)) & 1), (float)((kProgressTextureCoords >> (index << 1)) & 1));
                    case CCProgressTimerType.kCCProgressTimerTypeRadialCCW:
                        return new CCPoint((float)((kProgressTextureCoords >> (7 - (index << 1))) & 1), (float)((kProgressTextureCoords >> (7 - ((index << 1) + 1))) & 1));
                    default:
                        break;
                }
            }

            return new CCPoint(0, 0);
        }

        protected CCProgressTimerType m_eType;
        protected float m_fPercentage;
        protected CCSprite m_pSprite;
        protected int m_nVertexDataCount;
        protected ccV2F_C4B_T2F[] m_pVertexData;
        protected VertexPositionColorTexture[] vertices;
        protected short[] indexes;
    }

    /// <summary>
    /// Types of progress
    /// @since v0.99.1
    /// </summary>
    public enum CCProgressTimerType
    {
        /// Radial Counter-Clockwise 
        kCCProgressTimerTypeRadialCCW,
        /// Radial ClockWise
        kCCProgressTimerTypeRadialCW,
        /// Horizontal Left-Right
        kCCProgressTimerTypeHorizontalBarLR,
        /// Horizontal Right-Left
        kCCProgressTimerTypeHorizontalBarRL,
        /// Vertical Bottom-top
        kCCProgressTimerTypeVerticalBarBT,
        /// Vertical Top-Bottom
        kCCProgressTimerTypeVerticalBarTB,
    }
}
