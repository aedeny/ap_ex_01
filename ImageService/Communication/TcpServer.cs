﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logger;
using Infrastructure.Logging;

namespace ImageService.Communication
{
    public class TcpServer : ITcpServer
    {
        private readonly int _port;
        private TcpListener _listener;
        private ICollection<TcpClient> _clients;
        private readonly ILoggingService _loggingService;
        private readonly IClientHandlerFactory _clientHandlerFactory;
        private List<ITcpClientHandler> _clientHandlers;

        public TcpServer(int port, ILoggingService loggingService, IClientHandlerFactory clientHandlerFactory)
        {
            _clientHandlers = new List<ITcpClientHandler>();
            _clientHandlerFactory = clientHandlerFactory;
            _port = port;
            _clients = new List<TcpClient>();
            _loggingService = loggingService;
            Start();
        }


        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);
            _listener = new TcpListener(ep);

            _listener.Start();
            _loggingService.Log("Waiting for connections...", MessageTypeEnum.Info);

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = _listener.AcceptTcpClient();
                        _loggingService.Log("Got new connection", MessageTypeEnum.Info);
                        ITcpClientHandler ch = _clientHandlerFactory.Create(client,_loggingService);

                        _clientHandlers.Add(ch);
                        ch.HandleClient();
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }

                _loggingService.Log("Server stopped", MessageTypeEnum.Info);
            });
            task.Start();
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}