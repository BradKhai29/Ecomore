using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementation;

public class RoleRepository :
    BaseIdentityRepository<RoleEntity>,
    IRoleRepository
{
    private readonly RoleManager<RoleEntity> _roleManager;

    public RoleRepository(
        DbContext dbContext,
        RoleManager<RoleEntity> roleManager) : base(dbContext)
    {
        _roleManager = roleManager;
    }

    public override Task<IdentityResult> AddAsync(RoleEntity newEntity)
    {
        return _roleManager.CreateAsync(newEntity);
    }

    public override Task<IdentityResult> UpdateAsync(RoleEntity newEntity)
    {
        return _roleManager.UpdateAsync(newEntity);
    }

    public override async Task<IdentityResult> RemoveAsync(Guid id)
    {
        var foundRole = await _roleManager.FindByIdAsync(id.ToString());

        return await _roleManager.DeleteAsync(foundRole);
    }

    public override Task<RoleEntity> FindByIdAsync(Guid id)
    {
        return _roleManager.FindByIdAsync(id.ToString());
    }
    public override Task<RoleEntity> FindByNameAsync(string name)
    {
        return _roleManager.FindByNameAsync(name);
    }
}
