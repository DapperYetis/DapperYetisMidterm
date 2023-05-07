[System.Serializable]
public struct GameStats
{
    // General
    public double timePlayed;
    public uint totalPoints;
    public uint deaths;
    public double distanceMoved;
    public ulong jumps;

    // Collection
    public ulong goldCollected;
    public uint itemsCollected;
    public uint purchasesMade;

    // Offense
    public double damageDealt;
    public uint criticalHits;
    public uint bossesKilled;

    // Defense
    public double damageTaken;
    public double damageHealed;


    public override string ToString()
    {
        return "Stats:\n\n" +
            $"Time Played: {timePlayed}\n" +
            $"Total Points: {totalPoints}\n" +
            $"Deaths: {deaths}\n" +
            $"Distance Moved: {distanceMoved}\n" +
            $"Jumps: {jumps}\n\n" +

            $"Gold Collected: {goldCollected}\n" +
            $"Items Collected: {itemsCollected}\n\n" +

            $"Damage Dealt: {damageDealt}\n" +
            $"Critical Hits: {criticalHits}\n" +
            $"Bosses Killed: {bossesKilled}\n\n" +

            $"Damage Taken: {damageTaken}\n" +
            $"Damage Healed: {damageHealed}";
    }
}