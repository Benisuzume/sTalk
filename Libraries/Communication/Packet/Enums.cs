namespace sTalk.Libraries.Communication.Packet
{
    /// <summary>
    /// شناسه بسته های ارسالی و دریافتی
    /// </summary>
    public enum Id
    {
        Handshake,
        LogIn,
        LogOut,
        BlockUser,
        UnblockUser,
        AddContact,
        DeleteContact,
        PrivateMessage,
        PrivateMessageResult,
        JoinRoom,
        LeaveRoom,
        PublicMessage,
        PublicMessageResult,
        GetProfilePicture,
        SetProfilePicture,
        UserStatusChanged,
        UserJoinedRoom,
        UserLeftRoom
    }

    /// <summary>
    /// نتیجه درخواست ها
    /// </summary>
    public enum Result
    {
        Failure,
        Success,
        Useless,
        Pending,
        NotFound,
        Banned,
        Wrong,
        Full
    }

    /// <summary>
    /// وضعیت کاربر
    /// </summary>
    public enum Status
    {
        Online,
        Offline
    }
}