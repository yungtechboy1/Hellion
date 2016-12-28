using Hellion.Core.IO;
using System;
using System.Linq;

namespace Hellion.World.Systems
{
    public class Chat
    {
        private Player player;

        /// <summary>
        /// Creates a new instance of the Chat module.
        /// </summary>
        /// <param name="owner"></param>
        public Chat(Player owner)
        {
            this.player = owner;
        }

        /// <summary>
        /// Executes a regular or GM command.
        /// </summary>
        /// <param name="chatMessage"></param>
        public void CommandChat(string chatMessage)
        {
            string[] chatCommandArray = chatMessage.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (chatCommandArray.Length > 1 && this.player.Authority >= 80)
            {
                var chatCommand = chatCommandArray.First();

                if (this.IsNormalCommand(chatCommand))
                    this.NormalCommandChat(chatCommand, chatMessage);
                else
                    this.GMCommand(chatCommand, chatCommandArray);
            }
        }

        /// <summary>
        /// Executes a regular command.
        /// </summary>
        /// <param name="chatMessageCommand"></param>
        /// <param name="chatMessage"></param>
        private void NormalCommandChat(string chatMessageCommand, string chatMessage)
        {
        }

        /// <summary>
        /// Executes a GM command.
        /// </summary>
        /// <param name="chatCommand"></param>
        /// <param name="chatCommandArray"></param>
        private void GMCommand(string chatCommand, string[] chatCommandArray)
        {
            switch (chatCommand)
            {
                default:
                    Log.Info("Unknow GM command '{0}'.", chatCommand);
                    break;
            }
        }

        /// <summary>
        /// Check if the chat command is a regular command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool IsNormalCommand(string command)
        {
            switch (command)
            {
                case "/s":
                case "/shout":
                case "/w":
                case "/whisper":
                case "/p":
                case "/party":
                case "/g":
                case "/guild":
                    return true;
                default: return false;
            }
            
        }
    }
}
