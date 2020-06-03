using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
using Xamarin.Forms;

namespace PulsooximeterApp
{
            public class PulseTrigger : TriggerAction<Image>
        {
            public AnimationAction Action { get; set; }
            public enum AnimationAction
            { Beat }

            protected override async void Invoke(Image sender)
            {
                if (sender != null)
                {
                    await Pulse(sender);
                }
            }

        private async Task Pulse(Image targetImage)
        {
            await targetImage.ScaleTo(1.2, 100, Easing.BounceOut);

            await targetImage.ScaleTo(1, 200, Easing.BounceIn);
        }
    }

}
