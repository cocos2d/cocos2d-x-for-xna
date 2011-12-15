using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Atlas1 : AtlasDemo
    {
        CCTextureAtlas m_textureAtlas;

        public Atlas1()
        {
            m_textureAtlas = CCTextureAtlas.textureAtlasWithFile(TestResource.s_AtlasTest, 3);
            //m_textureAtlas.retain();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            //
            // Notice: u,v tex coordinates are inverted
            //
            //ccV3F_C4B_T2F_Quad quads[] = 
            //{
            //    {
            //        {{0,0,0},new ccColor4B(0,0,255,255),{0.0f,1.0f},},				// bottom left
            //        {{s.width,0,0},new ccColor4B(0,0,255,0),{1.0f,1.0f},},			// bottom right
            //        {{0,s.height,0},new ccColor4B(0,0,255,0),{0.0f,0.0f},},	    // top left
            //        {{s.width,s.height,0},{0,0,255,255},{1.0f,0.0f},},	                // top right
            //    },		
            //    {
            //        {{40,40,0}, new ccColor4B(255,255,255,255),{0.0f,0.2f},},			// bottom left
            //        {{120,80,0},new ccColor4B(255,0,0,255),{0.5f,0.2f},},			        // bottom right
            //        {{40,160,0},new ccColor4B(255,255,255,255),{0.0f,0.0f},},		    // top left
            //        {{160,160,0},new ccColor4B(0,255,0,255),{0.5f,0.0f},},			    // top right
            //    },

            //    {
            //        {{s.width/2,40,0},new ccColor4B(255,0,0,255),{0.0f,1.0f},},		         // bottom left
            //        {{s.width,40,0},new ccColor4B(0,255,0,255),{1.0f,1.0f},},		        // bottom right
            //        {{s.width/2-50,200,0},new ccColor4B(0,0,255,255),{0.0f,0.0f},},		// top left
            //        {{s.width,100,0},new ccColor4B(255,255,0,255),{1.0f,0.0f},},		    // top right
            //    },		
            //};

            //for( int i=0;i<3;i++) 
            //{
            //    m_textureAtlas.updateQuad(&quads[i], i);
            //}
        }

        public override string title()
        {
            return "CCTextureAtlas";
        }

        public override string subtitle()
        {
            return "Manual creation of CCTextureAtlas";
        }

        public override void draw()
        {
            // GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            // GL_TEXTURE_2D

            m_textureAtlas.drawQuads();

            //	[textureAtlas drawNumberOfQuads:3];
        }
    }
}
