using System;
namespace SBCM
{
	public class Player
	{
		public string Name { get; }
		public string PasswordHash { get; }

		public int ShotsHit { get; set; }
        public int ShotsTaken { get; set; }

        public int PersonalKills { get; set; }
		public int PersonalLosses { get; set; }
		public int PersonalFratricides { get; set; }

		public int UnitKills_Vehicles { get; set; }
        public int UnitLosses_Tanks { get; set; }
        public int UnitLosses_PCs { get; set; }
        public int UnitLosses_Personnel { get; set; }
        public int UnitLosses_Helicopters { get; set; }
        public int UnitLosses_Trucks { get; set; }

        public Player(string name)
		{
			Name = name;
			PasswordHash = ""; // TODO

			ShotsTaken = ShotsHit = 0;

			PersonalKills = PersonalLosses = PersonalFratricides = 0;

			UnitKills_Vehicles = 0;
			UnitLosses_Tanks = 0;
			UnitLosses_PCs = 0;
			UnitLosses_Personnel = 0;
			UnitLosses_Helicopters = 0;
			UnitLosses_Trucks = 0;
        }

		public double GetHitPercentage()
		{
			if (ShotsTaken > 0)
			{
				return 100.0 * ShotsHit / (double)ShotsTaken;
			}

			return 0;
		}

        public double GetPersonalKD()
        {
			int losses = PersonalLosses + PersonalFratricides;

            if (losses > 0)
            {
                return PersonalKills / (double)losses;
            }

			if(PersonalKills > 0)
			{
				return PersonalKills;
			}

            return 0;
        }

        public double GetUnitKD()
        {
            int losses = UnitLosses_Tanks
				+ UnitLosses_PCs
				+ UnitLosses_Helicopters
				+ UnitLosses_Trucks;

            if (losses > 0)
            {
                return UnitKills_Vehicles / (double)losses;
            }

			if(UnitKills_Vehicles > 0)
			{
				return UnitKills_Vehicles;
			}

            return 0;
        }
    }
}