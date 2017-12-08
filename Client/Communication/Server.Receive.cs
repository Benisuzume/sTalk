using sTalk.Client.Communication.Packets;
using sTalk.Libraries.Communication.Packet;
using sTalk.Libraries.Communication.Packet.Server;

namespace sTalk.Client.Communication
{
    public partial class Server
    {
        private Identifier ReceiveIdentifier(string buffer)
        {
            return Incoming.Parse<Identifier>(buffer);
        }

        private void ReceiveHandshake(string buffer)
        {
            var packet = Incoming.Parse<Handshake>(buffer);

            if (_state != States.Initialized || packet == null)
            {
                Disconnect(false);
                _logInDelegate(Result.Failure, null, null, null);
                return;
            }

            if (packet.Result != Result.Success)
            {
                Disconnect(false);
                _logInDelegate(packet.Result, null, null, null);
                return;
            }

            _token = packet.Token;

            SendLogIn();
        }

        private void ReceiveLogIn(string buffer)
        {
            var packet = Incoming.Parse<LogIn>(buffer);

            if (_state != States.Authenticated || packet == null)
            {
                Disconnect(false);
                _logInDelegate(Result.Failure, null, null, null);
                return;
            }

            if (packet.Result != Result.Success)
            {
                Disconnect(false);
                _logInDelegate(packet.Result, null, null, null);
                return;
            }

            IsLoggedIn = true;
            _logInDelegate(Result.Success, packet.Rooms, packet.Contacts, packet.Blocks);
        }

        private void ReceiveLogOut(string buffer)
        {
            Disconnect(true);
        }

        private void ReceiveBlockUser(string buffer)
        {
            var packet = Incoming.Parse<BlockUser>(buffer);

            if (!IsLoggedIn || packet == null)
            {
                _blockUserDelegate(Result.Failure);
                return;
            }

            _blockUserDelegate(packet.Result);
        }

        private void ReceiveUnblockUser(string buffer)
        {
            var packet = Incoming.Parse<UnblockUser>(buffer);

            if (!IsLoggedIn || packet == null)
            {
                _unblockUserDelegate(Result.Failure);
                return;
            }

            _unblockUserDelegate(packet.Result);
        }

        private void ReceiveAddContact(string buffer)
        {
            var packet = Incoming.Parse<AddContact>(buffer);

            if (!IsLoggedIn || packet == null)
            {
                _addContactDelegate(Result.Failure, null);
                return;
            }

            _addContactDelegate(packet.Result, packet.User);
        }

        private void ReceiveDeleteContact(string buffer)
        {
            var packet = Incoming.Parse<DeleteContact>(buffer);

            if (!IsLoggedIn || packet == null)
            {
                _deleteContactDelegate(Result.Failure);
                return;
            }

            _deleteContactDelegate(packet.Result);
        }

        private void ReceivePrivateMessage(string buffer)
        {
            var packet = Incoming.Parse<PrivateMessage>(buffer);

            if (!IsLoggedIn || packet == null)
                return;

            PrivateMessageReceived(packet.Username, packet.Message);
        }

        private void ReceivePrivateMessageResult(string buffer)
        {
            var packet = Incoming.Parse<PrivateMessageResult>(buffer);

            if (!IsLoggedIn || packet == null)
                return;

            PrivateMessageResultReceived(packet.Result, packet.Guid);
        }

        private void ReceiveJoinRoom(string buffer)
        {
            var packet = Incoming.Parse<JoinRoom>(buffer);

            if (!IsLoggedIn || packet == null)
            {
                _joinRoomDelegate(Result.Failure, null, null);
                return;
            }

            if (packet.Result == Result.Success)
                IsInRoom = true;

            _joinRoomDelegate(packet.Result, packet.Message, packet.UserList);
        }

        private void ReceiveLeaveRoom(string buffer)
        {
            var packet = Incoming.Parse<LeaveRoom>(buffer);

            if (!IsInRoom || packet == null)
            {
                _leaveRoomDelegate(Result.Failure);
                return;
            }

            if (packet.Result == Result.Success)
                IsInRoom = false;

            _leaveRoomDelegate(packet.Result);
        }

        private void ReceivePublicMessage(string buffer)
        {
            var packet = Incoming.Parse<PublicMessage>(buffer);

            if (!IsInRoom || packet == null)
                return;

            PublicMessageReceived(packet.Username, packet.Message);
        }

        private void ReceivePublicMessageResult(string buffer)
        {
            var packet = Incoming.Parse<PublicMessageResult>(buffer);

            if (!IsInRoom || packet == null)
                return;

            PublicMessageResultReceived(packet.Result, packet.Guid);
        }

        private void ReceiveGetProfilePicture(string buffer)
        {
            var packet = Incoming.Parse<GetProfilePicture>(buffer);

            if (!IsLoggedIn || packet == null)
                return;

            ProfilePictureReceived(packet.Result, packet.Username, packet.Image);
        }

        private void ReceiveSetProfilePicture(string buffer)
        {
            var packet = Incoming.Parse<SetProfilePicture>(buffer);

            if (!IsLoggedIn || packet == null)
            {
                _setProfilePictureDelegate(Result.Failure);
                return;
            }

            _setProfilePictureDelegate(packet.Result);
        }

        private void ReceiveUserStatusChanged(string buffer)
        {
            var packet = Incoming.Parse<UserStatusChanged>(buffer);

            if (!IsLoggedIn || packet == null)
                return;

            UserStatusChanged(packet.Username, packet.Status);
        }

        private void ReceiveUserJoinedRoom(string buffer)
        {
            var packet = Incoming.Parse<UserJoinedRoom>(buffer);

            if (!IsInRoom || packet == null)
                return;

            UserJoinedRoom(packet.Username);
        }

        private void ReceiveUserLeftRoom(string buffer)
        {
            var packet = Incoming.Parse<UserLeftRoom>(buffer);

            if (!IsInRoom || packet == null)
                return;

            UserLeftRoom(packet.Username);
        }
    }
}