using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Amphibian.Geometry;
using Microsoft.Xna.Framework.Content;
using Amphibian.Systems.Rendering.Sprites;

namespace Amphibian.Systems.Rendering
{

    /*public class DirectionalSpriteDefinition
    {
        public Dictionary<Direction, StaticSpriteDefinition> Directions { get; }
        public Direction AvailableDirections { get; set; }

        public void Draw (SpriteBatch spriteBatch, PointFP position, Direction dir, SpriteData spriteData)
        {
            Directions[dir].Draw(spriteBatch, position, spriteData);
        }
    }*/

    

    

    /*public class AnimationSetDefinition
    {
        public Dictionary<String, AnimatedSpriteDefinition> Sequences { get; }

        public void Draw (SpriteBatch spriteBatch, PointFP position, String set, AnimationData animData, SpriteData spriteData)
        {
            Sequences[set].Draw(spriteBatch, position, animData, spriteData);
        }
    }

    public class DirectionalAnimationSetDefinition
    {
        public Dictionary<String, DirectionalAnimatedSpriteDefinition> Sequences { get; }

        public void Draw (SpriteBatch spriteBatch, PointFP position, Direction dir, String set, AnimationData animData, SpriteData spriteData)
        {
            Sequences[set].Draw(spriteBatch, position, dir, animData, spriteData);
        }
    }*/

    //----


    /*public class DirectionalSpriteInfo : Sprite
    {
        DirectionalSpriteDefinition Definition;
        SpriteData SpriteData;
        public Direction CurrentDirection { get; set; }

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            Definition.Draw(spriteBatch, position, CurrentDirection, SpriteData);
        }
    }

    public class DirectionalAnimatedSpriteInfo : Sprite
    {
        DirectionalAnimatedSpriteDefinition Definition;
        //SpriteData SpriteData;
        Dictionary<Direction, AnimationData> AnimInfo;
        public Direction CurrentDirection { get; set; }

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            Definition.Draw(spriteBatch, position, CurrentDirection, AnimInfo[CurrentDirection], SpriteData);
        }
    }

    public class AnimationSetInfo : Sprite
    {
        AnimationSetDefinition Definition;
        //SpriteData SpriteData;
        Dictionary<String, AnimationData> SequenceInfo;
        public String CurrentSequence;

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            Definition.Draw(spriteBatch, position, CurrentSequence, SequenceInfo[CurrentSequence], SpriteData);
        }
    }

    public class DirectionalAnimationSetInfo : Sprite
    {
        DirectionalAnimationSetDefinition Definition;
        //SpriteData SpriteData;
        Dictionary<Direction, Dictionary<String, AnimationData>> SequenceInfo;
        public String CurrentSequence;
        public Direction CurrentDirection;

        public override void Draw (SpriteBatch spriteBatch, PointFP position)
        {
            Definition.Draw(spriteBatch, position, CurrentDirection, CurrentSequence, SequenceInfo[CurrentDirection][CurrentSequence], SpriteData);
        }
    }*/
}
