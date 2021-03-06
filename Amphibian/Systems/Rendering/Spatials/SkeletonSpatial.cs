﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Amphibian.Components;
using Amphibian.EntitySystem;
using Amphibian.Systems.Rendering.Spine;
using Amphibian.Systems.Rendering.Spine.Xml;
using Amphibian.Systems.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spine;

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

        public SkeletonSpatial (GraphicsDevice device, string contentPath, string skin)
        {
            if (!_registered.TryGetValue(contentPath, out _record))
                _record = Load(device, contentPath);

            _skeleton = new Skeleton(_record.Data);
            _skeleton.SetSkin(skin);
            _skeleton.SetSlotsToBindPose();
        }

        private SpatialTypeRecord Load (GraphicsDevice device, string contentPath)
        {
            SpatialTypeRecord record = new SpatialTypeRecord();

            using (Stream contentStream = TitleContainer.OpenStream(contentPath)) {
                using (XmlReader reader = XmlReader.Create(contentStream)) {
                    XmlSkeletonDefElement xmldef = new XmlSkeletonDefElement();
                    xmldef.ReadXml(reader);

                    Atlas atlas = new Atlas(xmldef.Atlas.Source, new XnaTextureLoader(device));
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

        public override Type RenderManagerType
        {
            get { return typeof(SpineRenderManager); }
        }

        public override void Render (SkeletonRenderer skeletonRenderer, EntityWorld world, Entity entity, Renderable position)
        {
            ActivityComponent activityCom = null;
            DirectionComponent directionCom = null;

            foreach (IComponent com in world.EntityManager.GetComponents(entity)) {
                if (com is ActivityComponent)
                    activityCom = com as ActivityComponent;
                else if (com is DirectionComponent)
                    directionCom = com as DirectionComponent;
            }

            if (activityCom != null && activityCom.Activity != _activity) {
                _activity = activityCom.Activity;

                string animationKey = null;
                if (!_record.ActivityMap.TryGetValue(activityCom.Activity, out animationKey))
                    animationKey = _record.DefaultAnimation;

                string animation = "";
                bool flipX = false;
                bool flipY = false;

                if (animationKey != null) {
                    if (directionCom != null && _record.DirectedAnimationMap.ContainsKey(animationKey)) {
                        if (_record.DirectedAnimationMap[animationKey].ContainsKey(directionCom.Direction)) {
                            ISpineDirectionElement element = _record.DirectedAnimationMap[animationKey][directionCom.Direction];
                            animation = element.Animation;
                            flipX = element.FlipX;
                            flipY = element.FlipY;
                        }
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

                _skeleton.FlipX = flipX;
                _skeleton.FlipY = flipY;
            }

            if (_animation != null) {
                _time += (float)world.GameTime.ElapsedGameTime.TotalSeconds;
                _animation.Apply(_skeleton, _time, true);
            }

            _skeleton.RootBone.X = (float)position.RenderX;
            _skeleton.RootBone.Y = (float)position.RenderY;
            _skeleton.UpdateWorldTransform();

            skeletonRenderer.Draw(_skeleton);
        }
    }
}
