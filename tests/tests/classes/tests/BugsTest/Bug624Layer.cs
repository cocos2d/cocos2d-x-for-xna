using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class Bug624Layer : BugsTestBaseLayer
    {
        public override bool init()
        {
            if (base.init())
            {
                CCSize size = CCDirector.sharedDirector().getWinSize();
                CCLabelTTF label = CCLabelTTF.labelWithString("Layer1", "Marker Felt", 36);

                label.position = new CCPoint(size.width / 2, size.height / 2);
                addChild(label);
                isAccelerometerEnabled = true;
                schedule(switchLayer, 5.0f);

                return true;
            }

            return false;
        }

        public void switchLayer(float dt)
        {
            //unschedule(Bug624Layer.switchLayer);

            CCScene scene = CCScene.node();
            scene.addChild(Bug624Layer2.node(), 0);
            CCDirector.sharedDirector().replaceScene(CCTransitionFade.transitionWithDuration(2.0f, scene, new ccColor3B { r = 255, g = 255, b = 255 }));
        }

        public virtual void didAccelerate(CCAcceleration pAccelerationValue)
        {
            Debug.WriteLine("Layer1 accel");
        }

        //LAYER_NODE_FUNC(Bug624Layer);
    }

    public class Bug624Layer2 : BugsTestBaseLayer
    {
        public override bool init()
        {
            if (base.init())
            {
                CCSize size = CCDirector.sharedDirector().getWinSize();
                CCLabelTTF label = CCLabelTTF.labelWithString("Layer2", "Marker Felt", 36);

                label.position = new CCPoint(size.width / 2, size.height / 2);
                addChild(label);
                isAccelerometerEnabled = true;
                schedule(switchLayer, 5.0f);

                return true;
            }

            return false;
        }

        public void switchLayer(float dt)
        {
            //unschedule(schedule_selector(Bug624Layer::switchLayer));

            CCScene scene = CCScene.node();
            scene.addChild(Bug624Layer.node(), 0);
            CCDirector.sharedDirector().replaceScene(CCTransitionFade.transitionWithDuration(2.0f, scene, new ccColor3B { r = 255, g = 0, b = 0 }));
        }

        public virtual void didAccelerate(CCAcceleration pAccelerationValue)
        {
            Debug.WriteLine("Layer2 accel");
        }

        //LAYER_NODE_FUNC(Bug624Layer2);
    }
}
