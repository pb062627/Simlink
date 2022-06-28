using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nini.Config;
using SimLink_HPC_NS;

namespace SIM_API_LINKS
{
    /// <summary>
    /// functionality related to hpc / compute environment
    /// some of this code may migrate into the simlink_hpc class
    ///     as of first integration (7/4/16), simlink_hpc is an external project
    ///     this is likely to be integrated eventually... but in early stage of frequent change, keeping separate.
    /// </summary>
    public partial class simlink
    {
        /// <summary>
        ///  check config xml for environment, and init object if needed....
        ///  for now, the Environment must be defined in the same xml
        ///  in the future, if needed, this function can be modified to look for a dif env xml.
        /// </summary>
        /// <param name="config"></param>
        public void InitHPCbyConfig(IConfig envConfig)
        {
            if (envConfig != null)
            {
                switch (envConfig.GetString("ComputeHost").ToLower())
                {
                    case "aws":
                        _hpc = new awsComputeWrapper();
                        _compute_env = ComputeEnvironment.AWS;
                        break;
                    case "condor":
                        _hpc = new condorWrapper();
                        _compute_env = ComputeEnvironment.Condor;
                        break;
                    case "local_hpc":                   //todo: check name...
                        _hpc = new LocalPCWrapper();
                        _compute_env = ComputeEnvironment.LocalViaHPC;
                        break;
                    default:
                        // this will run on local machine in "normal" format
                        _compute_env = ComputeEnvironment.LocalMachine;
                        break;
                }
                if (_compute_env != ComputeEnvironment.LocalMachine)
                {
                    _hpc.OpenEnvironment(envConfig);
                }
            }
            else
            {
                // no environment specified... so run on local machine
                _compute_env = ComputeEnvironment.LocalMachine;
            }

        }            
    }
}
