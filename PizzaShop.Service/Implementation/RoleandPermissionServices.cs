using PizzaShop.Repository.Data;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementation;

public class RoleandPermissionServices : IRoleandPermissionServices{

    private IRoleandPermission _roleandpermission ;

    public RoleandPermissionServices(IRoleandPermission roleandPermission){
        _roleandpermission = roleandPermission;
    }

    public Role GetRoleByIdService(int id){
        Role role = _roleandpermission.GetRolebyId(id);

        if(role == null){
        return new Role{};
        }

        return role;
    }

    public IQueryable<Permission> GetPermissionsByRoleService(int roleid){
        var permissions = _roleandpermission.GetPermissionsByRole(roleid);

        return permissions;
    }

    public void UpdatePermissionsService(IEnumerable<Permission> permissions,int id){
        var datetime = DateTime.Now;

        foreach(var permission in permissions){
            permission.Updatedby = id;
            permission.Updatedat = datetime;
        }

        _roleandpermission.UpdatingPermissions(permissions);
    }

}