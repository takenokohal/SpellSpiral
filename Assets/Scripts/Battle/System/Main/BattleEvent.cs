namespace Battle.System.Main
{
    public enum BattleEvent
    {
        //シーン読み込み、開幕演出
        SceneStart,

        //開幕演出終了通知
        BattleStart,

        Win,

        Lose
    }
}