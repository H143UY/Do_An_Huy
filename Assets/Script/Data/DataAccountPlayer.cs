
public class DataAccountPlayer
{
    private static PlayerData _playerdata;
    public static PlayerData PlayerData
    {
        get
        {
            if (_playerdata != null)
            {
                return _playerdata;
            }
            var playerdata = new PlayerData();
            _playerdata = ES3.Load(DataAccountPlayerConstains.PlayerData, playerdata);
            return playerdata;
        }
    }
    public static void SaveDataPlayerData()
    {
        ES3.Save(DataAccountPlayerConstains.PlayerData, _playerdata);
    }
}
