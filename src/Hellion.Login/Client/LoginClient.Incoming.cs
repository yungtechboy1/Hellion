using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.Database;
using System.Linq;

namespace Hellion.Login.Client
{
    public partial class LoginClient
    {
        /// <summary>
        /// Login request.
        /// </summary>
        /// <param name="packet"></param>
        [FFIncomingPacket(PacketType.CERTIFY)]
        private void OnCertifyRequest(FFPacket packet)
        {
            var buildVersion = packet.Read<string>();
            var username = packet.Read<string>();
            string password = string.Empty;

            if (this.Server.LoginConfiguration.PasswordEncryption)
                password = this.DecryptPassword(packet.ReadBytes(16 * 42));
            else
                password = packet.Read<string>();

            Log.Debug("Recieved from client: buildVersion: {0}, username: {1}, password: {2}", buildVersion, username, password);
            
            var user = DatabaseService.Users.Get(x => x.Username.ToLower() == username.ToLower());

            if (user == null)
            {
                Log.Info($"User '{username}' logged in with bad credentials. (Bad username)");
                this.SendLoginError(ErrorType.FLYFF_ACCOUNT);
                this.Server.RemoveClient(this);
            }
            else
            {
                this.accountId = user.Id;

                if (buildVersion.ToLower() != this.Server.LoginConfiguration.BuildVersion?.ToLower())
                {
                    Log.Info($"User '{username}' logged in with bad build version.");
                    this.SendLoginError(ErrorType.CERT_GENERAL);
                    this.Server.RemoveClient(this);
                    return;
                }

                if (password.ToLower() != user.Password.ToLower())
                {
                    Log.Info($"User '{username}' logged in with bad credentials. (Bad password)");
                    this.SendLoginError(ErrorType.FLYFF_PASSWORD);
                    this.Server.RemoveClient(this);
                    return;
                }

                if (user.Authority <= 0)
                {
                    Log.Info($"User '{username}' account is suspended.");
                    this.SendLoginError(ErrorType.BLOCKGOLD_ACCOUNT);
                    this.Server.RemoveClient(this);
                    return;
                }

                if (user.Verification == false && this.Server.LoginConfiguration.AccountVerification == true)
                {
                    Log.Info($"User '{username}' account's has not been verified yet.");
                    this.SendLoginError(ErrorType.BLOCKGOLD_ACCOUNT);
                    this.Server.RemoveClient(this);
                    return;
                }

                LoginClient connectedClient = null;
                if (this.IsAlreadyConnected(out connectedClient))
                {
                    this.SendLoginError(ErrorType.DUPLICATE_ACCOUNT);
                    this.Server.RemoveClient(this);
                    this.Server.RemoveClient(connectedClient);
                    return;
                }

                this.username = user.Username;
                this.SendServerList();
            }
        }
    }
}
