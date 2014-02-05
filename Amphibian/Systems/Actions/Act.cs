using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amphibian.Geometry;
using Amphibian.Utility;
using Microsoft.Xna.Framework;

namespace Amphibian.Systems.Actions
{
    public static class Act
    {
        public static T Action<T> ()
            where T : EntityAction, new()
        {
            T action = Pools<T>.Obtain();
            action.Pool = Pools<T>.Pool;
            return action;
        }

        public static AlphaAction Alpha (float a)
        {
            return Alpha(a, 0, null);
        }

        public static AlphaAction Alpha (float a, float duration)
        {
            return Alpha(a, duration, null);
        }

        public static AlphaAction Alpha (float a, float duration, Interpolation interpolation)
        {
            AlphaAction action = Action<AlphaAction>();
            action.Alpha = a;
            action.Duration = duration;
            action.Interpolation = interpolation;
            return action;
        }

        public static ColorAction Color (Color color)
        {
            return Color(color, 0, null);
        }

        public static ColorAction Color (Color color, float duration)
        {
            return Color(color, duration, null);
        }

        public static ColorAction Color (Color color, float duration, Interpolation interpolation)
        {
            ColorAction action = Action<ColorAction>();
            action.EndColor = color;
            action.Duration = duration;
            action.Interpolation = interpolation;
            return action;
        }

        public static AlphaAction FadeIn (float duration)
        {
            return FadeIn(duration, null);
        }

        public static AlphaAction FadeIn (float duration, Interpolation interpolation)
        {
            AlphaAction action = Action<AlphaAction>();
            action.Alpha = 1;
            action.Duration = duration;
            action.Interpolation = interpolation;
            return action;
        }

        public static AlphaAction FadeOut (float duration)
        {
            return FadeOut(duration, null);
        }

        public static AlphaAction FadeOut (float duration, Interpolation interpolation)
        {
            AlphaAction action = Action<AlphaAction>();
            action.Alpha = 0;
            action.Duration = duration;
            action.Interpolation = interpolation;
            return action;
        }

        public static ScaleToAction ScaleTo (float scale)
        {
            return ScaleTo(scale, 0, null);
        }

        public static ScaleToAction ScaleTo (float scale, float duration)
        {
            return ScaleTo(scale, duration, null);
        }

        public static ScaleToAction ScaleTo (float scale, float duration, Interpolation interpolation)
        {
            ScaleToAction action = Action<ScaleToAction>();
            action.Scale = scale;
            action.Duration = duration;
            action.Interpolation = interpolation;
            return action;
        }
    }
}
