namespace Battle.Attack
{
    public interface IAttackHittable
    {
        public void OnAttacked(AttackHitController attackHitController);

        public OwnerType GetOwnerType();
    }
}