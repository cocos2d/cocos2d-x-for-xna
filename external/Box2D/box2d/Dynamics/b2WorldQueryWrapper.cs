using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using Box2D.Collision;

namespace Box2D.Dynamics
{
    public class b2WorldQueryWrapper
    {
        public bool QueryCallback(int proxyId)
        {
            b2FixtureProxy proxy = (b2FixtureProxy)broadPhase.GetUserData(proxyId);
            return callback.ReportFixture(proxy.fixture);
        }

        public b2BroadPhase broadPhase;
        public b2QueryCallback callback;
    }
}
