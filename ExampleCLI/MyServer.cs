﻿using System;
using NamedPipeWrapper;

namespace ExampleCLI
{
    class MyServer
    {

        private readonly NamedPipeServer<MyMessage> server;
        private bool KeepRunning
        {
            get
            {
                var line = Console.ReadLine();

                if (line == "exit")
                    return false;
                server.PushMessage(new MyMessage() { Text = line });
                return true;
            }
        }

        public MyServer(string pipeName)
        {
            server = new NamedPipeServer<MyMessage>(pipeName);
            server.ClientConnected += OnClientConnected;
            server.ClientDisconnected += OnClientDisconnected;
            server.ClientMessage += OnClientMessage;
            server.Error += OnError;
            server.Start();
            while (KeepRunning)
            {
                // Do nothing - wait for user to press 'q' key
            }
            server.Stop();
        }

        private void OnClientConnected(NamedPipeConnection<MyMessage, MyMessage> connection)
        {
            Console.WriteLine("Client {0} is now connected!", connection.Id);
            connection.PushMessage(new MyMessage
                {
                    Id = new Random().Next(),
                    Text = "Welcome!"
                });
        }

        private void OnClientDisconnected(NamedPipeConnection<MyMessage, MyMessage> connection)
        {
            Console.WriteLine("Client {0} disconnected", connection.Id);
        }

        private void OnClientMessage(NamedPipeConnection<MyMessage, MyMessage> connection, MyMessage message)
        {
            Console.WriteLine("Client {0} says: {1}", connection.Id, message);
        }

        private void OnError(Exception exception)
        {
            Console.Error.WriteLine("ERROR: {0}", exception);
        }
    }
}