using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCLayerMultiplex : CCLayer
    {

        protected uint m_nEnabledLayer;
        protected List<CCLayer> m_pLayers;


        public CCLayerMultiplex() 
        {
        
        }

        /// <summary>
        ///  creates a CCLayerMultiplex with one or more layers using a variable argument list. 
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static CCLayerMultiplex layerWithLayers(params CCLayer[] layer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  * lua script can not init with undetermined number of variables
        /// * so add these functinons to be used with lua.
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static CCLayerMultiplex layerWithLayer(CCLayer layer)
        {
            throw new NotImplementedException();
        }

        public void addLayer(CCLayer layer)
        {
            throw new NotImplementedException();
        }

        public bool initWithLayer(CCLayer layer)
        {
            throw new NotImplementedException();
        }

        /** initializes a MultiplexLayer with one or more layers using a variable argument list. */
        public bool initWithLayers(CCLayer layer, params string[] va_list)
        {
            throw new NotImplementedException();
        }
        /** switches to a certain layer indexed by n. 
        The current (old) layer will be removed from it's parent with 'cleanup:YES'.
        */
        public void switchTo(uint n)
        {
            throw new NotImplementedException();
        }
        /** release the current layer and switches to another layer indexed by n.
        The current (old) layer will be removed from it's parent with 'cleanup:YES'.
        */
        void switchToAndReleaseMe(uint n)
        {
            throw new NotImplementedException();
        }
    }
}
