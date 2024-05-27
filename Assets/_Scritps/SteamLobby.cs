using UnityEngine;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{

    [SerializeField] private GameObject hostButton;

    private NetworkManager _networkManager;

    protected Callback<LobbyCreated_t> _lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> _gameLobbyEnter;

    private const string HOSTADRESSKEY = "HostAdress";


    private void Start()
    {
        _networkManager = GetComponent<NetworkManager>();

        if(!SteamManager.Initialized)
        {
            return;
        }

        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        _gameLobbyEnter = Callback<LobbyEnter_t>.Create(OnGameLobbyEnter);
    }


    public void HostLobby()
    {

        hostButton.SetActive(false);

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);

    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            hostButton.SetActive(true);
            return;
        }

        _networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby),
            HOSTADRESSKEY, SteamUser.GetSteamID().ToString());

    }
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnGameLobbyEnter(LobbyEnter_t callback)
    {

        if(NetworkServer.active)
        {
            return;
        }

        string hostAdress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby)
            ,HOSTADRESSKEY);

        _networkManager.networkAddress = hostAdress;
        _networkManager.StartClient();


        hostButton.SetActive(false);

    }


}
