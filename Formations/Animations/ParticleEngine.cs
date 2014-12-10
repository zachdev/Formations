﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;

namespace Formations
{
    [DataContract]
    public class ParticleEngine
    {
        private Random random;
        [IgnoreDataMember]
        private Vector2 _emitterLocation;
        public Vector2 EmitterLocation { get { return _emitterLocation; } set { _emitterLocation = value; } }
        private List<Particle> particles;
        [IgnoreDataMember]
        private List<Texture2D> textures;

        public bool particlesOn {get; set;}
        public int count;

        public ParticleEngine(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
        }

        public void Update()
        {
            if (count > 50)
            {
                particlesOn = false;

                count = 0;
            }

            if (particlesOn)
            {
                int total = 1;

                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }

                count++;
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 2 - 1),
                                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.17f * (float)(random.NextDouble() * 3 - 1);
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 80 + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}