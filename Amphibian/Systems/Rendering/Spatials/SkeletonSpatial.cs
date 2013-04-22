using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spine;
using Amphibian.EntitySystem;
using Amphibian.Components;
using Microsoft.Xna.Framework.Content;
using Amphibian.Systems.Rendering.Sprites;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;
using Amphibian.Systems.Rendering.Sprites.Xml;

namespace Amphibian.Systems.Rendering.Spatials
{
    public class SkeletonSpatial : SpineSpatial
    {
        private class SpatialTypeRecord
        {
            public SkeletonData Data;
            public Dictionary<string, string> ActivityMap;
            public Dictionary<string, string> DefaultAnimationMap;
            public Dictionary<string, Dictionary<Direction, ISpineDirectionElement>> DirectedAnimationMap;
            public string DefaultAnimation;
        }

        private static Dictionary<String, SpatialTypeRecord> _registered = new Dictionary<string, SpatialTypeRecord>();

        private SpatialTypeRecord _record;
        private Skeleton _skeleton;
        private Animation _animation;
        private string _activity;
        private float _time;

        public SkeletonSpatial (string contentPath, EntityWorld world, string skin)
            : base(world)
        {
            if (!_registered.TryGetValue(contentPath, out _record))
                _record = Load(contentPath);

            _skeleton = new Skeleton(_record.Data);
            _skeleton.SetSkin(skin);
            _skeleton.SetSlotsToBindPose();
        }

        private SpatialTypeRecord Load (string contentPath)
        {
            SpatialTypeRecord record = new SpatialTypeRecord();

            using (Stream contentStream = TitleContainer.OpenStream(contentPath)) {
                using (XmlReader reader = XmlReader.Create(contentStream)) {
                    XmlSkeletonDefElement xmldef = new XmlSkeletonDefElement();
                    xmldef.ReadXml(reader);

                    Atlas atlas = new Atlas(xmldef.Atlas.Source, new XnaTextureLoader(World.Frame.Engine.GraphicsDevice));
                    SkeletonJson json = new SkeletonJson(atlas);

                    record.Data = json.ReadSkeletonData(xmldef.Skeleton.Source);
                    record.DefaultAnimationMap = xmldef.BuildDefaultAnimationMap();
                    record.DirectedAnimationMap = xmldef.BuildDirectedAnimationMap();
                    record.ActivityMap = xmldef.BuildActivityMap();
                    record.DefaultAnimation = xmldef.ActivityMap.DefaultAnimation;
                }
            }

            _registered[contentPath] = record;
            return record;
        }

        public override void Render (SkeletonRenderer skeletonRenderer, Entity entity, Renderable position)
        {
            ActivityComponent activityCom = null;
            DirectionComponent directionCom = null;

            foreach (IComponent com in World.EntityManager.GetComponents(entity)) {
                if (com is ActivityComponent)
                    activityCom = com as ActivityComponent;
                else if (com is DirectionComponent)
                    directionCom = com as DirectionComponent;
            }

            if (activityCom != null && activityCom.Activity != _activity) {
                string animationKey = null;
                if (!_record.ActivityMap.TryGetValue(activityCom.Activity, out animationKey))
                    animationKey = _record.DefaultAnimation;

                string animation = "";
                if (animationKey != null) {
                    if (directionCom != null && _record.DirectedAnimationMap.ContainsKey(animationKey)) {
                        if (_record.DirectedAnimationMap[animationKey].ContainsKey(directionCom.Direction))
                            animation = _record.DirectedAnimationMap[animationKey][directionCom.Direction].Animation;
                        else if (_record.DefaultAnimationMap.ContainsKey(animationKey))
                            animation = _record.DefaultAnimationMap[animationKey];
                    }
                    else if (_record.DefaultAnimationMap.ContainsKey(animationKey))
                        animation = _record.DefaultAnimationMap[animationKey];
                }

                if (_animation == null || animation != _animation.Name) {
                    _animation = _skeleton.Data.FindAnimation(animation);
                    _time = 0;
                }

                if (_animation == null && _skeleton.Data.Animations.Count > 0) {
                    _animation = _skeleton.Data.Animations[0];
                    _time = 0;
                }

                if (_animation != null) {
                    _time += (float)World.GameTime.ElapsedGameTime.TotalSeconds;
                    _animation.Apply(_skeleton, _time, true);
                }
            }

            _skeleton.RootBone.X = (float)position.RenderX;
            _skeleton.RootBone.Y = (float)position.RenderY;
            _skeleton.UpdateWorldTransform();

            skeletonRenderer.Draw(_skeleton);
        }
    }
}
