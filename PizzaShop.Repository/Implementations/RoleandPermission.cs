using Microsoft.EntityFrameworkCore;
using PizzaShop.Repository.Data;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class RoleandPermission : IRoleandPermission{

    private readonly PizzaShopDbContext _context;

    public RoleandPermission(PizzaShopDbContext context){
        _context = context;
    }

    public Role GetRolebyId(int id){

        Role role = _context.Roles.FirstOrDefault(r => r.RoleId == id);

        if(role == null){
        return new Role{};
        }

        return role;
    }

    public IQueryable<Permission> GetPermissionsByRole(int roleid){
        var permissions = _context.Permissions.Include(p => p.Permissionfield).Where(p => p.RoleId == roleid);

        return permissions;
    }

    public void UpdatingPermissions(IEnumerable<Permission> permissions){

        foreach(var permission in permissions){
            _context.Permissions.Update(permission);
        }
        _context.SaveChanges();

    }
}