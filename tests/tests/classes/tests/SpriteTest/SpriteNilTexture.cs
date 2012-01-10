using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Microsoft.Xna.Framework;

namespace tests
{
    public class SpriteNilTexture : SpriteTestDemo
    {
        public SpriteNilTexture()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sprite = null;

            // TEST: If no texture is given, then Opacity + Color should work.

            sprite = new CCSprite();
            sprite.init();
            sprite.setTextureRect(new CCRect(0, 0, 300, 300));
            sprite.Color = new ccColor3B(Color.Red);
            sprite.Opacity = 128;
            sprite.position = (new CCPoint(3 * s.width / 4, s.height / 2));
            addChild(sprite, 100);

            sprite = new CCSprite();
            sprite.init();
            sprite.setTextureRect(new CCRect(0, 0, 300, 300));
            sprite.Color = new ccColor3B(Color.Blue);
            sprite.Opacity = 128 ;
            sprite.position = (new CCPoint(1 * s.width / 4, s.height / 2));
            addChild(sprite, 100);
        }
        public override string title() 
        {
            return "Sprite without texture";
        }
        public override string subtitle() 
        {
            return "opacity and color should work";
        }
    }
}
