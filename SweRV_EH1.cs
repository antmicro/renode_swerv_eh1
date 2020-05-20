//
// Copyright (c) 2010-2020 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using Antmicro.Renode.Core;
using Antmicro.Renode.Peripherals.Timers;
using Antmicro.Renode.Time;
using Antmicro.Renode.Logging;

namespace Antmicro.Renode.Peripherals.CPU
{
    public partial class SweRV_EH1 : RiscV32, IRiscVTimeProvider
    {
        public SweRV_EH1(Machine machine, IRiscVTimeProvider timeProvider = null, uint hartId = 0, PrivilegeArchitecture privilegeArchitecture = PrivilegeArchitecture.Priv1_10, string cpuType = "rv32imc") : base(timeProvider, cpuType, machine, hartId, privilegeArchitecture)
        {
            timer = new LimitTimer(machine.ClockSource, 1000000, this, nameof(timer), uint.MaxValue, Direction.Descending, false, autoUpdate: true);
            TlibSetCsrValidationLevel((uint)CSRValidationLevel.None);

            // register custom CSRs
            RegisterCSR((ulong)0xBC8, () => 0u, _ => {}); //meivt
            RegisterCSR((ulong)0xBC9, () => 0u, _ => {}); //meipt
            RegisterCSR((ulong)0xBCA, () => 0u, _ => {}); //meicpct
            RegisterCSR((ulong)0xBCB, () => 0u, _ => {}); //meicidpl
            RegisterCSR((ulong)0xBCC, () => 0u, _ => {}); //meicurpl
            RegisterCSR((ulong)0xFC8, () => 0u, _ => {}); //meihap
        }

        public ulong TimerValue
        {
            get
            {
                this.Log(LogLevel.Debug, "Returning time {0}", timer.Value);
                return timer.Value;
            }
        }

        private LimitTimer timer;
    }
}

