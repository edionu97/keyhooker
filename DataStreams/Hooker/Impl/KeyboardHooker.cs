using System;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace DataStreams.Hooker.Hooker.Impl
{
    public class KeyboardHooker : IHooker<char>
    {
        public void Start(Action<char> onHookedAction = null)
        {
            Configure(onHookedAction);
            Application.Run();
        }

        private static void Configure(Action<char> onAction)
        {
            if (onAction == null)
            {
                onAction = Console.WriteLine;
            }

            Hook.GlobalEvents().KeyPress += (_, e) =>onAction(e.KeyChar);
        }
    }
}
