using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCMotionStreak : CCNode, ICCTextureProtocol, ICCRGBAProtocol
    {
        public bool isFastMode() { return m_bFastMode; }
        public void setFastMode(bool bFastMode) { m_bFastMode = bFastMode; }

        public bool isStartingPositionInitialized() { return m_bStartingPositionInitialized; }
        public void setStartingPositionInitialized(bool bStartingPositionInitialized)
        {
            m_bStartingPositionInitialized = bStartingPositionInitialized;
        }
        protected bool m_bFastMode;
        protected bool m_bStartingPositionInitialized;
        /** texture used for the motion streak */
        private CCTexture2D m_pTexture;
        private ccBlendFunc m_tBlendFunc;
        private CCPoint m_tPositionR;
        private ccColor3B m_tColor;

        private float m_fStroke;
        private float m_fFadeDelta;
        private float m_fMinSeg;

        private int m_uMaxPoints;
        private int m_uNuPoints;
        private int m_uPreviousNuPoints;

        /** Pointers */
        private CCPoint[] m_pPointVertexes;
        private float[] m_pPointState;

        // Opengl
        private ccVertex2F[] m_pVertices;
        private ccColor4B[] m_pColor;
        private ccTex2F[] m_pTexCoords;

        public CCMotionStreak()
        {
            m_tBlendFunc = new ccBlendFunc();
            m_tBlendFunc.src = 0;// CC_BLEND_SRC;
            m_tBlendFunc.dst = 0x0303;// CC_BLEND_DST;
            m_tOpacity = 0xff; // Fully opaque
        }

        public static CCMotionStreak streakWithFade(float fade, float minSeg, float stroke, ccColor3B color, string path)
        {
            return create(fade, minSeg, stroke, color, path);
        }

        public static CCMotionStreak create(float fade, float minSeg, float stroke, ccColor3B color, string path)
        {
            CCMotionStreak pRet = new CCMotionStreak();
            if (pRet.initWithFade(fade, minSeg, stroke, color, path))
            {
                return pRet;
            }
            return null;
        }

        public static CCMotionStreak streakWithFade(float fade, float minSeg, float stroke, ccColor3B color, CCTexture2D texture)
        {
            return CCMotionStreak.create(fade, minSeg, stroke, color, texture);
        }

        public static CCMotionStreak create(float fade, float minSeg, float stroke, ccColor3B color, CCTexture2D texture)
        {
            CCMotionStreak pRet = new CCMotionStreak();
            if (pRet.initWithFade(fade, minSeg, stroke, color, texture))
            {
                return pRet;
            }
            return null;
        }

        public bool initWithFade(float fade, float minSeg, float stroke, ccColor3B color, string path)
        {
            if (path == null)
            {
                throw (new ArgumentNullException(path, "Path is required to create the texture2d."));
            }
            CCTexture2D texture = CCTextureCache.sharedTextureCache().addImage(path);
            return initWithFade(fade, minSeg, stroke, color, texture);
        }

        public bool initWithFade(float fade, float minSeg, float stroke, ccColor3B color, CCTexture2D texture)
        {
            position = CCPoint.CCPointZero;
            anchorPoint = CCPoint.CCPointZero;
            isRelativeAnchorPoint = false;
//            ignoreAnchorPointForPosition(true);
            m_bStartingPositionInitialized = false;

            m_tPositionR = CCPoint.CCPointZero;
            m_bFastMode = true;
            m_fMinSeg = (minSeg == -1.0f) ? stroke / 5.0f : minSeg;
            m_fMinSeg *= m_fMinSeg;

            m_fStroke = stroke;
            m_fFadeDelta = 1.0f / fade;

            m_uMaxPoints = (int)(fade * 60.0f) + 2;
            m_uNuPoints = 0;
            m_pPointState = new float[m_uMaxPoints]; // (float *)malloc(sizeof(float) * m_uMaxPoints);
            m_pPointVertexes = new CCPoint[m_uMaxPoints]; // (CCPoint*)malloc(sizeof(CCPoint) * m_uMaxPoints);

            m_pVertices = new ccVertex2F[m_uMaxPoints * 2]; // (ccVertex2F*)malloc(sizeof(ccVertex2F) * m_uMaxPoints * 2);
            m_pTexCoords = new ccTex2F[m_uMaxPoints * 2];// (ccTex2F*)malloc(sizeof(ccTex2F) * m_uMaxPoints * 2);
            m_pColor = new ccColor4B[m_uMaxPoints * 2];// m_pColorPointer = (GLubyte*)malloc(sizeof(GLubyte) * m_uMaxPoints * 2 * 4);

            // Set blend mode
            m_tBlendFunc.src = OGLES.GL_SRC_ALPHA;
            m_tBlendFunc.dst = OGLES.GL_ONE_MINUS_SRC_ALPHA;

            // shader program
            // TODO: setup the shader program
            // setShaderProgram(CCShaderCache.sharedShaderCache().programForKey(kCCShader_PositionTextureColor));

            Texture = texture;
            Color = color;
//            scheduleUpdate();
            schedule(update);

            return true;
        }

        public override CCPoint position
        {
            set
            {
                m_bStartingPositionInitialized = true;
                base.position = value;
                m_tPositionR = value;
            }
        }

        public void tintWithColor(ccColor3B colors)
        {
            Color = colors;

            for (int i = 0; i < m_uNuPoints * 2; i++)
            {
                m_pColor[i] = new ccColor4B(colors);
                //        *((ccColor3B*) (m_pColorPointer+i*4)) = colors;
            }
        }

        public CCTexture2D Texture
        {
            get
            {
                return (m_pTexture);
            }
            set
            {
                if (m_pTexture != value)
                {
                    m_pTexture = value;
                }
            }
        }

        public ccBlendFunc BlendFunc
        {
            set
            {
                m_tBlendFunc = value;
            }
            get
            {
                return (m_tBlendFunc);
            }
        }

        public ccColor3B Color
        {
            set
            {
                m_tColor = value.copy();
            }
            get
            {
                return (m_tColor.copy());
            }
        }

        private byte m_tOpacity = 0;

        public byte Opacity
        {
            get
            {
                return (m_tOpacity);
            }
            set
            {
                m_tOpacity = value;
            }
        }


        public bool IsOpacityModifyRGB
        {
            get
            {
                return (false);
            }
            set
            {
            }
        }

        public override void update(float delta)
        {
            if (!m_bStartingPositionInitialized)
            {
                return;
            }

            if (m_pVerticesPCT == null)
            {
                m_pVerticesPCT = new VertexPositionColorTexture[((m_uNuPoints+1) * 2)];
            }
            if (m_uNuPoints * 2 > m_pVerticesPCT.Length)
            {
                VertexPositionColorTexture[] tmp = new VertexPositionColorTexture[((m_uNuPoints + 1) * 2)];
                m_pVerticesPCT.CopyTo(tmp, 0);
                m_pVerticesPCT = tmp;
            }
            delta *= m_fFadeDelta;

            int newIdx, newIdx2, i, i2;
            int mov = 0;

            // Update current points
            for (i = 0; i < m_uNuPoints; i++)
            {
                if (m_pVerticesPCT[i] == null)
                {
                    m_pVerticesPCT[i] = new VertexPositionColorTexture(m_pVertices[i].ToVector3(), m_pColor[i].XNAColor, m_pTexCoords[i].ToVector2());
                }
                if (m_pVerticesPCT[i*2] == null)
                {
                    m_pVerticesPCT[i*2] = new VertexPositionColorTexture(m_pVerticesPCT[i].Position, m_pVerticesPCT[i].Color, m_pVerticesPCT[i].TextureCoordinate);
                }
                m_pPointState[i] -= delta;

                if (m_pPointState[i] <= 0)
                    mov++;
                else
                {
                    newIdx = i - mov;

                    if (mov > 0)
                    {
                        // Move data
                        m_pPointState[newIdx] = m_pPointState[i];

                        // Move point
                        m_pPointVertexes[newIdx] = new CCPoint(m_pPointVertexes[i]);

                        // Move vertices
                        i2 = i*2;
                        newIdx2 = newIdx*2;
                        m_pVertices[newIdx2] = new ccVertex2F(m_pVertices[i2]);
                        m_pVertices[newIdx2 + 1] = new ccVertex2F(m_pVertices[i2 + 1]);
                        // Move color
                        m_pColor[newIdx2] = new ccColor4B(m_pColor[i2]);
                        m_pColor[newIdx2 + 1] = new ccColor4B(m_pColor[i2 + 1]);
                        // Move the GL vertex data
                        m_pVerticesPCT[newIdx2] = m_pVerticesPCT[i2];
                        m_pVerticesPCT[newIdx2 + 1] = m_pVerticesPCT[i2 + 1];
                    }
                    else
                    {
                        newIdx2 = newIdx * 2;
                    }
                    byte op = (byte)(m_pPointState[newIdx] * 255.0f);
                    m_pColor[newIdx2].a = op;
                    m_pColor[newIdx2 + 1].a = op;
                    VertexPositionColorTexture vpc;
                    if (m_pVertices[newIdx2] != null)
                    {
                        vpc = new VertexPositionColorTexture(m_pVertices[newIdx2].ToVector3(), m_pColor[newIdx2].XNAColor, m_pTexCoords[newIdx2].ToVector2());
                        m_pVerticesPCT[newIdx2] = vpc;
                    }
                    if (m_pVertices[newIdx2 + 1] != null)
                    {
                        vpc = new VertexPositionColorTexture(m_pVertices[newIdx2 + 1].ToVector3(), m_pColor[newIdx2 + 1].XNAColor, m_pTexCoords[newIdx2 + 1].ToVector2());
                        m_pVerticesPCT[newIdx2 + 1] = vpc;
                    }
                }
            }
            m_uNuPoints -= mov;

            // Append new point
            bool appendNewPoint = true;
            if (m_uNuPoints >= m_uMaxPoints)
            {
                appendNewPoint = false;
            }

            else if (m_uNuPoints > 0)
            {
                bool a1 = (m_pPointVertexes[m_uNuPoints - 1].DistanceSQ(m_tPositionR) < m_fMinSeg);
                bool a2 = (m_uNuPoints == 1) ? false : (m_pPointVertexes[m_uNuPoints - 2].DistanceSQ(m_tPositionR) < (m_fMinSeg * 2.0f));
                if (a1 || a2)
                {
                    appendNewPoint = false;
                }
            }

            if (appendNewPoint)
            {
                m_pPointVertexes[m_uNuPoints] = new CCPoint(m_tPositionR);
                m_pPointState[m_uNuPoints] = 1.0f;

                // Color asignment
                int offset = m_uNuPoints * 2;
                m_pColor[offset] = new ccColor4B(m_tColor);
                m_pColor[offset + 1] = new ccColor4B(m_tColor);

                // Opacity
                m_pColor[offset].a = 255;
                m_pColor[offset + 1].a = 255;

                // Generate polygon
                if (m_uNuPoints > 0 && m_bFastMode)
                {
                    if (m_uNuPoints > 1)
                    {
                        CCVertex.LineToPolygon(m_pPointVertexes, m_fStroke, m_pVertices, m_uNuPoints, 1);
                    }
                    else
                    {
                        CCVertex.LineToPolygon(m_pPointVertexes, m_fStroke, m_pVertices, 0, 2);
                    }
                }

                m_uNuPoints++;
            }

            if (!m_bFastMode)
            {
                CCVertex.LineToPolygon(m_pPointVertexes, m_fStroke, m_pVertices, 0, m_uNuPoints);
            }

            // Updated Tex Coords only if they are different than previous step
            if (m_uPreviousNuPoints != m_uNuPoints)
            {
                if (m_uNuPoints > m_uPreviousNuPoints)
                {
                    int count = (m_uNuPoints + 1) * 2;
                    if (count < m_pVerticesPCT.Length)
                    {
                        count = m_pVerticesPCT.Length;
                    }
                    VertexPositionColorTexture[] tmp = new VertexPositionColorTexture[count];
                    m_pVerticesPCT.CopyTo(tmp, 0);
                    m_pVerticesPCT = tmp;
                }
                float texDelta = 1.0f / m_uNuPoints;
                for (i = 0; i < m_uNuPoints; i++)
                {
                    m_pTexCoords[i * 2] = new ccTex2F(0, texDelta * i);
                    m_pTexCoords[i * 2 + 1] = new ccTex2F(1, texDelta * i);
                    VertexPositionColorTexture vpc;
                    if (m_pVertices[i * 2] != null)
                    {
                        vpc = new VertexPositionColorTexture(m_pVertices[i * 2].ToVector3(), m_pColor[i * 2].XNAColor, m_pTexCoords[i * 2].ToVector2());
                        m_pVerticesPCT[i * 2] = vpc;
                    }
                    if (m_pVertices[i * 2 + 1] != null)
                    {
                        vpc = new VertexPositionColorTexture(m_pVertices[i * 2 + 1].ToVector3(), m_pColor[i * 2 + 1].XNAColor, m_pTexCoords[i * 2 + 1].ToVector2());
                        m_pVerticesPCT[i * 2 + 1] = vpc;
                    }
                }

                m_uPreviousNuPoints = m_uNuPoints;
            }
        }

        private VertexPositionColorTexture[] m_pVerticesPCT;

        private void reset()
        {
            m_uNuPoints = 0;
        }

        public override void draw()
        {
            base.draw();
            if (m_uNuPoints <= 1)
                return;

            CCApplication app = CCApplication.sharedApplication();
            float startAlpha = app.basicEffect.Alpha;
            CCSize size = CCDirector.sharedDirector().getWinSize();

            app.basicEffect.VertexColorEnabled = true;
            app.basicEffect.TextureEnabled = true;
            app.basicEffect.Alpha = (float)Opacity / 255.0f;
            app.basicEffect.Texture = Texture.Texture;
            /*
                ccGLEnableVertexAttribs(kCCVertexAttribFlag_PosColorTex );
                ccGLBlendFunc( m_tBlendFunc.src, m_tBlendFunc.dst );

                ccGLBindTexture2D( m_pTexture->getName() );

                glVertexAttribPointer(kCCVertexAttrib_Position, 2, GL_FLOAT, GL_FALSE, 0, m_pVertices);
                glVertexAttribPointer(kCCVertexAttrib_TexCoords, 2, GL_FLOAT, GL_FALSE, 0, m_pTexCoords);
                glVertexAttribPointer(kCCVertexAttrib_Color, 4, GL_UNSIGNED_BYTE, GL_TRUE, 0, m_pColorPointer);

                glDrawArrays(GL_TRIANGLE_STRIP, 0, (GLsizei)m_uNuPoints*2);
            */

            // SEE CCDrawingPrimitives.cs line 275


            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                app.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleStrip, m_pVerticesPCT, 0, m_uNuPoints*2);
            }
            app.basicEffect.Alpha = startAlpha;
        }
    }
}
