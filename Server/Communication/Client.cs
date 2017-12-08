using sTalk.Libraries.Communication.Packet;
using sTalk.Libraries.Communication.Packet.Data;
using sTalk.Libraries.Encryption;
using sTalk.Server.Communication.Packets;
using sTalk.Server.Data;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace sTalk.Server.Communication
{
    public class Client
    {
        private readonly Socket _socket;
        private readonly byte[] _buffer;
        private readonly AsyncCallback _sendCallback;
        private readonly string _token;

        private States _state;
        private Symmetric _aes;
        private Account _account;
        private Room _room;

        public Client(Socket socket, int bufferSize, AsyncCallback sendCallback)
        {
            _socket = socket;
            _buffer = new byte[bufferSize];
            _sendCallback = sendCallback;
            _token = Guid.NewGuid().ToString("N");

            _state = States.Connected;
        }

        public enum States
        {
            Connected,
            Disconnecting,
            Initializing,
            Initialized,
            Authenticating,
            Authenticated
        }

        public Socket Socket
        {
            get
            {
                return _socket;
            }
        }

        public byte[] Buffer
        {
            get
            {
                return _buffer;
            }
        }

        public States State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return _state == States.Authenticated;
            }
        }

        public bool IsInRoom
        {
            get
            {
                return IsLoggedIn && _room != null;
            }
        }

        public User User
        {
            get
            {
                return IsLoggedIn ? (User)_account : null;
            }
        }

        public Room Room
        {
            get
            {
                return IsInRoom ? _room : null;
            }
        }

        public string Decrypt(byte[] cipherText)
        {
            return _aes.Decrypt(cipherText);
        }

        public void Handshake(byte[] key, byte[] iv, string username)
        {
            _aes = new Symmetric(key, iv);
            _account = Accounts.Get(username);

            Result result;
            string token;

            if (_account == null)
            {
                _state = States.Disconnecting;

                result = Result.NotFound;
                token = null;
            }
            else if (_account.IsBanned)
            {
                _state = States.Disconnecting;

                result = Result.Banned;
                token = null;
            }
            else
            {
                _state = States.Initializing;

                result = Result.Success;
                token = _token;
            }

            var json = Outgoing.Handshake(result, token);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void LogIn(string hash)
        {
            var correctHash = Hash.CalculateMD5(_token + _account.Password);

            Result result;
            List<Room> rooms;
            List<User> contacts;
            List<string> blocks;
            List<OfflineMessage> offlineMessages;

            if (hash == correctHash)
            {
                _state = States.Authenticating;

                result = Result.Success;
                rooms = Rooms.Get();
                contacts = Contacts.GetContacts(_account.Id);
                blocks = Contacts.GetBlocks(_account.Id);
                offlineMessages = null; // todo: fill this with actual data
            }
            else
            {
                _state = States.Disconnecting;

                result = Result.Wrong;
                rooms = null;
                contacts = null;
                blocks = null;
                offlineMessages = null;
            }

            var json = Outgoing.LogIn(result, rooms, contacts, blocks, offlineMessages);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void LogOut()
        {
            var json = Outgoing.LogOut();
            var buffer = _aes.Encrypt(json);

            _state = States.Disconnecting;
            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void BlockUser(string username)
        {
            var account = Accounts.Get(username);

            Result result;

            if (account == null || account.Id == User.Id)
            {
                result = Result.NotFound;
            }
            else
            {
                if (Contacts.IsBlocked(User.Id, account.Id))
                {
                    result = Result.Useless;
                }
                else
                {
                    Contacts.Block(User.Id, account.Id);

                    result = Result.Success;
                }
            }

            var json = Outgoing.BlockUser(result);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void UnblockUser(string username)
        {
            var account = Accounts.Get(username);

            Result result;

            if (account == null || account.Id == User.Id)
            {
                result = Result.NotFound;
            }
            else
            {
                if (Contacts.IsBlocked(User.Id, account.Id))
                {
                    Contacts.Unblock(User.Id, account.Id);

                    result = Result.Success;
                }
                else
                {
                    result = Result.Useless;
                }
            }

            var json = Outgoing.UnblockUser(result);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void AddContact(string username)
        {
            var account = Accounts.Get(username);

            Result result;
            User user;

            if (account == null || account.Id == User.Id)
            {
                result = Result.NotFound;
                user = null;
            }
            else
            {
                if (Contacts.IsBlocked(account.Id, User.Id))
                {
                    result = Result.Banned;
                    user = null;
                }
                else
                {
                    if (Contacts.IsContact(User.Id, account.Id))
                    {
                        result = Result.Useless;
                        user = null;
                    }
                    else
                    {
                        Contacts.Add(User.Id, account.Id);

                        result = Result.Success;
                        user = (User)account;
                        user.Status = Server.IsOnline(user) ? Status.Online : Status.Offline;
                    }
                }
            }

            var json = Outgoing.AddContact(result, user);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void DeleteContact(string username)
        {
            var account = Accounts.Get(username);

            Result result;

            if (account == null || account.Id == User.Id)
            {
                result = Result.NotFound;
            }
            else
            {
                if (Contacts.IsContact(User.Id, account.Id))
                {
                    Contacts.Delete(User.Id, account.Id);

                    result = Result.Success;
                }
                else
                {
                    result = Result.Useless;
                }
            }

            var json = Outgoing.DeleteContact(result);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void PrivateMessage(string username, string message)
        {
            var json = Outgoing.PrivateMessage(username, message);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void PrivateMessageResult(string guid)
        {
            var result = Result.Success;

            var json = Outgoing.PrivateMessageResult(result, guid);
            var buffer = _aes.Encrypt(json);

            // _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void JoinRoom(uint roomId)
        {
            Result result;
            string message;
            List<string> userList;

            if (IsInRoom)
            {
                result = Result.Useless;
                message = null;
                userList = null;
            }
            else
            {
                bool banned = Rooms.IsUserBanned(User.Id, roomId)
;
                if (banned)
                {
                    result = Result.Banned;
                    message = null;
                    userList = null;
                }
                else
                {
                    var room = Rooms.Get(roomId);

                    if (room == null)
                    {
                        result = Result.NotFound;
                        message = null;
                        userList = null;
                    }
                    else
                    {
                        var users = Server.GetRoomUsers(room);

                        if (users.Count < room.Capacity)
                        {
                            _room = room;
                            Server.UserJoinedRoom(_room, User);

                            result = Result.Success;
                            message = room.Message;
                            userList = users;
                        }
                        else
                        {
                            result = Result.Full;
                            message = null;
                            userList = null;
                        }
                    }
                }
            }

            var json = Outgoing.JoinRoom(result, message, userList);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void LeaveRoom()
        {
            Result result;

            if (IsInRoom)
            {
                Server.UserLeftRoom(_room, User);
                _room = null;

                result = Result.Success;
            }
            else
            {
                result = Result.Useless;
            }

            var json = Outgoing.LeaveRoom(result);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void PublicMessage(string username, string message)
        {
            var json = Outgoing.PublicMessage(username, message);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void PublicMessageResult(string guid, string message)
        {
            Result result;

            if (!IsInRoom)
            {
                result = Result.Useless;
            }
            else if (false /* banned */)
            {
                // Chat blocked...
                // result = Result.Banned;
            }
            else
            {
                Server.PublicMessage(_room, User, message);

                result = Result.Success;
            }

            var json = Outgoing.PublicMessageResult(result, guid);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void GetProfilePicture(string username)
        {
            var account = Accounts.Get(username);

            var result = Result.Success;
            var image = new byte[0];

            var json = Outgoing.GetProfilePicture(result, username, image);
            var buffer = _aes.Encrypt(json);

            // _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void SetProfilePicture(byte[] image)
        {
            // todo: different results
            // todo: room's system message

            var json = Outgoing.SetProfilePicture();
            var buffer = _aes.Encrypt(json);

            // _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void StatusChanged(string username, Status status)
        {
            var json = Outgoing.UserStatusChanged(username, status);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void UserJoinedRoom(string username)
        {
            var json = Outgoing.UserJoinedRoom(username);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }

        public void UserLeftRoom(string username)
        {
            var json = Outgoing.UserLeftRoom(username);
            var buffer = _aes.Encrypt(json);

            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, _sendCallback, this);
        }
    }
}