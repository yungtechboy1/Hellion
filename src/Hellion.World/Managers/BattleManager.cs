using Hellion.Core.Data.Headers;
using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Managers
{
    /// <summary>
    /// This <see cref="BattleManager"/> handles everything realated with the battles
    /// between players and monsters.
    /// It has some help methods used to calculate the damages and defense rate.
    /// </summary>
    public static class BattleManager
    {
        public static int CalculateDamages(Mover attacker, Mover defender)
        {
            var attackFlags = GetAttackFlags();

            if (attackFlags.HasFlag(AttackFlags.AF_MISS))
                return 0;
            if (attackFlags.HasFlag(AttackFlags.AF_GENERIC))
            {
                // normal attack
            }

            return 0;
        }

        public static AttackFlags GetAttackFlags()
        {
            AttackFlags attackFlags = 0;



            return attackFlags;
        }
    }
}
