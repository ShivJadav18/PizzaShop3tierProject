using PizzaShop.Repository.Data;

namespace PizzaShop.Repository.Interfaces;

public interface IRoleandPermission {

    public Role GetRolebyId(int id);

    public IQueryable<Permission> GetPermissionsByRole(int roleid);

    public void UpdatingPermissions(IEnumerable<Permission> permissions);

}