using Unity.Netcode.Components;

namespace OtherScripts
{
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}