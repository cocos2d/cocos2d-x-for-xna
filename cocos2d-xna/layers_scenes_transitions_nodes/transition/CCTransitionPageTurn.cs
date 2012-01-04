using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /**
    @brief A transition which peels back the bottom right hand corner of a scene
    to transition to the scene beneath it simulating a page turn.

    This uses a 3DAction so it's strongly recommended that depth buffering
    is turned on in CCDirector using:

     CCDirector::sharedDirector()->setDepthBufferFormat(kDepthBuffer16);

     @since v0.8.2
    */
    public class CCTransitionPageTurn : CCTransitionScene
    {
        protected bool m_bBack;

        public CCTransitionPageTurn()
        { }
  

        /**
        * Creates a base transition with duration and incoming scene.
        * If back is true then the effect is reversed to appear as if the incoming 
        * scene is being turned from left over the outgoing scene.
        */
        public static CCTransitionPageTurn transitionWithDuration(float t, CCScene scene, bool backwards)
        {
            throw new NotImplementedException();
        }

        /**
        * Creates a base transition with duration and incoming scene.
        * If back is true then the effect is reversed to appear as if the incoming 
        * scene is being turned from left over the outgoing scene.
        */
        public virtual bool initWithDuration(float t, CCScene scene, bool backwards)
        {
            throw new NotImplementedException();
        }

        public CCActionInterval actionWithSize(ccGridSize vector)
        {
            throw new NotImplementedException();
        }

        public override void onEnter()
        { }

        protected override void sceneOrder()
        { }
    }
}
