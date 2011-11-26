/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
Copyright (c) 2011      Fulcrum Mobile Network, Inc.

http://www.cocos2d-x.org
http://www.openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTMXObjectGroup : CCObject
    {
        protected CCPoint m_tPositionOffset;
        /// <summary>
        /// offset position of child objects
        /// </summary>
        public CCPoint PositionOffset
        {
            get { return m_tPositionOffset; }
            set { m_tPositionOffset = value; }
        }

        protected Dictionary<string, string> m_pProperties;
        /// <summary>
        /// list of properties stored in a dictionary
        /// </summary>
        public Dictionary<string, string> Properties
        {
            get { return m_pProperties; }
            set { m_pProperties = value; }
        }

        protected List<Dictionary<string, string>> m_pObjects;
        /// <summary>
        /// array of the objects
        /// </summary>
        public List<Dictionary<string, string>> Objects
        {
            get { return Objects; }
            set { Objects = value; }
        }

        /// <summary>
        /// name of the group
        /// </summary>
        protected string m_sGroupName;
        public string GroupName
        {
            get { return m_sGroupName; }
            set { m_sGroupName = value; }
        }

        public CCTMXObjectGroup()
        {
            m_pObjects = new List<Dictionary<string, string>>();
            m_pProperties = new Dictionary<string, string>();
        }

        /// <summary>
        ///  return the value for the specific property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string propertyNamed(string propertyName)
        {
           return m_pProperties[propertyName];
        }

        /// <summary>
        ///  return the dictionary for the specific object name.
        ///  It will return the 1st object found on the array for the given name.
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        Dictionary<string, string> objectNamed(string objectName)
        {
            if (m_pObjects != null && m_pObjects.Count > 0)
            {
                for (int i = 0; i < m_pObjects.Count; i++)
                {
                    string name = m_pObjects[i]["name"];
                    if (name != null && name == objectName)
                    {
                        return m_pObjects[i];
                    }
                }
            }
            // object not found
            return null;
        }
    }
}
