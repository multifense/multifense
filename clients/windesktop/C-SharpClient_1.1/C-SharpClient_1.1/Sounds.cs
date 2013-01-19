using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace C_SharpClient_1._1
{
    class Sounds
    {
        private SoundEffect youLose;
        private SoundEffectInstance youLoseInstance;
        private SoundEffect dedefloweredtorpedo;
        private SoundEffectInstance dedefloweredtorpedoInstance;
        private SoundEffect multowerDeplayer;
        private SoundEffectInstance multowerDeplayerInstance;


        public Sounds(Game content)
        {
            multowerDeplayer = content.Content.Load<SoundEffect>("multower deplayer");
            dedefloweredtorpedo = content.Content.Load<SoundEffect>("deflowered torpedo");
            youLose = content.Content.Load<SoundEffect>("multower slowed");
            multowerDeplayerInstance = multowerDeplayer.CreateInstance();
            multowerDeplayerInstance.IsLooped = true;
            dedefloweredtorpedoInstance = dedefloweredtorpedo.CreateInstance();
            dedefloweredtorpedoInstance.IsLooped = true;
            youLoseInstance = youLose.CreateInstance();
            multowerDeplayerInstance.Play();
        }
        public void PlayYouLose()
        {
            multowerDeplayerInstance.Stop();
            dedefloweredtorpedoInstance.Stop();
            youLoseInstance.Play();
        }
        public void PlayDeFlowered()
        {
            multowerDeplayerInstance.Stop();
            dedefloweredtorpedoInstance.Play();
            youLoseInstance.Stop();
        }
        public void PlayMultower()
        {
            multowerDeplayerInstance.Play();
            dedefloweredtorpedoInstance.Stop();
            youLoseInstance.Stop();
        }
    }
}
