using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using TInput = System.String;
using TOutput = cocos2d.Framework.CCData;

namespace cocos2d.Content.Pipeline.Importers
{
    [ContentProcessor(DisplayName = "TextProcessor")]
    public class TextProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            TOutput result = new TOutput();
            result.Content = input;

            return result;
        }
    }
}