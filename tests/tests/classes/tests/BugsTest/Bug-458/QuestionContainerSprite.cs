using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class QuestionContainerSprite : CCSprite
    {
        public override bool init()
        {
            if (base.init())
            {
                //Add label
                CCLabelTTF label = CCLabelTTF.labelWithString("Answer 1", "Arial", 12);
                label.tag = 100;

                //Add the background
                CCSize size = CCDirector.sharedDirector().getWinSize();
                CCSprite corner = CCSprite.spriteWithFile("Images/bugs/corner");

                int width = (int)(size.width * 0.9f - (corner.contentSize.width * 2));
                int height = (int)(size.height * 0.15f - (corner.contentSize.height * 2));
                //CCLayerColor layer = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(r = 255, g = 255, b = 255, a = 255 * 0.75), width, height);
                //layer.position = new CCPoint(-width / 2, -height / 2);

                //First button is blue,
                //Second is red
                //Used for testing - change later
                int a = 0;

                if (a == 0)
                    label.Color = new ccColor3B(0, 0, 255); //ccBLUE
                else
                {
                    Debug.WriteLine("Color changed");
                    label.Color = new ccColor3B(255, 0, 0);
                }
                a++;
                //addChild(layer);

                corner.position = new CCPoint(-(width / 2 + corner.contentSize.width / 2), -(height / 2 + corner.contentSize.height / 2));
                addChild(corner);

                CCSprite corner2 = CCSprite.spriteWithFile("Images/bugs/corner");
                corner2.position = new CCPoint(-corner.position.x, corner.position.y);
                corner2.IsFlipX = true;
                addChild(corner2);

                CCSprite corner3 = CCSprite.spriteWithFile("Images/bugs/corner");
                corner3.position = new CCPoint(corner.position.x, -corner.position.y);
                corner3.IsFlipY = true;
                addChild(corner3);

                CCSprite corner4 = CCSprite.spriteWithFile("Images/bugs/corner");
                corner4.position = new CCPoint(corner2.position.x, -corner2.position.y);
                corner4.IsFlipX = true;
                corner4.IsFlipY = true;
                addChild(corner4);

                CCSprite edge = CCSprite.spriteWithFile("Images/bugs/edge");
                edge.scaleX = width;
                edge.position = new CCPoint(corner.position.x + (corner.contentSize.width / 2) + (width / 2), corner.position.y);
                addChild(edge);

                CCSprite edge2 = CCSprite.spriteWithFile("Images/bugs/edge");
                edge2.scaleX = width;
                edge2.position = new CCPoint(corner.position.x + (corner.contentSize.width / 2) + (width / 2), -corner.position.y);
                edge2.IsFlipX = true;
                addChild(edge2);

                CCSprite edge3 = CCSprite.spriteWithFile("Images/bugs/edge");
                edge3.rotation = 90;
                edge3.scaleX = height;
                edge3.position = new CCPoint(corner.position.x, corner.position.y + (corner.contentSize.height / 2) + (height / 2));
                addChild(edge3);

                CCSprite edge4 = CCSprite.spriteWithFile("Images/bugs/edge");
                edge4.rotation = 270;
                edge4.scaleX = height;
                edge4.position = new CCPoint(-corner.position.x, corner.position.y + (corner.contentSize.height / 2) + (height / 2));
                addChild(edge4);

                addChild(label);
                return true;
            }

            return false;
        }
    }
}
