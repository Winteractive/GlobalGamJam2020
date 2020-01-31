using System;



public interface INetworkService
{
    /// <summary>
    /// Is called when a Metadata is changed or added for a user
    /// </summary>
    event OnMember OnMemberUpdate;
    /// <summary>
    /// Is called when a user joins the lobby
    /// </summary>
    event OnMember OnMemberJoin;
    /// <summary>
    /// Is called when a User disconnects with the lobby
    /// </summary>
    event OnMember OnMemberDisconnect;

    /// <summary>
    /// Is called when a MetaData is changed or added for the Lobby
    /// </summary>
    event OnLobby OnLobbyUpdate;

    /// <summary>
    /// Is Called when we get a NetworkMessage
    /// </summary>
    event OnMessageReceived OnNetworkMessage;

    /// <summary>
    /// Host a Lobby
    /// </summary>
    void CreateLobby();

    /// <summary>
    /// Join a existing Lobby
    /// </summary>
    void JoinLobby(long lobbyId, string secret);
    /// <summary>
    /// Leave current Lobby
    /// </summary>
    void LeaveLobby();

    /// <summary>
    /// Send NetworkMeassage to current Lobby Host
    /// </summary>
    /// <param name="channelName"></param>
    /// <param name="data"></param>
    void SendMessageToHost(in ChannelName channelName, in byte[] data);
    /// <summary>
    /// Send NetworkMeassage to all other Clients
    /// </summary>
    /// <param name="channelName"></param>
    /// <param name="data"></param>
    void SendMessageToClients(in ChannelName channelName, in byte[] data);

    /// <summary>
    /// Get Local User Id
    /// </summary>
    /// <returns></returns>
    long GetLocalUserId();

    /// <summary>
    /// Set Metadata for Local User
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    void SetLocalMemberMetaData(string key, string data);
    
    /// <summary>
    /// Set Metadata for a User
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="key"></param>
    /// <param name="data"></param>
    void SetMemberMetaData(long userId,string key, string data);

    /// <summary>
    /// Set Metadata for the lobby
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    void SetLobbyMetaData(string key, string data);

    /// <summary>
    /// Retrive MetaData from Loacal User
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string GetLocalMemberMetaData(string key);
    /// <summary>
    /// Retrive MetaData from User
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    string GetMemberMetaData(long userId, string key);
    /// <summary>
    /// Retrive MetaData from Lobby
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    string GetLobbyMetaData(string key);
}
public enum ChannelName
{
    Character_Select,
    Character_Deselect,
    Character_Select_Index,
    Move_Input,
}

public delegate void OnMember(long userId);
public delegate void OnLobby();
public delegate void OnMessageReceived(ChannelName channelName, byte[] data);