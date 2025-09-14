using UnityEngine;

public interface IEntityBuilder : IBuilder
{
    public void BuildEntityView(BaseEnemy baseObject);

    public Entity GetResult();
}
