﻿using Ether.Network;
using Ether.Network.Packets;
using Hellion.Core.Configuration;
using Hellion.Core.Database;
using Hellion.Core.IO;
using Hellion.Core.Network;
using System;
using System.Collections.Generic;
using System.IO;

namespace Hellion.Login
{
    /// <summary>
    /// Hellion LoginServer implementation.
    /// </summary>
    public sealed class LoginServer : NetServer<Client>
    {
        private const string LoginConfigurationFile = "config/login.json";
        private const string DatabaseConfigurationFile = "config/database.json";

        /// <summary>
        /// Gets the Database context.
        /// </summary>
        public static DatabaseContext DbContext
        {
            get
            {
                lock (syncDatabase)
                {
                    return dbContext;
                }
            }
        }
        private static object syncDatabase = new object();
        private static DatabaseContext dbContext = null;

        /// <summary>
        /// Gets the login server configuration.
        /// </summary>
        public LoginConfiguration LoginConfiguration { get; private set; }

        /// <summary>
        /// Gets the database configuration.
        /// </summary>
        public DatabaseConfiguration DatabaseConfiguration { get; private set; }

        /// <summary>
        /// Creates a new LoginServer instance.
        /// </summary>
        public LoginServer()
            : base()
        {
            Console.Title = "Hellion LoginServer";
            Log.Info("Starting LoginServer...");
        }

        /// <summary>
        /// LoginServer idle.
        /// </summary>
        protected override void Idle()
        {
            Log.Info("Server listening on port {0}", this.LoginConfiguration.Port);

            while (true)
            {
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Initialize the LoginServer.
        /// </summary>
        protected override void Initialize()
        {
            this.LoadConfiguration();
            this.ConnectToDatabase();
            this.ConnectToISC();

            Console.WriteLine();
        }

        /// <summary>
        /// On client connected.
        /// </summary>
        /// <param name="client">Client</param>
        protected override void OnClientConnected(NetConnection client)
        {
            Log.Info("New client connected!");
        }

        /// <summary>
        /// On client disconnected.
        /// </summary>
        /// <param name="client">Client</param>
        protected override void OnClientDisconnected(NetConnection client)
        {
            Log.Info("client disconnected");
        }

        /// <summary>
        /// Split incoming buffer into several FFPacket.
        /// </summary>
        /// <param name="buffer">Incoming buffer</param>
        /// <returns></returns>
        protected override IReadOnlyCollection<NetPacketBase> SplitPackets(byte[] buffer)
        {
            return FFPacket.SplitPackets(buffer);
        }

        /// <summary>
        /// Load the LoginServer configuration.
        /// </summary>
        private void LoadConfiguration()
        {
            Log.Info("Loading configuration...");

            if (File.Exists(LoginConfigurationFile) == false)
                ConfigurationManager.Save(new LoginConfiguration(), LoginConfigurationFile);

            this.LoginConfiguration = ConfigurationManager.Load<LoginConfiguration>(LoginConfigurationFile);

            this.Configuration.Ip = this.LoginConfiguration.Ip;
            this.Configuration.Port = this.LoginConfiguration.Port;

            if (File.Exists(DatabaseConfigurationFile) == false)
                ConfigurationManager.Save(new DatabaseConfiguration(), DatabaseConfigurationFile);

            this.DatabaseConfiguration = ConfigurationManager.Load<DatabaseConfiguration>(DatabaseConfigurationFile);

            Log.Done("Configuration loaded!");
        }

        /// <summary>
        /// Connect to the database.
        /// </summary>
        private void ConnectToDatabase()
        {
            try
            {
                Log.Info("Connecting to database...");
                dbContext = new DatabaseContext(this.DatabaseConfiguration);
                dbContext.Database.EnsureCreated();
                Log.Done("Connected to database!");
            }
            catch (Exception e)
            {
                Log.Error($"Cannot connect to database. {e.Message}");
            }
        }

        /// <summary>
        /// Connect to the Inter-Server.
        /// </summary>
        private void ConnectToISC()
        {
            Log.Info("Connecting to Inter-Server...");
            // TODO
            Log.Done("Connected to Inter-Server!");
        }
    }
}
