using Newtonsoft.Json;
using sTalk.Libraries.Communication.Packet;
using sTalk.Libraries.Communication.Packet.Data;
using sTalk.Libraries.Communication.Packet.Server;
using System.Collections.Generic;

namespace sTalk.Server.Communication.Packets
{
    public static class Outgoing
    {
        public static string Handshake(Result result, string token)
        {
            var packet = new Handshake()
            {
                Id = Id.Handshake,
                Result = result,
                Token = token
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string LogIn(Result result, List<Room> rooms, List<User> contacts, List<string> blocks, List<OfflineMessage> offlineMessages)
        {
            var packet = new LogIn()
            {
                Id = Id.LogIn,
                Result = result,
                Rooms = rooms,
                Contacts = contacts,
                Blocks = blocks,
                OfflineMessages = offlineMessages
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string LogOut()
        {
            var packet = new LogOut()
            {
                Id = Id.LogOut
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string BlockUser(Result result)
        {
            var packet = new BlockUser()
            {
                Id = Id.BlockUser,
                Result = result
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string UnblockUser(Result result)
        {
            var packet = new UnblockUser()
            {
                Id = Id.UnblockUser,
                Result = result
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string AddContact(Result result, User user)
        {
            var packet = new AddContact()
            {
                Id = Id.AddContact,
                Result = result,
                User = user
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string DeleteContact(Result result)
        {
            var packet = new DeleteContact()
            {
                Id = Id.DeleteContact,
                Result = result
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string PrivateMessage(string username, string message)
        {
            var packet = new PrivateMessage()
            {
                Id = Id.PrivateMessage,
                Username = username,
                Message = message
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string PrivateMessageResult(Result result, string guid)
        {
            var packet = new PrivateMessageResult()
            {
                Id = Id.PrivateMessageResult,
                Result = result,
                Guid = guid
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string JoinRoom(Result result, string message, List<string> userList)
        {
            var packet = new JoinRoom()
            {
                Id = Id.JoinRoom,
                Result = result,
                Message = message,
                UserList = userList
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string LeaveRoom(Result result)
        {
            var packet = new LeaveRoom()
            {
                Id = Id.LeaveRoom,
                Result = result
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string PublicMessage(string username, string message)
        {
            var packet = new PublicMessage()
            {
                Id = Id.PublicMessage,
                Username = username,
                Message = message
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string PublicMessageResult(Result result, string guid)
        {
            var packet = new PublicMessageResult()
            {
                Id = Id.PublicMessageResult,
                Result = result,
                Guid = guid
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string GetProfilePicture(Result result, string username, byte[] image)
        {
            var packet = new GetProfilePicture()
            {
                Id = Id.GetProfilePicture,
                Result = result,
                Username = username,
                Image = image
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string SetProfilePicture()
        {
            var packet = new SetProfilePicture()
            {
                Id = Id.SetProfilePicture
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string UserStatusChanged(string username, Status status)
        {
            var packet = new UserStatusChanged()
            {
                Id = Id.UserStatusChanged,
                Username = username,
                Status = status
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string UserJoinedRoom(string username)
        {
            var packet = new UserJoinedRoom()
            {
                Id = Id.UserJoinedRoom,
                Username = username
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string UserLeftRoom(string username)
        {
            var packet = new UserLeftRoom()
            {
                Id = Id.UserLeftRoom,
                Username = username
            };

            return JsonConvert.SerializeObject(packet);
        }
    }
}