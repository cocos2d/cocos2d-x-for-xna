using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace cocos2d
{
    public class CCDirector
    {

    }

    /// <summary>
    ///  Possible OpenGL projections used by director
    /// </summary>
    enum ccDirectorProjection
    {
        /// sets a 2D projection (orthogonal projection)
        kCCDirectorProjection2D,

        /// sets a 3D projection with a fovy=60, znear=0.5f and zfar=1500.
        kCCDirectorProjection3D,

        /// it calls "updateProjection" on the projection delegate.
        kCCDirectorProjectionCustom,

        /// Detault projection is 3D projection
        kCCDirectorProjectionDefault = kCCDirectorProjection3D,

        // backward compatibility stuff
        CCDirectorProjection2D = kCCDirectorProjection2D,
        CCDirectorProjection3D = kCCDirectorProjection3D,
        CCDirectorProjectionCustom = kCCDirectorProjectionCustom
    }

    /// <summary>
    /// Possible Director Types.
    /// since v0.8.2
    /// </summary>
    enum ccDirectorType
    {
        /** Will use a Director that triggers the main loop from an NSTimer object
         *
         * Features and Limitations:
         * - Integrates OK with UIKit objects
         * - It the slowest director
         * - The interval update is customizable from 1 to 60
         */
        kCCDirectorTypeNSTimer,

        /** will use a Director that triggers the main loop from a custom main loop.
         *
         * Features and Limitations:
         * - Faster than NSTimer Director
         * - It doesn't integrate well with UIKit objects
         * - The interval update can't be customizable
         */
        kCCDirectorTypeMainLoop,

        /** Will use a Director that triggers the main loop from a thread, but the main loop will be executed on the main thread.
         *
         * Features and Limitations:
         * - Faster than NSTimer Director
         * - It doesn't integrate well with UIKit objects
         * - The interval update can't be customizable
         */
        kCCDirectorTypeThreadMainLoop,

        /** Will use a Director that synchronizes timers with the refresh rate of the display.
         *
         * Features and Limitations:
         * - Faster than NSTimer Director
         * - Only available on 3.1+
         * - Scheduled timers & drawing are synchronizes with the refresh rate of the display
         * - Integrates OK with UIKit objects
         * - The interval update can be 1/60, 1/30, 1/15
         */
        kCCDirectorTypeDisplayLink,

        /** Default director is the NSTimer directory */
        kCCDirectorTypeDefault = kCCDirectorTypeNSTimer,

        // backward compatibility stuff
        CCDirectorTypeNSTimer = kCCDirectorTypeNSTimer,
        CCDirectorTypeMainLoop = kCCDirectorTypeMainLoop,
        CCDirectorTypeThreadMainLoop = kCCDirectorTypeThreadMainLoop,
        CCDirectorTypeDisplayLink = kCCDirectorTypeDisplayLink,
        CCDirectorTypeDefault = kCCDirectorTypeDefault
    }

    /// <summary>
    /// Possible device orientations
    /// </summary>
    enum ccDeviceOrientation
    {
        /// Device oriented vertically, home button on the bottom
        kCCDeviceOrientationPortrait = 0, // UIDeviceOrientationPortrait,	
        /// Device oriented vertically, home button on the top
        kCCDeviceOrientationPortraitUpsideDown = 1, // UIDeviceOrientationPortraitUpsideDown,
        /// Device oriented horizontally, home button on the right
        kCCDeviceOrientationLandscapeLeft = 2, // UIDeviceOrientationLandscapeLeft,
        /// Device oriented horizontally, home button on the left
        kCCDeviceOrientationLandscapeRight = 3, // UIDeviceOrientationLandscapeRight,

        // Backward compatibility stuff
        CCDeviceOrientationPortrait = kCCDeviceOrientationPortrait,
        CCDeviceOrientationPortraitUpsideDown = kCCDeviceOrientationPortraitUpsideDown,
        CCDeviceOrientationLandscapeLeft = kCCDeviceOrientationLandscapeLeft,
        CCDeviceOrientationLandscapeRight = kCCDeviceOrientationLandscapeRight
    }
}
