public interface IEntityBuilder : IBuilder
{
    public void BuildEntityView(EntityView baseObject);

    public Entity GetResult();
}
