namespace SBCM {
    public class DamageState {
        public bool Destroyed { get; set; }
        public bool Immobilized { get; set; }
        public bool CasualtyCommander { get; set; }
        public bool CasualtyGunner { get; set; }
        public bool CasualtyLoader { get; set; }
        public bool CasualtyDriver { get; set; }
        public bool DamagedFCS { get; set; }
        public bool DamagedRadio { get; set; }
        public bool DamagedTurret { get; set; }

        public DamageState() {
            Destroyed = false;
            Immobilized = false;

            CasualtyCommander = false;
            CasualtyGunner = false;
            CasualtyLoader = false;
            CasualtyDriver = false;

            DamagedFCS = false;
            DamagedRadio = false;
            DamagedTurret = false;
        }

        public bool IsDamaged() {
            return Immobilized
                || CasualtyCommander
                || CasualtyGunner
                || CasualtyLoader
                || CasualtyDriver
                || DamagedFCS
                || DamagedRadio
                || DamagedTurret;
        }
    }
}

