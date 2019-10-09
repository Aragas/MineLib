using MineLib.Server.Core;

namespace MineLib.AddressTracker
{
    /// <summary>
    /// It should have an open connection (with pinging) with every MineLib process
    /// So it will know which process is working or not, maybe even send commands
    /// To some thing that creates and destroys processes based on that info.
    /// For example, if MineLib.Proxy is down, send a command to create a new process.
    /// ---
    /// Or maybe use the IPC protocol?
    /// </summary>
    public class Program : BaseProgram
    {
        public static void Main(string[] args) => Main<Program>(args);


        public ConnectionListener ConnectionListener { get; } = new ConnectionListener();

        public override void Run()
        {
            ConnectionListener.Start();
        }


        public override void Dispose()
        {
        }
    }
}