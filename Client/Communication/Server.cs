using sTalk.Libraries.Communication.Packet;
using sTalk.Libraries.Communication.Packet.Data;
using sTalk.Libraries.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace sTalk.Client.Communication
{
    public partial class Server
    {
        private const int BufferSize = 10240;

        private readonly IPEndPoint _remoteEP;
        private Socket _socket;
        private byte[] _buffer;

        private States _state;
        private string _username;
        private string _password;
        private string _token;

        private Asymmetric _rsa;
        private Symmetric _aes;

        private Action<Result, List<Room>, List<User>, List<string>> _logInDelegate;
        private Action<Result> _blockUserDelegate;
        private Action<Result> _unblockUserDelegate;
        private Action<Result, User> _addContactDelegate;
        private Action<Result> _deleteContactDelegate;
        private Action<Result, string, List<string>> _joinRoomDelegate;
        private Action<Result> _leaveRoomDelegate;
        private Action<Result> _setProfilePictureDelegate;

        public Server(string publicKey, string ipAddress, int port)
        {
            try
            {
                _remoteEP = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                _rsa = new Asymmetric(publicKey);
            }
            catch
            {
                throw;
            }
        }

        public event Action Disconnected;
        public event Action<string, string> PrivateMessageReceived;
        public event Action<Result, string> PrivateMessageResultReceived;
        public event Action<string, string> PublicMessageReceived;
        public event Action<Result, string> PublicMessageResultReceived;
        public event Action<Result, string, byte[]> ProfilePictureReceived;
        public event Action<string, Status> UserStatusChanged;
        public event Action<string> UserJoinedRoom;
        public event Action<string> UserLeftRoom;

        public enum States
        {
            Initializing,
            Initialized,
            Authenticating,
            Authenticated,
        }

        public bool IsLoggedIn { get; private set; }
        public bool IsInRoom { get; private set; }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                _socket.EndConnect(ar);

                SendHandshake();
                _socket.BeginReceive(_buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, null);
            }
            catch
            {
                _socket.Close();

                _logInDelegate(Result.Failure, null, null, null);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!_socket.Connected)
            {
                Disconnect(true);
                return;
            }

            var bytesRead = 0;
            try
            {
                bytesRead = _socket.EndReceive(ar);
            }
            catch
            {
                Disconnect(true);
                return;
            }

            if (bytesRead == 0)
            {
                Disconnect(true);
                return;
            }

            string buffer = string.Empty;
            Identifier packet = null;

            try
            {
                buffer = _aes.Decrypt(_buffer.Skip(0).Take(bytesRead).ToArray());

                packet = ReceiveIdentifier(buffer);
                if (packet == null)
                {
                    Disconnect(true);
                    return;
                }
            }
            catch
            {
                Disconnect(true);
                return;
            }

            _socket.BeginReceive(_buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, null);

            switch (packet.Id)
            {
                case Id.Handshake:
                    ReceiveHandshake(buffer);
                    break;

                case Id.LogIn:
                    ReceiveLogIn(buffer);
                    break;

                case Id.LogOut:
                    ReceiveLogOut(buffer);
                    break;

                case Id.BlockUser:
                    ReceiveBlockUser(buffer);
                    break;

                case Id.UnblockUser:
                    ReceiveUnblockUser(buffer);
                    break;

                case Id.AddContact:
                    ReceiveAddContact(buffer);
                    break;

                case Id.DeleteContact:
                    ReceiveDeleteContact(buffer);
                    break;

                case Id.PrivateMessage:
                    ReceivePrivateMessage(buffer);
                    break;

                case Id.PrivateMessageResult:
                    ReceivePrivateMessageResult(buffer);
                    break;

                case Id.JoinRoom:
                    ReceiveJoinRoom(buffer);
                    break;

                case Id.LeaveRoom:
                    ReceiveLeaveRoom(buffer);
                    break;

                case Id.PublicMessage:
                    ReceivePublicMessage(buffer);
                    break;

                case Id.PublicMessageResult:
                    ReceivePublicMessageResult(buffer);
                    break;

                case Id.GetProfilePicture:
                    ReceiveGetProfilePicture(buffer);
                    break;

                case Id.SetProfilePicture:
                    ReceiveSetProfilePicture(buffer);
                    break;

                case Id.UserStatusChanged:
                    ReceiveUserStatusChanged(buffer);
                    break;

                case Id.UserJoinedRoom:
                    ReceiveUserJoinedRoom(buffer);
                    break;

                case Id.UserLeftRoom:
                    ReceiveUserLeftRoom(buffer);
                    break;
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            if (!_socket.Connected)
            {
                Disconnect(true);
                return;
            }

            try
            {
                _socket.EndSend(ar);
            }
            catch
            {
                Disconnect(true);
                return;
            }

            switch (_state)
            {
                case States.Initializing:
                    _state = States.Initialized;
                    break;

                case States.Authenticating:
                    _state = States.Authenticated;
                    break;
            }
        }

        private void Disconnect(bool raiseEvent)
        {
            if (_socket.Connected)
                _socket.Shutdown(SocketShutdown.Both);

            _socket.Close();

            if (raiseEvent && Disconnected != null)
                Disconnected();

            IsLoggedIn = false;
        }

        public void LogIn(string username, string password, Action<Result, List<Room>, List<User>, List<string>> logInDelegate)
        {
            if (IsLoggedIn)
                return;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _buffer = new byte[BufferSize];
            _logInDelegate = logInDelegate;

            _username = username;
            _password = password;

            _socket.BeginConnect(_remoteEP, ConnectCallback, null);
        }

        public void LogOut()
        {
            if (!IsLoggedIn)
                return;

            SendLogOut();
        }

        public void BlockUser(string username, Action<Result> blockUserDelegate)
        {
            if (!IsLoggedIn)
                return;

            _blockUserDelegate = blockUserDelegate;
            SendBlockUser(username);
        }

        public void UnblockUser(string username, Action<Result> unblockUserDelegate)
        {
            if (!IsLoggedIn)
                return;

            _unblockUserDelegate = unblockUserDelegate;
            SendUnblockUser(username);
        }

        public void AddContact(string username, Action<Result, User> addContactDelegate)
        {
            if (!IsLoggedIn)
                return;

            _addContactDelegate = addContactDelegate;
            SendAddContact(username);
        }

        public void DeleteContact(string username, Action<Result> deleteContactDelegate)
        {
            if (!IsLoggedIn)
                return;

            _deleteContactDelegate = deleteContactDelegate;
            SendDeleteContact(username);
        }

        public void PrivateMessage(string guid, string username, string message)
        {
            if (!IsLoggedIn)
                return;

            SendPrivateMessage(guid, username, message);
        }

        public void JoinRoom(uint roomId, Action<Result, string, List<string>> joinRoomDelegate)
        {
            if (IsInRoom)
            {
                joinRoomDelegate(Result.Useless, null, null);
                return;
            }

            _joinRoomDelegate = joinRoomDelegate;
            SendJoinRoom(roomId);
        }

        public void LeaveRoom(Action<Result> leaveRoomDelegate)
        {
            if (!IsInRoom)
            {
                leaveRoomDelegate(Result.Useless);
                return;
            }

            _leaveRoomDelegate = leaveRoomDelegate;
            SendLeaveRoom();
        }

        public void PublicMessage(string guid, string message)
        {
            if (!IsInRoom)
                return;

            SendPublicMessage(guid, message);
        }
    }
}