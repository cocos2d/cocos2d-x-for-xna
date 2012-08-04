/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
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

namespace cocos2d
{
    public class CCTransitionCrossFade : CCTransitionScene
    {
        const int kSceneFade = int.MaxValue;

        public override void draw()
        {
            // override draw since both scenes (textures) are rendered in 1 scene
        }

        public override void onEnter()
        {
            base.onEnter();

            // create a transparent color layer
            // in which we are going to add our rendertextures
            ccColor4B color = new ccColor4B(0, 0, 0, 0);
            CCSize size = CCDirector.sharedDirector().getWinSize();
            //CCLayerColor layer = CCLayerColor.layerWithColor(color);

            CCLayer layer = new CCLayer();

            // create the first render texture for inScene
            CCRenderTexture inTexture = CCRenderTexture.renderTextureWithWidthAndHeight((int)size.width, (int)size.height);

            if (null == inTexture)
            {
                return;
            }

            inTexture.Sprite.anchorPoint = new CCPoint(0.5f, 0.5f);
            inTexture.position = new CCPoint(size.width / 2, size.height / 2);
            inTexture.anchorPoint = new CCPoint(0.5f, 0.5f);

            //  render inScene to its texturebuffer
            inTexture.begin();
            m_pInScene.visit();
            inTexture.end();

            // create the second render texture for outScene
            CCRenderTexture outTexture = CCRenderTexture.renderTextureWithWidthAndHeight((int)size.width, (int)size.height);
            outTexture.Sprite.anchorPoint = new CCPoint(0.5f, 0.5f);
            outTexture.position = new CCPoint(size.width / 2, size.height / 2);
            outTexture.anchorPoint = new CCPoint(0.5f, 0.5f);

            //  render outScene to its texturebuffer
            outTexture.begin();
            m_pOutScene.visit();
            outTexture.end();

            // create blend functions

            ccBlendFunc blend1 = new ccBlendFunc(OGLES.GL_ONE, OGLES.GL_ONE); // inScene will lay on background and will not be used with alpha
            ccBlendFunc blend2 = new ccBlendFunc(OGLES.GL_SRC_ALPHA, OGLES.GL_ONE_MINUS_SRC_ALPHA); // we are going to blend outScene via alpha 

            // set blendfunctions
            inTexture.Sprite.BlendFunc = blend1;
            outTexture.Sprite.BlendFunc = blend2;

            // add render textures to the layer
            layer.addChild(inTexture);
            layer.addChild(outTexture);

            // initial opacity:
            inTexture.Sprite.Opacity = 255;
            outTexture.Sprite.Opacity = 255;

            // create the blend action
            CCAction layerAction = CCSequence.actions
            (
                CCFadeTo.actionWithDuration(m_fDuration, 0),
                CCCallFunc.actionWithTarget(this, (base.hideOutShowIn)),
                CCCallFunc.actionWithTarget(this, (base.finish))
            );


            //// run the blend action
            outTexture.Sprite.runAction(layerAction);

            // add the layer (which contains our two rendertextures) to the scene
            addChild(layer, 2, kSceneFade);
        }

        public override void onExit()
        {
            // remove our layer and release all containing objects 
            this.removeChildByTag(kSceneFade, false);
            base.onExit();
        }

        public static new CCTransitionCrossFade transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionCrossFade pScene = new CCTransitionCrossFade();
            if (pScene.initWithDuration(t, scene))
            {
                return pScene;
            }

            return null;
        }
    }
}
