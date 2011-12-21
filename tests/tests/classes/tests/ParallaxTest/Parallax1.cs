using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Parallax1 : ParallaxDemo
    {
        string s_Power = "Images/powered.png";
        string s_TilesPng = "TileMaps/tiles.png";
        string s_LevelMapTga = "TileMaps/levelmap.tga";

        protected CCNode m_root;
        protected CCNode m_target;
        protected CCMotionStreak m_streak;


        public Parallax1()
        {
            // Top Layer, a simple image
            CCSprite cocosImage = CCSprite.spriteWithFile(s_Power);
            // scale the image (optional)
            cocosImage.scale = 2.5f;
            // change the transform anchor point to 0,0 (optional)
            cocosImage.anchorPoint = new CCPoint(0, 0);


            // Middle layer: a Tile map atlas
            //CCTileMapAtlas tilemap = CCTileMapAtlas.tileMapAtlasWithTileFile(s_TilesPng, s_LevelMapTga, 16, 16);
            //tilemap.releaseMap();

            // change the transform anchor to 0,0 (optional)
            //tilemap.anchorPoint( new CCPoint(0, 0) );

            //// Anti Aliased images
            //tilemap.Texture.AntiAliasTexParameters();


            // background layer: another image
            //CCSprite background = CCSprite.spriteWithFile(s_back);
            //// scale the image (optional)
            //background->setScale( 1.5f );G:\cocos2d-xna\xna\cocos2d-xna\CCDisplayLinkDirector.cs
            //// change the transform anchor point (optional)
            //background->setAnchorPoint( ccp(0,0) );


            //// create a void node, a parent node
            //CCParallaxNode* voidNode = CCParallaxNode::node();

            //// NOW add the 3 layers to the 'void' node

            //// background image is moved at a ratio of 0.4x, 0.5y
            //voidNode->addChild(background, -1, ccp(0.4f,0.5f), CCPointZero);

            //// tiles are moved at a ratio of 2.2x, 1.0y
            //voidNode->addChild(tilemap, 1, ccp(2.2f,1.0f), ccp(0,-200) );

            //// top image is moved at a ratio of 3.0x, 2.5y
            //voidNode->addChild(cocosImage, 2, ccp(3.0f,2.5f), ccp(200,800) );


            //// now create some actions that will move the 'void' node
            //// and the children of the 'void' node will move at different
            //// speed, thus, simulation the 3D environment
            //CCActionInterval* goUp = CCMoveBy::actionWithDuration(4, ccp(0,-500) );
            //CCActionInterval* goDown = goUp->reverse();
            //CCActionInterval* go = CCMoveBy::actionWithDuration(8, ccp(-1000,0) );
            //CCActionInterval* goBack = go->reverse();
            //CCFiniteTimeAction* seq = CCSequence::actions(goUp, go, goDown, goBack, NULL);
            //voidNode->runAction( (CCRepeatForever::actionWithAction((CCActionInterval*) seq) ));

            //addChild( voidNode );
        }
        public virtual string title()
        {
            return  "";
        }
    }
}
