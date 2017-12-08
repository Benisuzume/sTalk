using sTalk.Client.Communication.Packets;
using sTalk.Libraries.Encryption;
using System.Net.Sockets;

namespace sTalk.Client.Communication
{
    public partial class Server
    {
        private void SendHandshake()
        {
            _aes = new Symmetric();

            var json = Outgoing.Handshake(_aes.Key, _aes.IV, _username);
            var buffer = _rsa.Encrypt(json);

            _state = States.Initializing;
            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendLogIn()
        {
            var hash = Hash.CalculateMD5(_token + Hash.CalculateMD5(_password));

            var json = Outgoing.LogIn(hash);
            var buffer = _aes.Encrypt(json);

            _state = States.Authenticating;
            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendLogOut()
        {
            var json = Outgoing.LogOut();
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendBlockUser(string username)
        {
            var json = Outgoing.BlockUser(username);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendUnblockUser(string username)
        {
            var json = Outgoing.UnblockUser(username);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendAddContact(string username)
        {
            var json = Outgoing.AddContact(username);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendDeleteContact(string username)
        {
            var json = Outgoing.DeleteContact(username);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendPrivateMessage(string guid, string username, string message)
        {
            var json = Outgoing.PrivateMessage(guid, username, message);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendJoinRoom(uint roomId)
        {
            var json = Outgoing.JoinRoom(roomId);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendLeaveRoom()
        {
            var json = Outgoing.LeaveRoom();
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendPublicMessage(string guid, string message)
        {
            var json = Outgoing.PublicMessage(guid, message);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendGetProfilePicture(string username)
        {
            var json = Outgoing.GetProfilePicture(username);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendSetProfilePicture(byte[] image)
        {
            var json = Outgoing.SetProfilePicture(image);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }
    }
}