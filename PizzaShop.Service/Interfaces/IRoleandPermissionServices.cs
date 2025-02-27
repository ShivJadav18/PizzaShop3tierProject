using PizzaShop.Repository.Data;

namespace PizzaShop.Service.Interfaces;

public interface IRoleandPermissionServices{

    public Role GetRoleByIdService(int id);

    public IQueryable<Permission> GetPermissionsByRoleService(int roleid);

    public void UpdatePermissionsService(IEnumerable<Permission> permissions,int id);
}