using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /** CCLayerGradient is a subclass of CCLayerColor that draws gradients across
  the background.

  All features from CCLayerColor are valid, plus the following new features:
  - direction
  - final color
  - interpolation mode

  Color is interpolated between the startColor and endColor along the given
  vector (starting at the origin, ending at the terminus).  If no vector is
  supplied, it defaults to (0, -1) -- a fade from top to bottom.

  If 'compressedInterpolation' is disabled, you will not see either the start or end color for
  non-cardinal vectors; a smooth gradient implying both end points will be still
  be drawn, however.

  If ' compressedInterpolation' is enabled (default mode) you will see both the start and end colors of the gradient.

  @since v0.99.5
     */
    public class CCLayerGradient : CCLayerColor
    {
    }
}
