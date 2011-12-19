using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Xml.Linq;

using TInput = System.String;
using TOutput = cocos2d.Framework.CCContent;
using System.Text;

namespace cocos2d.Content.Pipeline.Importers.TMX
{

    [ContentProcessor(DisplayName = "TMX Processor")]
    public class TMXProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            TOutput output = new TOutput();
            output.Content = input;
            XElement doc = XElement.Parse(input);

            IEnumerable<XElement> elemnet = doc.Descendants("data");
            foreach (var item in elemnet)
            {
                string str = item.Value;
                byte[] bs = Convert.FromBase64String(str);
                bs = Decompress.GZipDecompress(bs);
                output.Date.Add(bs);
            }
            return output;
            throw new NotImplementedException();
        }
    }
}