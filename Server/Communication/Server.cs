using sTalk.Libraries.Communication;
using sTalk.Libraries.Communication.Packet;
using sTalk.Libraries.Communication.Packet.Client;
using sTalk.Libraries.Communication.Packet.Data;
using sTalk.Libraries.Encryption;
using sTalk.Server.Communication.Packets;
using sTalk.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace sTalk.Server.Communication
{
    public class Server
    {
        public const int BufferSize = 10240;

        private static List<Client> _clients;
        private Socket _socket;
        private Asymmetric _rsa;

        public Server(string privateKey, int port)
        {
            _clients = new List<Client>(10000);

            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var localEP = new IPEndPoint(IPAddress.Any, port);
                _socket.Bind(localEP);

                _rsa = new Asymmetric(privateKey);

                _socket.Listen(1000);
                _socket.BeginAccept(AcceptCallback, null);
            }
            catch
            {
                throw;
            }
        }

        public static bool IsOnline(User user)
        {
            var query = from Client client in _clients
                        where client.IsLoggedIn && client.User.Id == user.Id
                        select client;

            return query.Any();
        }

        public static List<string> GetRoomUsers(Room room)
        {
            var query = from Client client in _clients
                        where client.IsInRoom && client.Room.Id == room.Id
                        select client.User.Name;

            return query.ToList();
        }

        public static void PublicMessage(Room room, User user, string message)
        {
            var query = from Client client in _clients
                        where client.IsInRoom && client.Room.Id == room.Id && client.User.Id != user.Id
                        select client;

            query.ToList().ForEach(((client) =>
            {
                client.PublicMessage(user.Name, message);
            }));
        }

        public static void UserStatusChanged(User user, Status status)
        {
            var query = from Client client in _clients
                        where client.IsLoggedIn && Contacts.IsContact(client.User.Id, user.Id)
                        select client;

            query.ToList().ForEach(((client) =>
            {
                client.StatusChanged(user.Name, status);
            }));
        }

        public static void UserJoinedRoom(Room room, User user)
        {
            var query = from Client client in _clients
                        where client.IsInRoom && client.Room.Id == room.Id && client.User.Id != user.Id
                        select client;

            query.ToList().ForEach(((client) =>
            {
                client.UserJoinedRoom(user.Name);
            }));
        }

        public static void UserLeftRoom(Room room, User user)
        {
            var query = from Client client in _clients
                        where client.IsInRoom && client.Room.Id == room.Id && client.User.Id != user.Id
                        select client;

            query.ToList().ForEach(((client) =>
            {
                client.UserLeftRoom(user.Name);
            }));
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var socket = _socket.EndAccept(ar);
            _socket.BeginAccept(AcceptCallback, null);

            var client = new Client(socket, BufferSize, SendCallback);
            _clients.Add(client);

            client.Socket.BeginReceive(client.Buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, client);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var client = (Client)ar.AsyncState;
            if (!client.Socket.Connected)
            {
                Disconnect(client);
                return;
            }

            var bytesRead = 0;
            try
            {
                bytesRead = client.Socket.EndReceive(ar);
            }
            catch
            {
                Disconnect(client);
                return;
            }

            if (bytesRead == 0)
            {
                Disconnect(client);
                return;
            }

            string buffer = string.Empty;
            Identifier packet = null;

            try
            {
                if (client.State == Client.States.Connected)
                    buffer = _rsa.Decrypt(client.Buffer.Skip(0).Take(bytesRead).ToArray());
                else
                    buffer = client.Decrypt(client.Buffer.Skip(0).Take(bytesRead).ToArray());

                packet = Incoming.Parse<Identifier>(buffer);
                if (packet == null)
                {
                    Disconnect(client);
                    return;
                }
            }
            catch
            {
                Disconnect(client);
                return;
            }

            client.Socket.BeginReceive(client.Buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, client);

            switch (packet.Id)
            {
                case Id.Handshake:
                    Handshake(client, buffer);
                    break;

                case Id.LogIn:
                    LogIn(client, buffer);
                    break;

                case Id.LogOut:
                    LogOut(client, buffer);
                    break;

                case Id.BlockUser:
                    BlockUser(client, buffer);
                    break;

                case Id.UnblockUser:
                    UnblockUser(client, buffer);
                    break;

                case Id.AddContact:
                    AddContact(client, buffer);
                    break;

                case Id.DeleteContact:
                    DeleteContact(client, buffer);
                    break;

                case Id.PrivateMessage:
                    PrivateMessage(client, buffer);
                    break;

                case Id.JoinRoom:
                    JoinRoom(client, buffer);
                    break;

                case Id.LeaveRoom:
                    LeaveRoom(client, buffer);
                    break;

                case Id.PublicMessage:
                    PublicMessage(client, buffer);
                    break;

                case Id.GetProfilePicture:
                    GetProfilePicture(client, buffer);
                    break;

                case Id.SetProfilePicture:
                    SetProfilePicture(client, buffer);
                    break;
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            var client = (Client)ar.AsyncState;
            if (!client.Socket.Connected)
            {
                Disconnect(client);
                return;
            }

            try
            {
                client.Socket.EndSend(ar);
            }
            catch
            {
                Disconnect(client);
                return;
            }

            switch (client.State)
            {
                case Client.States.Initializing:
                    client.State = Client.States.Initialized;
                    break;

                case Client.States.Authenticating:
                    client.State = Client.States.Authenticated;

                    UserStatusChanged(client.User, Status.Online);
                    break;

                case Client.States.Disconnecting:
                    Disconnect(client);
                    break;
            }
        }

        private void Disconnect(Client client)
        {
            if (client.Socket.Connected)
                client.Socket.Shutdown(SocketShutdown.Both);

            client.Socket.Close();

            if (client.IsLoggedIn)
            {
                UserStatusChanged(client.User, Status.Offline);

                if (client.IsInRoom)
                    UserLeftRoom(client.Room, client.User);
            }

            _clients.Remove(client);
        }

        private void Handshake(Client client, string buffer)
        {
            if (client.State != Client.States.Connected)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<Handshake>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            packet.Username = Sanitize.Username(packet.Username);
            if (packet.Key.Length != 32 || packet.IV.Length != 16 || packet.Username == null)
            {
                Disconnect(client);
                return;
            }

            client.Handshake(packet.Key, packet.IV, packet.Username);
        }

        private void LogIn(Client client, string buffer)
        {
            if (client.State != Client.States.Initialized)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<LogIn>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            packet.Hash = Sanitize.Hash(packet.Hash);
            if (packet.Hash == null)
            {
                Disconnect(client);
                return;
            }

            client.LogIn(packet.Hash);
        }

        private void LogOut(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<LogOut>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            client.LogOut();
        }

        private void BlockUser(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<BlockUser>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            packet.Username = Sanitize.Username(packet.Username);
            if (packet.Username == null)
            {
                Disconnect(client);
                return;
            }

            client.BlockUser(packet.Username);
        }

        private void UnblockUser(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<UnblockUser>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            packet.Username = Sanitize.Username(packet.Username);
            if (packet.Username == null)
            {
                Disconnect(client);
                return;
            }

            client.UnblockUser(packet.Username);
        }

        private void AddContact(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<AddContact>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            packet.Username = Sanitize.Username(packet.Username);
            if (packet.Username == null)
            {
                Disconnect(client);
                return;
            }

            client.AddContact(packet.Username);
        }

        private void DeleteContact(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<DeleteContact>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            packet.Username = Sanitize.Username(packet.Username);
            if (packet.Username == null)
            {
                Disconnect(client);
                return;
            }

            client.DeleteContact(packet.Username);
        }

        private void PrivateMessage(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<PrivateMessage>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            packet.Username = Sanitize.Username(packet.Username);
            packet.Guid = Sanitize.Hash(packet.Guid);
            if (packet.Username == null || packet.Guid == null || packet.Message.Length > 2000)
            {
                Disconnect(client);
                return;
            }

            client.PrivateMessageResult(packet.Guid);
        }

        private void JoinRoom(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<JoinRoom>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            client.JoinRoom(packet.RoomId);
        }

        private void LeaveRoom(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<LeaveRoom>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            client.LeaveRoom();
        }

        private void PublicMessage(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<PublicMessage>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            packet.Guid = Sanitize.Hash(packet.Guid);
            if (packet.Guid == null || packet.Message.Length > 2000)
            {
                Disconnect(client);
                return;
            }

            client.PublicMessageResult(packet.Guid, packet.Message);
        }

        private void GetProfilePicture(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<GetProfilePicture>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            packet.Username = Sanitize.Username(packet.Username);
            if (packet.Username == null)
            {
                Disconnect(client);
                return;
            }

            client.GetProfilePicture(packet.Username);
        }

        private void SetProfilePicture(Client client, string buffer)
        {
            if (!client.IsLoggedIn)
            {
                Disconnect(client);
                return;
            }

            var packet = Incoming.Parse<SetProfilePicture>(buffer);
            if (packet == null)
            {
                Disconnect(client);
                return;
            }

            client.SetProfilePicture(packet.Image);
        }
    }
}