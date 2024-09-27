using HostMaster.Shared.Entities;

namespace HostMaster.Backend.Data;

public class SeedDb
{
	private readonly DataContext _context;

	public SeedDb(DataContext context)
	{
		_context = context;
	}

	public async Task SeedAsync()
	{
		await _context.Database.EnsureCreatedAsync();
		await CheckCountriesAsync();
	}

	private async Task CheckCountriesAsync()
	{
		if (!_context.Countries.Any())
		{
			_context.Countries.Add(new Country
			{
				Name = "Colombia",
				States = new List<State>()
					{
						new State()
						{
							Name = "Antioquia",
							Cities = new List<City>()
							{
								new City() { Name = "Medellín" },
								new City() { Name = "Intagui" },
								new City() { Name = "Envigado" },
								new City() { Name = "Bello" },
								new City() { Name = "Rionegro" }
							}
						},
						new State()
						{
							Name = "Bogotá",
							Cities = new List<City>()
							{
								new City() { Name = "Usaquen" },
								new City() { Name = "Chapinero" },
								new City() { Name = "Santa fe" },
								new City() { Name = "Usme" },
								new City() { Name = "Bosa" }
							}
						}
					}
			});
		}

		await _context.SaveChangesAsync();
	}
}