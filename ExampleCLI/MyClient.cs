using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamedPipeWrapper;

namespace ExampleCLI
{
    class MyClient
    {
        private readonly NamedPipeClient<MyMessage> client;

        private bool KeepRunning
        {
            get
            {
                var line= Console.ReadLine();

                if (line == "exit")
                    return false;
                client.PushMessage(new MyMessage() { Text = line });
                return true;
            }
        }

        public MyClient(string pipeName)
        {
            client = new NamedPipeClient<MyMessage>(pipeName);
            client.ServerMessage += OnServerMessage;
            client.Error += OnError;
            client.Start();
            while (KeepRunning)
            {
                // Do nothing - wait for user to press 'q' key
            }
            client.Stop();
        }

        private void OnServerMessage(NamedPipeConnection<MyMessage, MyMessage> connection, MyMessage message)
        {
            Console.WriteLine("Server says: {0}", message);
        }

        private void OnError(Exception exception)
        {
            Console.Error.WriteLine("ERROR: {0}", exception);
        }
    }
}
