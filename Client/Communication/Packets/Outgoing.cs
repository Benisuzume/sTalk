using Newtonsoft.Json;
using sTalk.Libraries.Communication.Packet;
using sTalk.Libraries.Communication.Packet.Client;

namespace sTalk.Client.Communication.Packets
{
    public static class Outgoing
    {
        public static string Handshake(byte[] key, byte[] iv, string username)
        {
            var packet = new Handshake()
            {
                Id = Id.Handshake,
                Key = key,
                IV = iv,
                Username = username
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string LogIn(string hash)
        {
            var packet = new LogIn()
            {
                Id = Id.LogIn,
                Hash = hash
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

        public static string BlockUser(string username)
        {
            var packet = new BlockUser()
            {
                Id = Id.BlockUser,
                Username = username
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string UnblockUser(string username)
        {
            var packet = new UnblockUser()
            {
                Id = Id.UnblockUser,
                Username = username
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string AddContact(string username)
        {
            var packet = new AddContact()
            {
                Id = Id.AddContact,
                Username = username
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string DeleteContact(string username)
        {
            var packet = new DeleteContact()
            {
                Id = Id.DeleteContact,
                Username = username
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string PrivateMessage(string guid, string username, string message)
        {
            var packet = new PrivateMessage()
            {
                Id = Id.PrivateMessage,
                Guid = guid,
                Username = username,
                Message = message
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string JoinRoom(uint roomId)
        {
            var packet = new JoinRoom()
            {
                Id = Id.JoinRoom,
                RoomId = roomId
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string LeaveRoom()
        {
            var packet = new LeaveRoom()
            {
                Id = Id.LeaveRoom
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string PublicMessage(string guid, string message)
        {
            var packet = new PublicMessage()
            {
                Id = Id.PublicMessage,
                Guid = guid,
                Message = message
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string GetProfilePicture(string username)
        {
            var packet = new GetProfilePicture()
            {
                Id = Id.GetProfilePicture,
                Username = username
            };

            return JsonConvert.SerializeObject(packet);
        }

        public static string SetProfilePicture(byte[] image)
        {
            var packet = new SetProfilePicture()
            {
                Id = Id.SetProfilePicture,
                Image = image
            };

            return JsonConvert.SerializeObject(packet);
        }
    }
}