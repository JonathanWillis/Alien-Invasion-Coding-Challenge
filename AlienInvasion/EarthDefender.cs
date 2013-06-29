using System.Collections.Generic;
using System.Linq;
using AlienInvasion.Client;
using AlienInvasion.Client.DefenceAssets;

namespace AlienInvasion
{
	public class EarthDefender : IEarthDefender
	{
	    private Armory _armory;

	    public DefenceStrategy DefendEarth(IAlienInvasionWave invasionWave)
		{
            if (_armory == null)
		        _armory = new Armory(invasionWave.WeaponsAvailableForDefence);

            _armory.ReloadWeapons();

            var weaponsToFire = new List<IDefenceWeapon>();
            for (var i = 0; i < invasionWave.AlienInvaders.Count(); i++)
                weaponsToFire.Add(_armory.GetWeapon());

		    return new DefenceStrategy(weaponsToFire);
		}
	}
}
