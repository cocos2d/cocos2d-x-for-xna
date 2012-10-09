using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics
{
    public enum b2BodyType
    {
        b2_staticBody = 0,
        b2_kinematicBody,
        b2_dynamicBody

        //b2_bulletBody,
    }

    [Flags]
    public enum b2BodyFlags
    {
        e_islandFlag        = 0x0001,
        e_awakeFlag            = 0x0002,
        e_autoSleepFlag        = 0x0004,
        e_bulletFlag        = 0x0008,
        e_fixedRotationFlag    = 0x0010,
        e_activeFlag        = 0x0020,
        e_toiFlag            = 0x0040
    }

    
    public class b2Body
    {
        protected b2BodyType m_type;
        public b2BodyType BodyType
        {
            get { return (m_type); }
            set { m_type = value; }
        }

        protected b2BodyFlags m_flags;
        public b2BodyFlags BodyFlags
        {
            get { return (m_flags); }
            set { m_flags = value; }
        }

        protected int m_islandIndex;
        public int IslandIndex
        {
            get { return (m_islandIndex); }
            set { m_islandIndex = value; }
        }

        protected b2Transform m_xf;        // the body origin transform
        public b2Transform XF
        {
            get { return (m_xf); }
            set { m_xf = value; }
        }

        protected b2Sweep m_sweep;        // the swept motion for CCD
        public b2Sweep Sweep
        {
            get { return (m_sweep); }
            set { m_sweep = value; }
        }
        protected b2Vec2 m_linearVelocity;
        public b2Vec2 LinearVelocity
        {
            get { return (m_linearVelocity); }
            set { m_linearVelocity = value; }
        }
        protected float m_angularVelocity;
        public float AngularVelocity
        {
            get { return (m_angularVelocity); }
            set { m_angularVelocity = value; }
        }

        protected b2Vec2 m_force;
        public b2Vec2 Force
        {
            get { return (m_force); }
            set { m_force = value; }
        }
        protected float m_torque;
        public float Torque
        {
            get { return (m_torque); }
            set { m_torque = value; }
        }

        protected b2World m_world;
        public b2World World
        {
            get { return (m_world); }
            set { m_world = value; }
        }
        protected b2Body m_prev;
        public b2Body Prev
        {
            get { return (m_prev); }
            set { m_prev = value; }
        }
        protected b2Body m_next;
        public b2Body Next
        {
            get { return (m_next); }
            set { m_next = value; }
        }

        protected b2Fixture m_fixtureList;
        public b2Fixture FixtureList
        {
            get { return (m_fixtureList); }
            set { m_fixtureList = value; }
        }
        protected int m_fixtureCount;
        public int FixtureCount
        {
            get { return (m_fixtureCount); }
            set { m_fixtureCount = value; }
        }

        protected b2JointEdge m_jointList;
        public b2JointEdge JointList
        {
            get { return (m_jointList); }
            set { m_jointList = value; }
        }
        protected b2ContactEdge m_contactList;
        public b2ContactEdge ContactList
        {
            get { return (m_contactList); }
            set { m_contactList = value; }
        }

        protected float m_mass, m_invMass;
        public float Mass
        {
            get { return (m_mass); }
            set { m_mass = value; }
        }
        public float InvertedMass
        {
            get { return (m_invMass); }
            set { m_invMass = value; }
        }

        // Rotational inertia about the center of mass.
        protected float m_I, m_invI;
        public float I
        {
            get { return (m_I); }
            set { m_I = value; }
        }
        public float InvertedI
        {
            get { return (m_invI); }
            set { m_invI = value; }
        }
        

        protected float m_linearDamping;
        public float LinearDamping
        {
            get { return (m_linearDamping); }
            set { m_linearDamping = value; }
        }
        protected float m_angularDamping;
        public float AngularDamping
        {
            get { return (m_angularDamping); }
            set { m_angularDamping = value; }
        }
        protected float m_gravityScale;
        public float GravityScale
        {
            get { return (m_gravityScale); }
            set { m_gravityScale = value; }
        }

        protected float m_sleepTime;
        public float SleepTime
        {
            get { return (m_sleepTime); }
            set { m_sleepTime = value; }
        }

        protected object m_userData;
        public object UserData
        {
            get { return (m_userData); }
            set { m_userData = value; }
        }
    
    }
}
