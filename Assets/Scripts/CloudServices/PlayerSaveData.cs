namespace CloudServices
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public string id;
        public bool[] unlockedHats;

        public PlayerSaveData(string id, bool[] unlockedHats)
        {
            this.id = id;
            this.unlockedHats = unlockedHats;
        }
    }
}
