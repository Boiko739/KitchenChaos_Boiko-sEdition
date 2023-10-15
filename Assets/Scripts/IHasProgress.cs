using System;

namespace KitchenChaos
{
    public interface IHasProgress
    {
        public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

        public class OnProgressChangedEventArgs : EventArgs
        {
            public float progressNormalized;

            public OnProgressChangedEventArgs()
            { }

            public OnProgressChangedEventArgs(float progressNormalized)
            {
                this.progressNormalized = progressNormalized;
            }
        }
    }
}