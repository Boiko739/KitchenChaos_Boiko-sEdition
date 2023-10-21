using System;

namespace KitchenChaos
{
    public interface IHasProgress
    {
        public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

        public class OnProgressChangedEventArgs : EventArgs
        {
            public float ProgressNormalized { get; set; }

            public OnProgressChangedEventArgs()
            { }

            public OnProgressChangedEventArgs(float progressNormalized)
            {
                this.ProgressNormalized = progressNormalized;
            }

            public readonly float minWarningProgressAmount = .4f;
        }
    }
}