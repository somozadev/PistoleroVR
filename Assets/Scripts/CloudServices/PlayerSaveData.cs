namespace CloudServices
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public string id;
        public string username;
        public bool[] unlockedHats;
        public int kills;
        public int deaths;

        public PlayerSaveData()
        {
            id = "NaN";
            username = "NaN";
            this.unlockedHats = new bool[0];
            this.kills = 0;
            this.deaths = 0;
        }
        public PlayerSaveData(string id)
        {
            this.id = id;
            this.unlockedHats = new bool[0];
            this.username = id;
            this.kills = 0;
            this.deaths = 0;
        }
        public PlayerSaveData(string id, bool[] unlockedHats)
        {
            this.id = id;
            this.unlockedHats = unlockedHats;
            this.username = id;
            this.kills = 0;
            this.deaths = 0;
        }

        public PlayerSaveData(string id, string username, bool[] unlockedHats)
        {
            this.id = id;
            this.unlockedHats = unlockedHats;
            this.username = username;
            this.kills = 0;
            this.deaths = 0;
        }

        public PlayerSaveData(string id, string username, bool[] unlockedHats, int kills, int deaths)
        {
            this.id = id;
            this.unlockedHats = unlockedHats;
            this.username = username;
            this.kills = kills;
            this.deaths = deaths;
        }

        public override string ToString()
        {
            string str = id + "/" + username + "/" + kills + "/" + deaths + "/" + unlockedHats.Length;
            return str;
        }
    }
}