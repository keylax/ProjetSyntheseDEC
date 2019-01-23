namespace Assets.Scripts.GameObjectsBehavior
{
    public static class ObjectTags
    {
        public const string Player1Tag = "Player1";
        public const string Player2Tag = "Player2";
        public const string Player3Tag = "Player3";
        public const string Player4Tag = "Player4";

        public const string GoalTag = "Goal";
        public const string BallTag = "Ball";

        public const string BlueGoalie = "BlueGoalie";
        public const string RedGoalie = "RedGoalie";

        public const string AITag = "AI Player";

        public const string StunningProjectileTag = "StunningProjectile";

        public const string SpringTag = "SpringProjectile";

        public const string WallTag = "Wall";

        public const string Environment = "Environment";

        public const string EndlessHole = "EndlessHole";

        public const string ShipCollider = "Ship";

        public static bool IsPlayer(string _tag)
        {
            if (_tag == Player1Tag || _tag == Player2Tag || _tag == Player3Tag || _tag == Player4Tag || _tag == AITag)
            {
                return true;
            }
            return false;
        }
    }

}